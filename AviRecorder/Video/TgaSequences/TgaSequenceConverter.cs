using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;
using AviRecorder.Imaging;
using AviRecorder.Video.Avi;
using AviRecorder.Video.Compression;

namespace AviRecorder.Video.TgaSequences
{
    public class TgaSequenceConverter : IDisposable
    {
        public const int MaxSupportedFrameRate = int.MaxValue - AviFileWriter.AudioSamples;

        private const int MaxSharingViolations = 50;
        private const int SharingViolationRetryDelay = 100;
        
        private TargaImageLoader _tgaLoader;
        private TargaImage _tga;
        private BitmapImage _bmp;
        private TgaToBmpConverter _converter;

        private int _frameRate;
        private int _frameBlendingIndex;
        private int _frameProcessIndex;
        private bool _deleteOnClose;
        private VideoCompressor _compressor;

        private int _frameIndex;
        private int _frameRateRemainder;
        private int _audioSamplesAvailable;

        private FileStream _audioStream;
        private byte[] _audioBuffer;
        private int _audioBufferLength;

        private AviFileWriter _avi;

        public TgaSequenceConverter()
        {
            _tgaLoader = new TargaImageLoader();
            _tga = new TargaImage();
            _bmp = new BitmapImage();
            _converter = new TgaToBmpConverter();

            _frameRate = 30;
            _frameBlendingIndex = 0;
            _frameProcessIndex = 0;
            _deleteOnClose = false;
            _compressor = null;
        }

        public int FrameRate
        {
            get => _frameRate;
            set
            {
                if (value < 1 || value > MaxSupportedFrameRate)
                    throw new ArgumentOutOfRangeException(nameof(value));

                CheckMovieStarted();
                _frameRate = value;
            }
        }

        public int FrameBlendingFactor
        {
            get => _frameBlendingIndex + 1;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                CheckMovieStarted();
                _frameBlendingIndex = value - 1;

                if (_frameProcessIndex > _frameBlendingIndex)
                    _frameProcessIndex = _frameBlendingIndex;
            }
        }

        public int FramesToProcess
        {
            get => _frameProcessIndex + 1;
            set
            {
                if (value < 1 || value > _frameBlendingIndex + 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                CheckMovieStarted();
                _frameProcessIndex = value - 1;
            }
        }

        public bool DeleteOnClose
        {
            get => _deleteOnClose;
            set
            {
                CheckMovieStarted();
                _deleteOnClose = value;
            }
        }

        public VideoCompressor Compressor
        {
            get => _compressor;
            set
            {
                if (value != null && value.IsCompressing)
                    throw new ArgumentException("Video compressor is already compressing.", nameof(value));

                CheckMovieStarted();
                _compressor = value;
            }
        }

        public bool MovieStarted => _avi != null;

        public void StartMovie(string tgaPath, string wavPath, string aviPath)
        {
            if (tgaPath == null)
                throw new ArgumentNullException(nameof(tgaPath));
            if (aviPath == null)
                throw new ArgumentNullException(nameof(aviPath));
            if (_avi != null)
                throw new InvalidOperationException("A movie is already in progress.");

            LoadTga(tgaPath, FileOptions.SequentialScan);

            _bmp.ChangeResolution(_tga.Width, _tga.Height);
            _converter.ChangeResolution(_tga.Width, _tga.Height);

            _frameIndex = 0;
            _frameRateRemainder = _frameRate - 1;
            _audioSamplesAvailable = 0;

            _audioStream = wavPath == null ? null : LoadWav(wavPath, FileOptions.SequentialScan);
            try
            {
                _avi = AviFileWriter.Create(aviPath, _frameRate, _bmp.InfoHeader, wavPath != null, _compressor);
                try
                {
                    if (_frameProcessIndex == 0)
                    {
                        _converter.Convert(_tga, _bmp);
                        WriteBmpToAvi();
                    }
                    else
                    {
                        _converter.Add(_tga);
                    }

                    if (_deleteOnClose)
                        File.Delete(tgaPath);
                }
                catch (Exception)
                {
                    _avi.Dispose();
                    _avi = null;

                    throw;
                }
            }
            catch (Exception)
            {
                if (_audioStream != null)
                {
                    _audioStream.Dispose();
                    _audioStream = null;
                }

                throw;
            }
        }

        public void ProcessFrame(string tgaPath)
        {
            if (tgaPath == null)
                throw new ArgumentNullException(nameof(tgaPath));

            if (_frameIndex < _frameProcessIndex)
            {
                LoadTga(tgaPath, _deleteOnClose ? FileOptions.DeleteOnClose | FileOptions.SequentialScan : FileOptions.SequentialScan);
                _converter.Add(_tga);
            }
            else if (_frameIndex == _frameProcessIndex)
            {
                LoadTga(tgaPath, _deleteOnClose ? FileOptions.DeleteOnClose | FileOptions.SequentialScan : FileOptions.SequentialScan);

                if (_frameProcessIndex == 0)
                {
                    _converter.Convert(_tga, _bmp);
                }
                else
                {
                    _converter.Add(_tga);
                    _converter.Get(_bmp);
                }

                WriteBmpToAvi();
            }
            else if (_deleteOnClose)
            {
                DeleteFile(tgaPath);
            }

            if (_frameIndex++ == _frameBlendingIndex)
                _frameIndex = 0;
        }

        public void ProcessAudio()
        {
            if (_audioStream == null)
                return;

            Debug.Assert(_audioSamplesAvailable <= int.MaxValue / 4);

            while (_audioSamplesAvailable > 0)
            {
                var bytesToRead = Math.Min(_audioSamplesAvailable * AviFileWriter.AudioSampleSize, AviFileWriter.AudioSamples * AviFileWriter.AudioSampleSize) - _audioBufferLength;
                int bytesRead;

                try
                {
                    bytesRead = _audioStream.Read(_audioBuffer, _audioBufferLength, bytesToRead);
                }
                catch (IOException ex)
                {
                    throw new RecoverableException(ex.Message, ex);
                }

                _audioBufferLength += bytesRead;

                if (_audioBufferLength >= AviFileWriter.AudioSampleSize)
                {
                    var bytesToWrite = _audioBufferLength & AviFileWriter.AudioWholeSamplesMask;

                    _avi.WriteAudioData(_audioBuffer, 0, bytesToWrite);

                    if (_audioBufferLength > bytesToWrite)
                        Buffer.BlockCopy(_audioBuffer, bytesToWrite, _audioBuffer, 0, _audioBufferLength - bytesToWrite);

                    _audioBufferLength -= bytesToWrite;
                    _audioSamplesAvailable -= (int)((uint)bytesToWrite >> 2);
                    continue;
                }

                if (bytesRead == 0)
                    break;
            }
        }

        public void ProcessAllAudio()
        {
            if (_audioStream == null)
                return;

            int size;

            do
            {
                if (_audioBufferLength >= AviFileWriter.AudioSampleSize)
                {
                    var bytesToWrite = _audioBufferLength & AviFileWriter.AudioWholeSamplesMask;

                    _avi.WriteAudioData(_audioBuffer, 0, bytesToWrite);

                    if (_audioBufferLength > bytesToWrite)
                        Buffer.BlockCopy(_audioBuffer, bytesToWrite, _audioBuffer, 0, _audioBufferLength - bytesToWrite);

                    _audioBufferLength -= bytesToWrite;
                }

                try
                {
                    size = _audioStream.Read(_audioBuffer, _audioBufferLength, AviFileWriter.AudioSamples * AviFileWriter.AudioSampleSize - _audioBufferLength);
                }
                catch (IOException ex)
                {
                    throw new RecoverableException(ex.Message, ex);
                }

                _audioBufferLength += size;
            } while (size > 0);

            _frameRateRemainder = _frameRate - 1;
            _audioSamplesAvailable = 0;
        }

        public void EndMovie()
        {
            try
            {
                if (_audioStream != null)
                {
                    _audioStream.Dispose();

                    if (_deleteOnClose)
                        DeleteFile(_audioStream.Name);

                    _audioStream = null;
                }
            }
            finally
            {
                if (_avi != null)
                {
                    _avi.Dispose();
                    _avi = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            try
            {
                EndMovie();
            }
            finally
            {
                _tga.Clear();
                _bmp.Clear();
                _converter.Clear();

                _compressor = null;
                _audioBuffer = null;
            }
        }

        private static void DeleteFile(string path)
        {
            try
            {
                var sharingViolations = 0;

                while (true)
                {
                    try
                    {
                        File.Delete(path);
                        break;
                    }
                    catch (IOException ex) when ((uint)ex.HResult == 0x80070020) /* Failure, Win32, ERROR_SHARING_VIOLATION */
                    {
                        if (++sharingViolations == MaxSharingViolations)
                            throw;

                        Thread.Sleep(SharingViolationRetryDelay);
                    }
                }
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new RecoverableException("An error occured while attempting to delete the file: " + ex.Message, ex);
            }
        }

        private void CheckMovieStarted()
        {
            if (MovieStarted)
                throw new InvalidOperationException("Settings must not be changed while a movie is in progress.");
        }

        private void LoadTga(string path, FileOptions options)
        {
            try
            {
                var sharingViolations = 0;

                while (true)
                {
                    try
                    {
                        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 1, options))
                            _tgaLoader.Load(fs, _tga);

                        break;
                    }
                    catch (IOException ex) when ((uint)ex.HResult == 0x80070020) /* Failure, Win32, ERROR_SHARING_VIOLATION */
                    {
                        if (++sharingViolations == MaxSharingViolations)
                            throw;
                        
                        Thread.Sleep(SharingViolationRetryDelay);
                    }
                }
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new RecoverableException("An error occured while attempting to load the frame into memory: " + ex.Message, ex);
            }
        }

        private FileStream LoadWav(string path, FileOptions options)
        {
            try
            {
                var result = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1, options);

                try
                {
                    result.Seek(9 * sizeof(uint) + 8 * sizeof(ushort), SeekOrigin.Current);

                    if (_audioBuffer == null)
                        _audioBuffer = new byte[AviFileWriter.AudioSamples * AviFileWriter.AudioSampleSize];

                    _audioBufferLength = 0;
                }
                catch (Exception)
                {
                    result.Dispose();
                    throw;
                }

                return result;
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new RecoverableException("An error occured while attempting to open the audio stream: " + ex.Message, ex);
            }
        }

        private void WriteBmpToAvi()
        {
            _avi.WriteVideoData(_bmp.RawData, 0, _bmp.RawData.Length, true);

            var samplesToAdd = Math.DivRem(44100 + _frameRateRemainder, _frameRate, out _frameRateRemainder);

            _audioSamplesAvailable += samplesToAdd;

            if (_audioSamplesAvailable > int.MaxValue / 4)
                ProcessAllAudio();
        }
    }
}
