using System;
using System.Runtime.InteropServices;
using System.Security;
using AviRecorder.Video.Compression;
using static AviRecorder.Video.Compression.VideoCompressorInfo;

namespace AviRecorder
{
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        public const uint AviifKeyframe = 0x10;

        [Flags]
        public enum AviMainHeaderFlags : uint
        {
            HasIndex = 0x00000010,
            MustUseIndex = 0x00000020,
            IsInterleaved = 0x00000100,
            TrustCkType = 0x00000800,
            WasCaptureFile = 0x00010000,
            Copyrighted = 0x00020000
        }

        [Flags]
        public enum IcCompressFlags : uint
        {
            None = 0,
            KeyFrame = 1
        }

        public enum IcMessage : uint
        {
            CompressGetFormat = 16388,
            CompressGetSize = 16389,
            CompressQuery = 16390,
            CompressBegin = 16391,
            Compress = 16392,
            CompressEnd = 16393,
            GetDefaultKeyFrameRate = 16426,
            CompressFramesInfo = 16454,
            GetState = 20480,
            SetState = 20481,
            Configure = 20490,
            About = 20491,
            GetDefaultQuality = 20510,
            GetQuality = 20511,
            SetQuality = 20512
        }

        public enum IcMode : uint
        {
            Compress = 1,
            Decompress = 2,
            FastDecompress = 3,
            Query = 4,
            FastCompress = 5,
            Draw = 8
        }

        [DllImport("Msvfw32.dll")]
        public static extern bool ICInfo(uint fccType,
                                         uint fccHandler,
                                         out ICINFO lpicinfo);

        [DllImport("Msvfw32.dll")]
        public static extern IntPtr ICGetInfo(VideoCompressorHandle hic,
                                              ref ICINFO lpicinfo,
                                              uint cb);

        [DllImport("Msvfw32.dll")]
        public static extern VideoCompressorHandle ICOpen(uint fccType,
                                                         uint fccHandler,
                                                         IcMode mode);

        [DllImport("Msvfw32.dll")]
        public static extern IntPtr ICClose(IntPtr hic);

        [DllImport("Msvfw32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint ICCompress(VideoCompressorHandle hic,
                                                    IcCompressFlags dwFlags,
                                                    byte[] lpbiOutput,
                                                    byte[] lpData,
                                                    byte[] lpbiInput,
                                                    byte* lpBits,
                                                    out uint lpckid,
                                                    out uint lpdwFlags,
                                                    int lFrameNum,
                                                    uint dwFrameSize,
                                                    uint dwQuality,
                                                    byte[] lpbiPrev,
                                                    byte[] lpPrev);

        [DllImport("Msvfw32.dll")]
        public static extern IntPtr ICSendMessage(VideoCompressorHandle hic,
                                                  IcMessage wMsg,
                                                  IntPtr dw1,
                                                  IntPtr dw2);

        [DllImport("Msvfw32.dll")]
        public static extern IntPtr ICSendMessage(VideoCompressorHandle hic,
                                                  IcMessage wMsg,
                                                  byte[] dw1,
                                                  IntPtr dw2);

        [DllImport("Msvfw32.dll")]
        public static extern IntPtr ICSendMessage(VideoCompressorHandle hic,
                                                  IcMessage wMsg,
                                                  byte[] dw1,
                                                  byte[] dw2);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ICINFO
        {
            public uint dwSize;
            public uint fccType;
            public uint fccHandler;
            public VideoCompressorFlags dwFlags;
            public uint dwVersion;
            public uint dwVersionICM;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string szName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szDriver;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct AviOldIndexEntry
        {
            public uint dwChunkId;
            public uint dwFlags;
            public uint dwOffset;
            public uint dwSize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct AviSuperIndexEntry
        {
            public ulong qwOffset; // 64 bit offset to sub index chunk
            public uint dwSize; // 32 bit size of sub index chunk
            public uint dwDuration; // time span of subindex chunk (in stream ticks)
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct AviStdIndexEntry
        {
            public uint dwOffset; // 32 bit offset to data (points to data, not riff header)
            public uint dwSize; // 31 bit size of data (does not include size of riff header), bit 31 is deltaframe bit
        }
    }
}
