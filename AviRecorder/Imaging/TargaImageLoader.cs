using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using AviRecorder.Extensions;

namespace AviRecorder.Imaging
{
    public class TargaImageLoader
    {
        public const int HeaderSize = 18;

        private byte[] _header;

        public TargaImageLoader()
        {
            _header = new byte[HeaderSize];
        }

        public void Load(Stream stream, TargaImage tga)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (tga == null)
                throw new ArgumentNullException(nameof(tga));

#if DEBUG
            if (stream.GetType() == typeof(FileStream))
            {
                var field = typeof(FileStream).GetField("_bufferSize", BindingFlags.Instance | BindingFlags.NonPublic);
                Debug.Assert(field != null);
                var value = (int)field.GetValue((FileStream)stream);
                Debug.Assert(value <= HeaderSize);
            }
#endif

            stream.SafeRead(_header, 0, _header.Length);
            tga.ChangeResolution(_header[12] << 0 | _header[13] << 8, _header[14] << 0 | _header[15] << 8);
            stream.SafeRead(tga.RawData, 0, tga.RawData.Length);
        }
    }
}
