using System;

namespace AviRecorder.Video.Avi
{
    public static class FourCC
    {
        // chunk types
        public const uint RIFF = 'R' << 0 | 'I' << 8 | 'F' << 16 | 'F' << 24;
        public const uint LIST = 'L' << 0 | 'I' << 8 | 'S' << 16 | 'T' << 24;

        // avi riff file
        public const uint AVI = 'A' << 0 | 'V' << 8 | 'I' << 16 | ' ' << 24;
        public const uint hdrl = 'h' << 0 | 'd' << 8 | 'r' << 16 | 'l' << 24;
        public const uint avih = 'a' << 0 | 'v' << 8 | 'i' << 16 | 'h' << 24;
        public const uint strl = 's' << 0 | 't' << 8 | 'r' << 16 | 'l' << 24;
        public const uint strh = 's' << 0 | 't' << 8 | 'r' << 16 | 'h' << 24;
        public const uint strf = 's' << 0 | 't' << 8 | 'r' << 16 | 'f' << 24;
        public const uint odml = 'o' << 0 | 'd' << 8 | 'm' << 16 | 'l' << 24;
        public const uint dmlh = 'd' << 0 | 'm' << 8 | 'l' << 16 | 'h' << 24;
        public const uint movi = 'm' << 0 | 'o' << 8 | 'v' << 16 | 'i' << 24;
        public const uint idx1 = 'i' << 0 | 'd' << 8 | 'x' << 16 | '1' << 24;
        public const uint AVIX = 'A' << 0 | 'V' << 8 | 'I' << 16 | 'X' << 24;
        public const uint DIB = 'D' << 0 | 'I' << 8 | 'B' << 16 | ' ' << 24;

        // avi stream types
        public const uint vids = 'v' << 0 | 'i' << 8 | 'd' << 16 | 's' << 24;
        public const uint auds = 'a' << 0 | 'u' << 8 | 'd' << 16 | 's' << 24;

        // avi chunk ids
        public const uint db00 = '0' << 0 | '0' << 8 | 'd' << 16 | 'b' << 24;
        public const uint dc00 = '0' << 0 | '0' << 8 | 'd' << 16 | 'c' << 24;
        public const uint wb01 = '0' << 0 | '1' << 8 | 'w' << 16 | 'b' << 24;
        public const uint indx = 'i' << 0 | 'n' << 8 | 'd' << 16 | 'x' << 24;
        public const uint ix00 = '0' << 0 | '0' << 8 | 'i' << 16 | 'x' << 24;
        public const uint ix01 = '0' << 0 | '1' << 8 | 'i' << 16 | 'x' << 24;

        // stream type
        public const uint VIDC = 'V' << 0 | 'I' << 8 | 'D' << 16 | 'C' << 24;

        public static uint Make(string fcc)
        {
            if (fcc == null)
                throw new ArgumentNullException(nameof(fcc));
            if (fcc.Length != sizeof(uint))
                throw new ArgumentException("Invalid Four-Character Code length.", nameof(fcc));

            return (uint)(fcc[0] << 0 | fcc[1] << 8 | fcc[2] << 16 | fcc[3] << 24);
        }

        public static string ToString(uint fcc)
        {
            var result = new char[sizeof(uint)];

            result[0] = (char)(byte)(fcc >> 0);
            result[1] = (char)(byte)(fcc >> 8);
            result[2] = (char)(byte)(fcc >> 16);
            result[3] = (char)(byte)(fcc >> 24);

            return new string(result);
        }

#if DEBUG
        internal static string GenerateFccCode(string fcc)
        {
            return $"public const uint {fcc} = '{fcc[0]}' << 0 | '{fcc[1]}' << 8 | '{fcc[2]}' << 16 | '{fcc[3]}' << 24;";
        }
#endif
    }
}
