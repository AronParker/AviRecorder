using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AviRecorder.Imaging;
using AviRecorder.Video.Compression;
using static AviRecorder.NativeMethods;

namespace AviRecorder.Video.Avi
{
    public abstract class AviFileWriter : IDisposable
    {
        public const int MaxWidth = short.MaxValue;
        public const int MaxHeight = short.MaxValue;

        public const int AudioSampleSize = 4;
        public const int AudioSamples = 44100;
        public const int AudioWholeSamplesMask = ~3;

        private const int ChunkHeaderSize = 2 * sizeof(uint);
        private const int MaxChunkDataSize = int.MaxValue - 8 * sizeof(uint);

        private const int RiffAviListMaxSize = int.MaxValue / 2;
        private const int RiffAviXListMaxSize = int.MaxValue;

        private const int OldIndexInitialCapacity = 512;
        private const int SuperIndexCapacity = 1024;
        private const int StandardIndexInitialCapacity = 256;

        private FileStream _fs;
        private BinaryWriter _bw;

        private int _frameRate;
        private uint _videoChunkId;
        private uint _videoFccHandler;
        private uint _videoSampleSize;
        private BitmapInfo _videoFormat;
        private bool _audio;

        private int _videoLengthInFirstRiff;
        private int _videoLength;
        private int _videoChunkMaxSize;
        
        private int _audioLength;
        private int _audioChunkMaxSize;

        private List<AviOldIndexEntry> _legacyIndex;
        private AviSuperIndex _videoSuperIndex;
        private AviSuperIndex _audioSuperIndex;

        private bool _firstRiff;
        private long _currentRiff;
        private long _currentMovi;
        private bool _disposed;
        
        private AviFileWriter(string path, int frameRate, uint videoChunkId, uint videoFccHandler, uint videoSampleSize, BitmapInfo videoFormat, bool audio)
        {
            _fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            _bw = new BinaryWriter(_fs, Encoding.ASCII, true);
            
            _frameRate = frameRate;
            _videoChunkId = videoChunkId;
            _videoFccHandler = videoFccHandler;
            _videoSampleSize = videoSampleSize;
            _videoFormat = videoFormat;
            _audio = audio;

            _videoLengthInFirstRiff = 0;
            _videoLength = 0;
            _videoChunkMaxSize = 0;

            _audioLength = 0;
            _audioChunkMaxSize = 0;

            _legacyIndex = new List<AviOldIndexEntry>(OldIndexInitialCapacity);
            _videoSuperIndex = new AviSuperIndex(SuperIndexCapacity, StandardIndexInitialCapacity);

            if (audio)
                _audioSuperIndex = new AviSuperIndex(SuperIndexCapacity, StandardIndexInitialCapacity);

            try
            {
                CreateFirstRiff();
            }
            catch (Exception)
            {
                _bw.Dispose();
                _fs.Dispose();
                throw;
            }
        }

        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">width is not positive or is larger than <see cref="MaxWidth"/>. -or- height is not positive or is larger than <see cref="MaxHeight"/>. -or- frameRate is not positive.</exception>
        /// <exception cref="System.ArgumentException">path is a zero-length string, contains only white space, contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.NotSupportedException">path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        public static AviFileWriter Create(string path, int width, int height, int frameRate)
        {
            return Create(path, width, height, frameRate, false, null);
        }

        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">width is not positive or is larger than <see cref="MaxWidth"/>. -or- height is not positive or is larger than <see cref="MaxHeight"/>. -or- frameRate is not positive.</exception>
        /// <exception cref="System.ArgumentException">path is a zero-length string, contains only white space, contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.NotSupportedException">path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        public static AviFileWriter Create(string path, int width, int height, int frameRate, bool audio)
        {
            return Create(path, width, height, frameRate, audio, null);
        }

        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">width is not positive or is larger than <see cref="MaxWidth"/>. -or- height is not positive or is larger than <see cref="MaxHeight"/>. -or- frameRate is not positive.</exception>
        /// <exception cref="System.ArgumentException">path is a zero-length string, contains only white space, contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.NotSupportedException">path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        /// <exception cref="System.InvalidOperationException">Video compressor is already compressing.</exception>
        /// <exception cref="AviRecorder.Video.Compression.VideoCompressorException">An error occured while accessing the internal video compressor.</exception>
        public static AviFileWriter Create(string path, int width, int height, int frameRate, bool audio, VideoCompressor compressor)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (width < 1 || width > MaxHeight)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 1 || height > MaxHeight)
                throw new ArgumentOutOfRangeException(nameof(height));
            if (frameRate < 1)
                throw new ArgumentOutOfRangeException(nameof(frameRate));

            return InternalCreate(path, frameRate, BitmapInfo.CreateRgb24(width, height), audio, compressor);
        }

        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">frameRate is not positive.</exception>
        /// <exception cref="System.ArgumentException">width of videoFormat is not positive or is larger than <see cref="MaxWidth"/>. -or- height of videoFormat is not positive or is larger than <see cref="MaxHeight"/>. -or- path is a zero-length string, contains only white space, contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="System.NotSupportedException">path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        /// <exception cref="System.InvalidOperationException">Video compressor is already compressing.</exception>
        /// <exception cref="AviRecorder.Video.Compression.VideoCompressorException">An error occured while accessing the internal video compressor.</exception>
        public static AviFileWriter Create(string path, int frameRate, BitmapInfo videoFormat, bool audio, VideoCompressor compressor)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (frameRate < 1)
                throw new ArgumentOutOfRangeException(nameof(frameRate));
            if (videoFormat == null)
                throw new ArgumentNullException(nameof(videoFormat));

            var width = videoFormat.Width;

            if (width < 1 || width > MaxWidth)
                throw new ArgumentException("The width of the video format is out of range.", nameof(videoFormat));

            var height = Math.Abs(videoFormat.Height);

            if (height < 1 || height > MaxHeight)
                throw new ArgumentException("The height of the video format is out of range.", nameof(videoFormat));

            return InternalCreate(path, frameRate, videoFormat, audio, compressor);
        }

        private static AviFileWriter InternalCreate(string path, int frameRate, BitmapInfo videoFormat, bool audio, VideoCompressor compressor)
        {
            if (compressor == null)
                return new UncompressedAviFileWriter(path, frameRate, videoFormat, audio);
            
            compressor.Begin(videoFormat);

            try
            {
                return new CompressedAviFileWriter(path, frameRate, compressor, audio);
            }
            catch (Exception)
            {
                compressor.End();
                throw;
            }
        }

        public string Path => _fs.Name;

        public int Width => _videoFormat.Width;

        public int Height => Math.Abs(_videoFormat.Height);

        public int FrameRate => _frameRate;

        public abstract BitmapInfo VideoInputFormat { get; }

        public BitmapInfo VideoOutputFormat => _videoFormat;

        public bool HasAudio => _audio;

        /// <exception cref="System.ArgumentNullException">buffer is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">offset, count or duration is out of range.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.InvalidOperationException">Video compressor is not compressing.</exception>
        /// <exception cref="VideoCompressorException">An error occured while accessing the internal video compressor.</exception>
        public abstract void WriteVideoData(byte[] buffer, int offset, int count, bool keyFrame);

        /// <exception cref="System.ArgumentNullException">buffer is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">offset, count or duration is out of range.</exception>
        /// <exception cref="System.ArgumentException">count is not a multiple of the audio sample size.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.NotSupportedException">The current avi riff file writer instance does not support audio.</exception>
        public void WriteAudioData(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > Math.Min((uint)buffer.Length - (uint)offset, MaxChunkDataSize))
                throw new ArgumentOutOfRangeException(nameof(count));
            if ((count & AudioWholeSamplesMask) != count)
                throw new ArgumentException("Partial audio samples are not supported.", nameof(count));
            if (!_audio)
                throw new NotSupportedException();

            CreateNewRiffIfNeeded(count);

            var chunkPos = _fs.Position;

            WriteChunk(FourCC.wb01, buffer, offset, count);
            WriteAudioMetadata(chunkPos, count);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            try
            {
                FlushStandardIndex(ref _videoSuperIndex, FourCC.ix00, _videoChunkId);

                if (_audio)
                    FlushStandardIndex(ref _audioSuperIndex, FourCC.ix01, FourCC.wb01);

                CloseRiff();
                WriteHeader(true);
            }
            finally
            {
                _bw.Dispose();
                _fs.Dispose();

                _disposed = true;
            }
        }

        private void WriteVideoData(byte[] buffer, int offset, int count, bool keyFrame, int duration)
        {
            CreateNewRiffIfNeeded(count);

            var chunkPos = _fs.Position;

            WriteChunk(_videoChunkId, buffer, offset, count);

            WriteVideoMetadata(_videoChunkId, chunkPos, count, keyFrame, duration);
        }

        private void WriteVideoMetadata(uint fcc, long chunkPos, int size, bool keyFrame, int frames)
        {
            if (_firstRiff)
                _videoLengthInFirstRiff += frames;

            _videoLength += frames;

            if (size > _videoChunkMaxSize)
                _videoChunkMaxSize = size;

            var indexed = IndexChunk(ref _videoSuperIndex, FourCC.ix00, fcc, size, chunkPos, keyFrame, frames);

            Debug.Assert(indexed);
        }

        private void WriteAudioMetadata(long chunk, int size)
        {
            var samples = size / 4;

            _audioLength += samples;

            if (size > _audioChunkMaxSize)
                _audioChunkMaxSize = size;

            var indexed = IndexChunk(ref _audioSuperIndex, FourCC.ix01, FourCC.wb01, size, chunk, true, samples);

            Debug.Assert(indexed);
        }

        private void WriteHeader(bool rewrite)
        {
            if (rewrite)
                _fs.Position = 3 * sizeof(int);

            var hdrl = CreateList(FourCC.LIST, FourCC.hdrl);

            // AviMainHeader
            _bw.Write(FourCC.avih);
            _bw.Write(14U * sizeof(uint));
            _bw.Write(1000000U / (uint)_frameRate); // dwMicroSecPerFrame
            _bw.Write((uint)_frameRate * (_audio ? (uint)_videoChunkMaxSize + (uint)_audioChunkMaxSize : (uint)_videoChunkMaxSize)); // dwMaxBytesPerSec
            _bw.Write(0U); // dwPaddingGranularity
            _bw.Write((uint)(AviMainHeaderFlags.HasIndex | AviMainHeaderFlags.IsInterleaved | AviMainHeaderFlags.TrustCkType)); // dwFlags
            _bw.Write((uint)_videoLengthInFirstRiff); // dwTotalFrames
            _bw.Write(0U); // dwInitialFrames
            _bw.Write(_audio ? 2U : 1U); // dwStreams
            _bw.Write((uint)_videoChunkMaxSize); // dwSuggestedBufferSize
            _bw.Write((uint)Width); // dwWidth
            _bw.Write((uint)Height); // dwHeight
            for (var i = 0; i < 4; i++) // dwReserved
                _bw.Write(0U);

            var strl1 = CreateList(FourCC.LIST, FourCC.strl);

            // AviStreamHeader
            _bw.Write(FourCC.strh);
            _bw.Write(14U * sizeof(uint));
            _bw.Write(FourCC.vids); // fccType
            _bw.Write(_videoFccHandler); // fccHandler
            _bw.Write(0U); // dwFlags
            _bw.Write((ushort)0); // wPriority
            _bw.Write((ushort)0); // wLanguage
            _bw.Write(0U); // dwInitialFrames
            _bw.Write(1U); // dwScale
            _bw.Write(_frameRate); // dwRate
            _bw.Write(0U); // dwStart
            _bw.Write((uint)_videoLength); // dwLength
            _bw.Write((uint)_videoChunkMaxSize); // dwSuggestedBufferSize
            _bw.Write(0xFFFFFFFF); // dwQuality
            _bw.Write(_videoSampleSize); // dwSampleSize
            _bw.Write((short)0); // rcFrame.left
            _bw.Write((short)0); // rcFrame.top
            _bw.Write((short)Width); // rcFrame.right
            _bw.Write((short)Height); // rcFrame.bottom

            // BITMAPINFO
            if (rewrite)
                _fs.Seek(2 * sizeof(uint) + _videoFormat.Size, SeekOrigin.Current);
            else
                WriteChunk(FourCC.strf, _videoFormat.RawData);

            // AviSuperIndex
            WriteSuperIndex(ref _videoSuperIndex, _videoChunkId, rewrite ? _videoSuperIndex.EntriesInUse : SuperIndexCapacity);

            CloseList(strl1);

            if (_audio)
            {
                var strl2 = CreateList(FourCC.LIST, FourCC.strl);

                // AviStreamHeader
                _bw.Write(FourCC.strh);
                _bw.Write(14U * sizeof(uint));
                _bw.Write(FourCC.auds); // fccType
                _bw.Write(0U); // fccHandler
                _bw.Write(0U); // dwFlags
                _bw.Write((ushort)0); // wPriority
                _bw.Write((ushort)0); // wLanguage
                _bw.Write(0U); // dwInitialFrames
                _bw.Write(4U); // dwScale
                _bw.Write(44100U * 4U); // dwRate
                _bw.Write(0U); // dwStart
                _bw.Write((uint)_audioLength); // dwLength
                _bw.Write((uint)_audioChunkMaxSize); // dwSuggestedBufferSize
                _bw.Write(0xFFFFFFFF); // dwQuality
                _bw.Write(4U); // dwSampleSize
                _bw.Write((short)0); // rcFrame.left
                _bw.Write((short)0); // rcFrame.top
                _bw.Write((short)0); // rcFrame.right
                _bw.Write((short)0); // rcFrame.bottom

                // WaveFormatEx
                if (rewrite)
                {
                    _fs.Seek(4 * sizeof(uint) + 5 * sizeof(ushort), SeekOrigin.Current);
                }
                else
                {
                    _bw.Write(FourCC.strf);
                    _bw.Write(2U * sizeof(uint) + 5U * sizeof(ushort));
                    _bw.Write((ushort)1); // wFormatTag
                    _bw.Write((ushort)2); // nChannels
                    _bw.Write(44100U); // nSamplesPerSec
                    _bw.Write(44100U * 4U); // nAvgBytesPerSec
                    _bw.Write((ushort)4); // nBlockAlign
                    _bw.Write((ushort)16); // wBitsPerSample
                    _bw.Write((ushort)0); // cbSize
                }
                
                // AviSuperIndex
                WriteSuperIndex(ref _audioSuperIndex, FourCC.wb01, rewrite ? _audioSuperIndex.EntriesInUse : SuperIndexCapacity);

                CloseList(strl2);
            }

            var odml = CreateList(FourCC.LIST, FourCC.odml);

            // AviExtHeader
            _bw.Write(FourCC.dmlh); // fcc
            _bw.Write(62U * sizeof(uint)); // cb
            _bw.Write((uint)_videoLength); // dwGrandFrames
            for (var i = 0; i < 61; i++) // // dwReserved
                _bw.Write(0U);

            CloseList(odml);
            CloseList(hdrl);
        }

        private bool IndexChunk(ref AviSuperIndex superIndex, uint fcc, uint chunkId, int size, long chunk, bool keyFrame, int duration)
        {
            if (_firstRiff)
            {
                var oldIndexEntry = default(AviOldIndexEntry);
                oldIndexEntry.dwChunkId = chunkId;
                oldIndexEntry.dwFlags = keyFrame ? AviifKeyframe : 0;
                oldIndexEntry.dwOffset = (uint)(chunk - _currentMovi);
                oldIndexEntry.dwSize = (uint)size;
                _legacyIndex.Add(oldIndexEntry);
            }

            long relativeOffset;

            if (superIndex.LastIndexEntries.Count == 0 || (relativeOffset = chunk + ChunkHeaderSize - superIndex.LastBaseOffset) > uint.MaxValue)
            {
                if (superIndex.LastIndexEntries.Count > 0 && !FlushStandardIndex(ref superIndex, fcc, chunkId))
                    return false;

                superIndex.LastBaseOffset = chunk + ChunkHeaderSize;
                relativeOffset = 0;
            }

            var stdIndexEntry = default(AviStdIndexEntry);
            stdIndexEntry.dwOffset = (uint)relativeOffset;
            stdIndexEntry.dwSize = keyFrame ? (uint)size : (uint)size | 0x80000000;
            superIndex.LastIndexEntries.Add(stdIndexEntry);
            superIndex.SuperIndexEntries[superIndex.EntriesInUse].dwDuration += (uint)duration;
            return true;
        }

        private void WriteSuperIndex(ref AviSuperIndex superIndex, uint chunkId, int entriesToWrite)
        {
            // AviSuperIndex
            _bw.Write(FourCC.indx);
            _bw.Write(5U * sizeof(uint) + sizeof(ushort) + 2 * sizeof(byte) + SuperIndexCapacity * (uint)Marshal.SizeOf<AviSuperIndexEntry>());
            _bw.Write((ushort)4); // wLongsPerEntry
            _bw.Write((byte)0); // bIndexSubType
            _bw.Write((byte)0); // bIndexType
            _bw.Write((uint)superIndex.EntriesInUse); // nEntriesInUse
            _bw.Write(chunkId); // dwChunkId

            for (var i = 0; i < 3; i++) // dwReserved
                _bw.Write(0U);

            for (var i = 0; i < entriesToWrite; i++) // aIndex
            {
                var entry = superIndex.SuperIndexEntries[i];

                _bw.Write(entry.qwOffset);
                _bw.Write(entry.dwSize);
                _bw.Write(entry.dwDuration);
            }

            var bytesRemaining = (SuperIndexCapacity - entriesToWrite) * Marshal.SizeOf<AviSuperIndexEntry>();

            if (bytesRemaining > 0)
                _fs.Seek(bytesRemaining, SeekOrigin.Current);
        }

        private bool FlushStandardIndex(ref AviSuperIndex superIndex, uint fcc, uint chunkId)
        {
            if (superIndex.LastIndexEntries.Count == 0)
                return true;

            if (superIndex.EntriesInUse >= SuperIndexCapacity)
                return false;

            var pos = (ulong)_fs.Position;
            var size = sizeof(ulong) + 3U * sizeof(uint) + sizeof(ushort) + 2U * sizeof(byte) + (uint)superIndex.LastIndexEntries.Count * 2 * sizeof(uint);

            // AviStdIndex
            _bw.Write(fcc);
            _bw.Write(size);
            _bw.Write((ushort)2); // wLongsPerEntry
            _bw.Write((byte)0); // bIndexSubType
            _bw.Write((byte)1); // bIndexType
            _bw.Write((uint)superIndex.LastIndexEntries.Count); // nEntriesInUse
            _bw.Write(chunkId); // dwChunkId
            _bw.Write(superIndex.LastBaseOffset); // qwBaseOffset
            _bw.Write(0); // dwReserved_3

            foreach (var entry in superIndex.LastIndexEntries) // aIndex
            {
                _bw.Write(entry.dwOffset);
                _bw.Write(entry.dwSize);
            }

            superIndex.LastIndexEntries.Clear();
            superIndex.SuperIndexEntries[superIndex.EntriesInUse].qwOffset = pos;
            superIndex.SuperIndexEntries[superIndex.EntriesInUse].dwSize = ChunkHeaderSize + size;
            superIndex.EntriesInUse++;
            return true;
        }

        private void CreateFirstRiff()
        {
            _firstRiff = true;
            _currentRiff = CreateList(FourCC.RIFF, FourCC.AVI);
            WriteHeader(false);
            _currentMovi = CreateList(FourCC.LIST, FourCC.movi);
        }

        private void CreateNewRiffIfNeeded(int chunkDataSize)
        {
            Debug.Assert(chunkDataSize < MaxChunkDataSize);

            var sizeInBytes = ChunkHeaderSize + chunkDataSize;

            if (_firstRiff)
                sizeInBytes += Marshal.SizeOf<AviOldIndexEntry>();

            var bytesRemaining = GetBytesRemainingThisRiff();

            if (sizeInBytes <= bytesRemaining)
                return;

            CloseRiff();
            _firstRiff = false;
            _currentRiff = CreateList(FourCC.RIFF, FourCC.AVIX);
            _currentMovi = CreateList(FourCC.LIST, FourCC.movi);
        }

        private int GetBytesRemainingThisRiff()
        {
            var riffSize = GetListSize(_currentRiff);

            Debug.Assert(riffSize >= 6 * sizeof(uint) && riffSize <= RiffAviXListMaxSize);

            if (_firstRiff)
            {
                var idx1Size = 2 * sizeof(uint) + _legacyIndex.Count * Marshal.SizeOf<AviOldIndexEntry>();

                return RiffAviListMaxSize - idx1Size - (int)riffSize;
            }

            return RiffAviXListMaxSize - (int)riffSize;
        }

        private void CloseRiff()
        {
            CloseList(_currentMovi);

            if (_firstRiff)
            {
                _bw.Write(FourCC.idx1);
                _bw.Write(_legacyIndex.Count * Marshal.SizeOf<AviOldIndexEntry>());

                foreach (var entry in _legacyIndex)
                {
                    _bw.Write(entry.dwChunkId);
                    _bw.Write(entry.dwFlags);
                    _bw.Write(entry.dwOffset);
                    _bw.Write(entry.dwSize);
                }
            }

            CloseList(_currentRiff);
        }

        private long CreateList(uint listType, uint fcc)
        {
            _bw.Write(listType);
            _bw.Write(0);

            var list = _fs.Position;

            _bw.Write(fcc);

            return list;
        }

        private long GetListSize(long list)
        {
            return _fs.Position - list;
        }

        private void CloseList(long list)
        {
            var size = _fs.Position - list;

            Debug.Assert(size >= 0 && size <= int.MaxValue);

            _fs.Position = list - sizeof(int);
            _bw.Write((int)size);
            _fs.Position = list + size + (size & 1);
        }

        private void WriteChunk(uint fcc, byte[] data)
        {
            _bw.Write(fcc);
            _bw.Write(data.Length);
            _fs.Write(data, 0, data.Length);

            if ((data.Length & 1) != 0)
                _fs.WriteByte(0);
        }

        private void WriteChunk(uint fcc, byte[] data, int offset, int count)
        {
            _bw.Write(fcc);
            _bw.Write(count);

            _fs.Write(data, offset, count);

            if ((count & 1) != 0)
                _fs.WriteByte(0);
        }

        private struct AviSuperIndex
        {
            public AviSuperIndexEntry[] SuperIndexEntries;
            public int EntriesInUse;
            public long LastBaseOffset;
            public List<AviStdIndexEntry> LastIndexEntries;

            public AviSuperIndex(int superIndexEntries, int newIndexInitialCapacity)
            {
                SuperIndexEntries = new AviSuperIndexEntry[superIndexEntries];
                EntriesInUse = 0;
                LastBaseOffset = 0;
                LastIndexEntries = new List<AviStdIndexEntry>(newIndexInitialCapacity);
            }
        }

        private sealed class UncompressedAviFileWriter : AviFileWriter
        {
            public UncompressedAviFileWriter(string path, int frameRate, BitmapInfo videoFormat, bool audio)
                : base(path, frameRate, FourCC.db00, FourCC.DIB, videoFormat.ImageSize, videoFormat, audio)
            {
            }

            public override BitmapInfo VideoInputFormat => _videoFormat;

            public override void WriteVideoData(byte[] buffer, int offset, int count, bool keyFrame)
            {
                if (buffer == null)
                    throw new ArgumentNullException(nameof(buffer));
                if ((uint)offset > (uint)buffer.Length)
                    throw new ArgumentOutOfRangeException(nameof(offset));
                if ((uint)count > Math.Min((uint)buffer.Length - (uint)offset, MaxChunkDataSize))
                    throw new ArgumentOutOfRangeException(nameof(count));
                if ((uint)count % _videoSampleSize != 0)
                    throw new ArgumentException("Partial video samples are not supported.", nameof(count));

                WriteVideoData(buffer, offset, count, true, (int)((uint)count / _videoSampleSize));
            }
        }

        private sealed class CompressedAviFileWriter : AviFileWriter
        {
            private VideoCompressor _compressor;

            public CompressedAviFileWriter(string path, int frameRate, VideoCompressor compressor, bool audio)
                : base(path, frameRate, FourCC.dc00, compressor.Info.FccHandler, 0, compressor.OutputFormat, audio)
            {
                _compressor = compressor;
            }

            public override BitmapInfo VideoInputFormat => _compressor.InputFormat;

            public override void WriteVideoData(byte[] buffer, int offset, int count, bool keyFrame)
            {
                if (buffer == null)
                    throw new ArgumentNullException(nameof(buffer));
                if ((uint)offset > (uint)buffer.Length)
                    throw new ArgumentOutOfRangeException(nameof(offset));
                if ((uint)count > Math.Min((uint)buffer.Length - (uint)offset, MaxChunkDataSize))
                    throw new ArgumentOutOfRangeException(nameof(count));

                var size = _compressor.Compress(buffer, offset, count, ref keyFrame);

                WriteVideoData(_compressor.OutputBuffer, 0, size, keyFrame, 1);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && !_disposed)
                    _compressor.End();

                base.Dispose(disposing);
            }
        }
    }
}
