using System;

namespace AviRecorder.Imaging
{
    public class TargaImage
    {
        public TargaImage()
        {
            Clear();
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public byte[] RawData { get; private set; }

        public void Clear()
        {
            Width = 0;
            Height = 0;
            RawData = Array.Empty<byte>();
        }

        public void ChangeResolution(int width, int height)
        {
            if (width < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(height));

            var length = checked(height * unchecked(width * 3));

            if (length != RawData.Length)
                RawData = length == 0 ? Array.Empty<byte>() : new byte[length];

            Width = width;
            Height = height;
        }
    }
}
