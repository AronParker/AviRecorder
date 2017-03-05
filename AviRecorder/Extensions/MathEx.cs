using System;

namespace AviRecorder.Extensions
{
    public static class MathEx
    {
        public static int Constrain(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
