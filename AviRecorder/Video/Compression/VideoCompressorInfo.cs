using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AviRecorder.Video.Avi;
using static AviRecorder.NativeMethods;

namespace AviRecorder.Video.Compression
{
    public class VideoCompressorInfo
    {
        private ICINFO _icInfo;

        internal VideoCompressorInfo(ref ICINFO icInfo)
        {
            _icInfo = icInfo;
        }

        [Flags]
        public enum VideoCompressorFlags : uint
        {
            Quality = 0x0001, // supports quality
            Crunch = 0x0002, // supports crunching to a frame size
            Temporal = 0x0004, // supports inter-frame compress
            CompressFrames = 0x0008, // wants the compress all frames message
            Draw = 0x0010, // supports drawing
            FastTemporalC = 0x0020, // does not need prev frame on compress
            FastTemporalD = 0x0080 // does not need prev frame on decompress
        }

        public uint FccHandler => _icInfo.fccHandler;

        public VideoCompressorFlags Flags => _icInfo.dwFlags;

        public uint Version => _icInfo.dwVersion;

        public string Name => _icInfo.szName;

        public string Description => _icInfo.szDescription;

        public string Driver => _icInfo.szDriver;

        public static VideoCompressorInfo[] GetCompressorInfos()
        {
            var results = new List<VideoCompressorInfo>();

            for (var index = 0U; ICInfo(FourCC.VIDC, index, out var icInfo); index++)
            {
                // logitech i420 codec crashes on 64 bit workaround until its fixed
                if (Environment.Is64BitProcess && icInfo.fccHandler == 0x30323469)
                    continue; 

                using (var hic = ICOpen(FourCC.VIDC, icInfo.fccHandler, IcMode.Compress))
                    if (!hic.IsInvalid && ICGetInfo(hic, ref icInfo, (uint)Marshal.SizeOf<ICINFO>()) != IntPtr.Zero && SupportsFlags(icInfo.dwFlags))
                        results.Add(new VideoCompressorInfo(ref icInfo));
            }

            return results.ToArray();
        }

        private static bool SupportsFlags(VideoCompressorFlags dwFlags)
        {
            const VideoCompressorFlags unsupportedFlags = VideoCompressorFlags.Quality | VideoCompressorFlags.Crunch | VideoCompressorFlags.CompressFrames;

            if ((dwFlags & unsupportedFlags) != 0)
                return false;

            return (dwFlags & VideoCompressorFlags.Temporal) == 0 || (dwFlags & VideoCompressorFlags.FastTemporalC) != 0;
        }
    }
}
