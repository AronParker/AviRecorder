namespace AviRecorder.Core
{
    internal static class BitConverter
    {
        public static short ReadInt16LE(byte[] buffer, int offset)
        {
            return (short)ReadUInt16LE(buffer, offset);
        }

        public static short ReadInt16BE(byte[] buffer, int offset)
        {
            return (short)ReadUInt16BE(buffer, offset);
        }

        public static ushort ReadUInt16LE(byte[] buffer, int offset)
        {
            return (ushort)(buffer[offset] | buffer[offset + 1] << 8);
        }

        public static ushort ReadUInt16BE(byte[] buffer, int offset)
        {
            return (ushort)(buffer[offset] << 8 | buffer[offset + 1]);
        }

        public static int ReadInt32LE(byte[] buffer, int offset)
        {
            return (int)ReadUInt32LE(buffer, offset);
        }

        public static int ReadInt32BE(byte[] buffer, int offset)
        {
            return (int)ReadUInt32BE(buffer, offset);
        }

        public static uint ReadUInt32LE(byte[] buffer, int offset)
        {
            return (uint)(buffer[offset] | buffer[offset + 1] << 8 | buffer[offset + 2] << 16 | buffer[offset + 3] << 24);
        }

        public static uint ReadUInt32BE(byte[] buffer, int offset)
        {
            return (uint)(buffer[offset] << 24 | buffer[offset + 1] << 16 | buffer[offset + 2] << 8 | buffer[offset + 3]);
        }

        public static long ReadInt64LE(byte[] buffer, int offset)
        {
            return (long)ReadUInt64LE(buffer, offset);
        }

        public static long ReadInt64BE(byte[] buffer, int offset)
        {
            return (long)ReadUInt64BE(buffer, offset);
        }

        public static ulong ReadUInt64LE(byte[] buffer, int offset)
        {
            return (uint)(buffer[offset] | buffer[offset + 1] << 8 | buffer[offset + 2] << 16 | buffer[offset + 3] << 24) | (ulong)(uint)(buffer[offset + 4] | buffer[offset + 5] << 8 | buffer[offset + 6] << 16 | buffer[offset + 7] << 24) << 32;
        }

        public static ulong ReadUInt64BE(byte[] buffer, int offset)
        {
            return (ulong)(uint)(buffer[offset] << 24 | buffer[offset + 1] << 16 | buffer[offset + 2] << 8 | buffer[offset + 3]) << 32 | (uint)(buffer[offset + 4] << 24 | buffer[offset + 5] << 16 | buffer[offset + 6] << 8 | buffer[offset + 7]);
        }

        public static void WriteInt16LE(byte[] buffer, int offset, short value)
        {
            WriteUInt16LE(buffer, offset, (ushort)value);
        }

        public static void WriteInt16BE(byte[] buffer, int offset, short value)
        {
            WriteUInt16BE(buffer, offset, (ushort)value);
        }

        public static void WriteUInt16LE(byte[] buffer, int offset, ushort value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
        }

        public static void WriteUInt16BE(byte[] buffer, int offset, ushort value)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)value;
        }

        public static void WriteInt32LE(byte[] buffer, int offset, int value)
        {
            WriteUInt32LE(buffer, offset, (uint)value);
        }

        public static void WriteInt32BE(byte[] buffer, int offset, int value)
        {
            WriteUInt32BE(buffer, offset, (uint)value);
        }

        public static void WriteUInt32LE(byte[] buffer, int offset, uint value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
        }

        public static void WriteUInt32BE(byte[] buffer, int offset, uint value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }

        public static void WriteInt64LE(byte[] buffer, int offset, long value)
        {
            WriteUInt64LE(buffer, offset, (ulong)value);
        }

        public static void WriteInt64BE(byte[] buffer, int offset, long value)
        {
            WriteUInt64BE(buffer, offset, (ulong)value);
        }

        public static void WriteUInt64LE(byte[] buffer, int offset, ulong value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
            buffer[offset + 4] = (byte)(value >> 32);
            buffer[offset + 5] = (byte)(value >> 40);
            buffer[offset + 6] = (byte)(value >> 48);
            buffer[offset + 7] = (byte)(value >> 56);
        }

        public static void WriteUInt64BE(byte[] buffer, int offset, ulong value)
        {
            buffer[offset] = (byte)(value >> 56);
            buffer[offset + 1] = (byte)(value >> 42);
            buffer[offset + 2] = (byte)(value >> 40);
            buffer[offset + 3] = (byte)(value >> 32);
            buffer[offset + 4] = (byte)(value >> 24);
            buffer[offset + 5] = (byte)(value >> 16);
            buffer[offset + 6] = (byte)(value >> 8);
            buffer[offset + 7] = (byte)value;
        }
    }
}
