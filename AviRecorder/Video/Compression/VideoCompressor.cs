using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AviRecorder.Imaging;
using AviRecorder.Video.Avi;
using static AviRecorder.NativeMethods;

namespace AviRecorder.Video.Compression
{
    public sealed class VideoCompressor : IDisposable
    {
        private VideoCompressorHandle _hic;
        private BitmapInfo _inputFormat;
        private BitmapInfo _outputFormat;
        private byte[] _outputBuffer;

        private VideoCompressor(VideoCompressorHandle hic, VideoCompressorInfo info)
        {
            _hic = hic;
            Info = info;
            IsCompressing = false;
            FrameIndex = -1;
        }

        public VideoCompressorInfo Info { get; }
        
        public int StateSize {
            get
            {
                try
                {
                    return ICSendMessage(_hic, IcMessage.GetState, IntPtr.Zero, IntPtr.Zero).ToInt32();
                }
                catch (OverflowException ex)
                {
                    throw new VideoCompressorException("The video compressor returned a value out of range.", ex);
                }
            }
    }

        public bool HasConfigureDialog => ICSendMessage(_hic, IcMessage.Configure, (IntPtr)(-1), IntPtr.Zero) == IntPtr.Zero;

        public bool HasAboutDialog => ICSendMessage(_hic, IcMessage.About, (IntPtr)(-1), IntPtr.Zero) == IntPtr.Zero;

        public bool IsCompressing { get; private set; }

        public int FrameIndex { get; private set; }

        public BitmapInfo InputFormat
        {
            get
            {
                if (!IsCompressing)
                    throw new InvalidOperationException();

                return _inputFormat;
            }
        }

        public BitmapInfo OutputFormat
        {
            get
            {
                if (!IsCompressing)
                    throw new InvalidOperationException();

                return _outputFormat;
            }
        }

        public byte[] OutputBuffer
        {
            get
            {
                if (!IsCompressing)
                    throw new InvalidOperationException();

                return _outputBuffer;
            }
        }

        public void Dispose()
        {
            if (IsCompressing)
            {
                var result = ICSendMessage(_hic, IcMessage.CompressEnd, IntPtr.Zero, IntPtr.Zero);
                Debug.Assert(result == IntPtr.Zero);
            }

            _hic.Dispose();
            _inputFormat = null;
            _outputFormat = null;
            _outputBuffer = null;

            IsCompressing = false;
            FrameIndex = -1;
        }

        public static VideoCompressor Open(uint fcc)
        {
            var compressor = TryOpen(fcc);

            if (compressor == null)
                throw new VideoCompressorException("Failed to open video compressor.");

            return compressor;
        }

        public static VideoCompressor Open(VideoCompressorInfo info)
        {
            var compressor = TryOpen(info);

            if (compressor == null)
                throw new VideoCompressorException("Failed to open video compressor.");

            return compressor;
        }

        public static VideoCompressor TryOpen(uint fcc)
        {
            var hic = ICOpen(FourCC.VIDC, fcc, IcMode.Compress);

            if (hic.IsInvalid)
                return null;

            var icInfo = default(ICINFO);

            if (ICGetInfo(hic, ref icInfo, (uint)Marshal.SizeOf<ICINFO>()) == IntPtr.Zero)
            {
                hic.Dispose();
                return null;
            }

            return new VideoCompressor(hic, new VideoCompressorInfo(ref icInfo));
        }

        public static VideoCompressor TryOpen(VideoCompressorInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            var hic = ICOpen(FourCC.VIDC, info.FccHandler, IcMode.Compress);

            if (hic.IsInvalid)
                return null;

            return new VideoCompressor(hic, info);
        }

        public void ShowConfigureDialog()
        {
            ShowConfigureDialog(null);
        }

        public void ShowConfigureDialog(IWin32Window owner)
        {
            var configure = ICSendMessage(_hic, IcMessage.Configure, owner?.Handle ?? IntPtr.Zero, IntPtr.Zero);

            if (configure != IntPtr.Zero)
            {
                var error = (VideoCompressorError)configure.ToInt64();
                throw new VideoCompressorException("Failed to notify the video compression driver to display its configuration dialog box. Error code: " + error.ToString() + ".", error);
            }
        }

        public void ShowAboutDialog()
        {
            ShowAboutDialog(null);
        }

        public void ShowAboutDialog(IWin32Window owner)
        {
            var about = ICSendMessage(_hic, IcMessage.About, owner?.Handle ?? IntPtr.Zero, IntPtr.Zero);

            if (about != IntPtr.Zero)
            {
                var error = (VideoCompressorError)about.ToInt64();
                throw new VideoCompressorException("Failed to notify the video compression driver to display its About dialog box. Error code: " + error.ToString() + ".", error);
            }
        }

        public byte[] GetState()
        {
            var getStateSize = StateSize;

            if (getStateSize <= 0)
                return null;

            var state = new byte[getStateSize];
            var getState = ICSendMessage(_hic, IcMessage.GetState, state, (IntPtr)state.Length);

            if (getState != IntPtr.Zero)
            {
                var error = (VideoCompressorError)getState.ToInt64();
                throw new VideoCompressorException("Failed to notify the video compression driver to return its current configuration. Error code: " + error.ToString() + ".", error);
            }

            return state;
        }
        
        public void SetState(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length == 0)
                throw new ArgumentException("The buffer must not be empty.", nameof(buffer));

            var setState = ICSendMessage(_hic, IcMessage.SetState, buffer, (IntPtr)buffer.Length);

            if (setState == IntPtr.Zero)
                throw new VideoCompressorException("Failed to notify the video compression driver to change its current configuration.");
        }

        public void ResetState()
        {
            ICSendMessage(_hic, IcMessage.SetState, IntPtr.Zero, IntPtr.Zero);
        }

        /// <exception cref="System.ArgumentNullException">bitmapInfo is null.</exception>
        public bool Supports(BitmapInfo bitmapInfo)
        {
            if (bitmapInfo == null)
                throw new ArgumentNullException(nameof(bitmapInfo));

            return ICSendMessage(_hic, IcMessage.CompressQuery, bitmapInfo.RawData, IntPtr.Zero) == IntPtr.Zero;
        }

        /// <exception cref="System.ArgumentNullException">bitmapInfo is null.</exception>
        /// <exception cref="System.InvalidOperationException">Video compressor is already compressing.</exception>
        /// <exception cref="VideoCompressorException">An error occured while accessing the internal video compressor.</exception>
        public void Begin(BitmapInfo format)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (IsCompressing)
                throw new InvalidOperationException("Video compressor is already compressing.");

            var compressQuery = ICSendMessage(_hic, IcMessage.CompressQuery, format.RawData, IntPtr.Zero);

            if (compressQuery != IntPtr.Zero)
            {
                var error = (VideoCompressorError)compressQuery.ToInt64();
                throw new VideoCompressorException("Failed to query the video compression driver whether it supports the specified input format. Error code: " + error.ToString() + ".", error);
            }

            try
            {
                var outputFormatSize = ICSendMessage(_hic, IcMessage.CompressGetFormat, format.RawData, IntPtr.Zero).ToInt32();

                if (_outputFormat == null)
                    _outputFormat = new BitmapInfo(outputFormatSize);
                else if (_outputFormat.RawData.Length != outputFormatSize)
                    _outputFormat.Size = outputFormatSize;

                var compressGetFormat = ICSendMessage(_hic, IcMessage.CompressGetFormat, format.RawData, _outputFormat.RawData);

                if (compressGetFormat != IntPtr.Zero)
                {
                    var error = (VideoCompressorError)compressGetFormat.ToInt64();
                    throw new VideoCompressorException("Failed to request the output format of the compressed data from the video compression driver. Error code: " + error.ToString() + ".", error);
                }

                var compressGetSize = ICSendMessage(_hic, IcMessage.CompressGetSize, format.RawData, _outputFormat.RawData).ToInt32();

                if (_outputBuffer == null || _outputBuffer.Length != compressGetSize)
                    _outputBuffer = new byte[compressGetSize];

                var compressBegin = ICSendMessage(_hic, IcMessage.CompressBegin, format.RawData, _outputFormat.RawData);

                if (compressBegin != IntPtr.Zero)
                {
                    var error = (VideoCompressorError)compressBegin.ToInt64();
                    throw new VideoCompressorException("Failed to notify the video compression driver to prepare to compress data. Error code: " + error.ToString() + ".", error);
                }

                _inputFormat = format;
                IsCompressing = true;
                FrameIndex = 0;
            }
            catch (OverflowException ex)
            {
                throw new VideoCompressorException("The video compressor returned a value out of range.", ex);
            }
        }

        /// <exception cref="System.ArgumentNullException">buffer is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">offset or count is out of range.</exception>
        /// <exception cref="System.InvalidOperationException">Video compressor is not compressing.</exception>
        /// <exception cref="VideoCompressorException">An error occured while accessing the internal video compressor.</exception>
        public unsafe int Compress(byte[] buffer, int offset, int count, ref bool keyFrame)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (!IsCompressing)
                throw new InvalidOperationException("Video compressor is not compressing.");

            if ((uint)count < _inputFormat.ImageSize)
                throw new ArgumentOutOfRangeException(nameof(count), "count must be equal or larger than the image size specified in the input format.");

            uint dwFlags;
            uint compress;

            fixed (byte* bufferPtr = buffer)
            {
                compress = ICCompress(_hic,
                                      keyFrame ? IcCompressFlags.KeyFrame : IcCompressFlags.None,
                                      _outputFormat.RawData,
                                      _outputBuffer,
                                      _inputFormat.RawData,
                                      bufferPtr + offset,
                                      out uint ckid,
                                      out dwFlags,
                                      FrameIndex,
                                      0,
                                      0,
                                      null,
                                      null);
            }

            if (compress != 0)
                throw new VideoCompressorException((VideoCompressorError)compress);

            FrameIndex++;
            keyFrame = (dwFlags & AviifKeyframe) != 0;

            return (int)_outputFormat.ImageSize;
        }

        public void End()
        {
            if (!IsCompressing)
                return;

            var result = ICSendMessage(_hic, IcMessage.CompressEnd, IntPtr.Zero, IntPtr.Zero);
            
            IsCompressing = false;
            FrameIndex = -1;

            Debug.Assert(result == IntPtr.Zero);
        }
    }
}
