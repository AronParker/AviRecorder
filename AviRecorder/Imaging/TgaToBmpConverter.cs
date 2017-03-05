using System;
using System.Threading.Tasks;

namespace AviRecorder.Imaging
{
    public class TgaToBmpConverter
    {
        private int _height;

        private int _tgaStride;
        private int _tgaSize;
        private byte[] _tgaImage;

        private int _bmpStride;
        private int _bmpSize;
        private byte[] _bmpImage;

        private double[] _state;
        private double _stateDivisor;

        public void ChangeResolution(int width, int height)
        {
            if (width < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0 || width > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(height));

            var bmpStride = (width * 3 + 3) & ~3;
            var bmpSize = checked(height * bmpStride);

            _height = height;

            _tgaStride = width * 3;
            _tgaSize = height * width * 3;

            _bmpStride = bmpStride;
            _bmpSize = bmpSize;

            if (_state != null)
            {
                if (_state.Length == height * width * 3)
                    Array.Clear(_state, 0, _state.Length);
                else
                    _state = null;
            }

            _stateDivisor = 0.0;
        }

        public void Convert(TargaImage tga, BitmapImage bmp)
        {
            if (tga == null)
                throw new ArgumentNullException(nameof(tga));
            if (bmp == null)
                throw new ArgumentNullException(nameof(bmp));
            if (tga.RawData.Length != _tgaSize)
                throw new ArgumentException("Invalid TGA Image dimensions.", nameof(tga));
            if (bmp.RawData.Length != _bmpSize)
                throw new ArgumentException("Invalid BMP Image dimensions.", nameof(bmp));

            _tgaImage = tga.RawData;
            _bmpImage = bmp.RawData;
            Parallel.For(0, _height, FlipLineY);
        }

        public void Add(TargaImage tga)
        {
            if (tga == null)
                throw new ArgumentNullException(nameof(tga));
            if (tga.RawData.Length != _tgaSize)
                throw new ArgumentException("Invalid Image dimensions.", nameof(tga));

            if (_state == null || _state.Length != _tgaSize)
            {
                _state = new double[_tgaSize];
                _stateDivisor = 0.0;
            }

            _tgaImage = tga.RawData;
            Parallel.For(0, _height, AddLineToState);
            _tgaImage = null;

            _stateDivisor++;
        }

        public void Get(BitmapImage bmp)
        {
            if (bmp == null)
                throw new ArgumentNullException(nameof(bmp));
            if (bmp.RawData.Length != _bmpSize)
                throw new ArgumentException("Invalid Image dimensions.", nameof(bmp));

            if (_state == null || _state.Length != _tgaSize)
            {
                _state = new double[_tgaSize];
                _stateDivisor = 0.0;
            }

            _bmpImage = bmp.RawData;
            Parallel.For(0, _height, BlendLineFromState);
            _bmpImage = null;

            Array.Clear(_state, 0, _state.Length);
            _stateDivisor = 0.0;
        }

        public void Clear()
        {
            _state = null;
            _stateDivisor = 0.0;
        }

        private void AddLineToState(int i)
        {
            var index = i * _tgaStride;
            var limit = index + _tgaStride;

            for (; index < limit; index++)
                _state[index] += SRGBColorSpace.SrgbToLinear(_tgaImage[index]);
        }

        private void BlendLineFromState(int i)
        {
            var index = i * _tgaStride;
            var limit = index + _tgaStride;
            var invertedIndex = (_height - i - 1) * _bmpStride;

            while (index < limit)
                _bmpImage[invertedIndex++] = SRGBColorSpace.LinearToSrgb(_state[index++] / _stateDivisor);
        }

        private void FlipLineY(int i)
        {
            Buffer.BlockCopy(_tgaImage, i * _tgaStride, _bmpImage, (_height - i - 1) * _bmpStride, _tgaStride);
        }
    }
}
