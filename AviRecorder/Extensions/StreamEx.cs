using System.IO;

namespace AviRecorder.Extensions
{
    public static class StreamEx
    {
        public static void SafeRead(this Stream stream, byte[] buffer, int offset, int count)
        {
            while (count > 0)
            {
                var size = stream.Read(buffer, offset, count);

                if (size == 0)
                    throw new EndOfStreamException();

                offset += size;
                count -= size;
            }
        }
    }
}
