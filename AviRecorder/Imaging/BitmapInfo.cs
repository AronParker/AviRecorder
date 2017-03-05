using System;
using AviRecorder.Extensions;
using BitConverter = AviRecorder.Core.BitConverter;

namespace AviRecorder.Imaging
{
    public class BitmapInfo
    {
        public const int MinimumSize = 40;

        private byte[] _info;

        public BitmapInfo() : this(MinimumSize)
        {
        }
        
        public BitmapInfo(int size)
        {
            if (size < MinimumSize)
                throw new ArgumentException($"Bitmap info size must be at least {MinimumSize} bytes.", nameof(size));

            _info = new byte[size];
            BitConverter.WriteUInt32LE(_info, 0, (uint)size);
        }

        public int Size
        {
            get => BitConverter.ReadInt32LE(_info, 0);
            set
            {
                if (value < MinimumSize)
                    throw new ArgumentException($"Bitmap info size must be at least {MinimumSize} bytes.", nameof(value));

                Array.Resize(ref _info, value);
                BitConverter.WriteUInt32LE(_info, 0, (uint)value);
            }
        }

        public int Width
        {
            get => BitConverter.ReadInt32LE(_info, 4);
            set => BitConverter.WriteInt32LE(_info, 4, value);
        }

        public int Height
        {
            get => BitConverter.ReadInt32LE(_info, 8);
            set => BitConverter.WriteInt32LE(_info, 8, value);
        }

        public short Planes
        {
            get => BitConverter.ReadInt16LE(_info, 12);
            set => BitConverter.WriteInt16LE(_info, 12, value);
        }

        public short BitCount
        {
            get => BitConverter.ReadInt16LE(_info, 14);
            set => BitConverter.WriteInt16LE(_info, 14, value);
        }

        public uint Compression
        {
            get => BitConverter.ReadUInt32LE(_info, 16);
            set => BitConverter.WriteUInt32LE(_info, 16, value);
        }

        public uint ImageSize
        {
            get => BitConverter.ReadUInt32LE(_info, 20);
            set => BitConverter.WriteUInt32LE(_info, 20, value);
        }

        public int XPelsPerMeter
        {
            get => BitConverter.ReadInt32LE(_info, 24);
            set => BitConverter.WriteInt32LE(_info, 24, value);
        }

        public int YPelsPerMeter
        {
            get => BitConverter.ReadInt32LE(_info, 28);
            set => BitConverter.WriteInt32LE(_info, 28, value);
        }

        public uint UsedColors
        {
            get => BitConverter.ReadUInt32LE(_info, 32);
            set => BitConverter.WriteUInt32LE(_info, 32, value);
        }

        public uint ImportantColors
        {
            get => BitConverter.ReadUInt32LE(_info, 36);
            set => BitConverter.WriteUInt32LE(_info, 36, value);
        }

        public byte[] RawData => _info;

        public static BitmapInfo CreateRgb24(int width, int height)
        {
            if (width < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0 || height > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(height));

            var imageSize = checked((uint)height * (uint)unchecked((width * 3 + 3) & ~3));

            return new BitmapInfo()
            {
                Width = width,
                Height = height,
                Planes = 1,
                BitCount = 24,
                ImageSize = imageSize
            };
        }

        public void SetRgb24(int width, int height)
        {
            if (width < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0 || height > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(height));

            var imageSize = checked((uint)height * (uint)unchecked((width * 3 + 3) & ~3));

            Size = MinimumSize;
            Width = width;
            Height = height;
            Planes = 1;
            BitCount = 24;
            Compression = 0;
            ImageSize = imageSize;
            XPelsPerMeter = 0;
            YPelsPerMeter = 0;
            UsedColors = 0;
            ImportantColors = 0;
        }
    }
}
