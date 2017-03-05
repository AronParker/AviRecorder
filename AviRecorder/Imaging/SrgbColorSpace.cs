using System;
using System.Linq;

namespace AviRecorder.Imaging
{
    public static class SRGBColorSpace
    {
        private static double[] _sRgbToLinearLookup = Enumerable.Range(0, 256).Select(x => InternalSrgbToLinear(x / 255.0)).ToArray();

        public static double SrgbToLinear(byte s)
        {
            return _sRgbToLinearLookup[s];
        }

        public static byte LinearToSrgb(double s)
        {
            return (byte)Math.Round(InternalLinearToSrgb(s) * byte.MaxValue);
        }

        private static double InternalSrgbToLinear(double s)
        {
            const double a = 0.055;

            return s <= 0.04045 ? s / 12.92 : Math.Pow((s + a) / (1 + a), 2.4);
        }

        private static double InternalLinearToSrgb(double s)
        {
            const double a = 0.055;

            return s <= 0.0031308 ? 12.92 * s : (1 + a) * Math.Pow(s, 1 / 2.4) - a;
        }
    }
}
