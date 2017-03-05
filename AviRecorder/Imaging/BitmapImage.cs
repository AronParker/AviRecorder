using System;

namespace AviRecorder.Imaging
{
    public class BitmapImage
    {
        public BitmapImage()
        {
            InfoHeader = BitmapInfo.CreateRgb24(0, 0);
            RawData = Array.Empty<byte>();
        }

        public BitmapInfo InfoHeader { get; private set; }

        public byte[] RawData { get; private set; }

        public void Clear()
        {
            InfoHeader.SetRgb24(0, 0);
            RawData = Array.Empty<byte>();
        }

        public void ChangeResolution(int width, int height)
        {
            if (width < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(height));

            var length = checked(height * unchecked((width * 3 + 3) & ~3));
            
            if (length != RawData.Length)
                RawData = length == 0 ? Array.Empty<byte>() : new byte[length];

            InfoHeader.SetRgb24(width, height);
        }
    }
}
