using System;
using System.Globalization;
using System.Text;

namespace AviRecorder.Core
{
    internal static class StringConverter
    {
        public static Encoding Utf8 { get; } = new UTF8Encoding(false);

        public static string ToString(sbyte value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(byte value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(short value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(ushort value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(int value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(uint value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(long value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(ulong value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(bool value)
        {
            return value.ToString();
        }

        public static string ToString(float value)
        {
            return value.ToString("R", NumberFormatInfo.InvariantInfo);
        }

        // "G17" instead of "R" because https://msdn.microsoft.com/en-us/library/kfsatb94(v=vs.110).aspx#Anchor_2
        public static string ToString(double value)
        {
            return value.ToString("G17", NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(decimal value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return InternalByteArrayToString(buffer, 0, buffer.Length);
        }

        public static string ToString(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalByteArrayToString(buffer, offset, count);
        }

        public static sbyte ToSByte(string s)
        {
            return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static byte ToByte(string s)
        {
            return byte.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static short ToInt16(string s)
        {
            return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static ushort ToUInt16(string s)
        {
            return ushort.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static int ToInt32(string s)
        {
            return int.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static uint ToUInt32(string s)
        {
            return uint.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static long ToInt64(string s)
        {
            return long.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static ulong ToUInt64(string s)
        {
            return ulong.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
        }

        public static bool ToBoolean(string s)
        {
            return bool.Parse(s);
        }

        public static float ToSingle(string s)
        {
            return float.Parse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
        }

        public static double ToDouble(string s)
        {
            return double.Parse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
        }

        public static decimal ToDecimal(string s)
        {
            return decimal.Parse(s, NumberStyles.Number, NumberFormatInfo.InvariantInfo);
        }

        public static byte[] ToByteArray(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            var resultLength = (int)((uint)s.Length + 1 >> 1);

            if (resultLength == 0)
                return Array.Empty<byte>();

            var result = new byte[resultLength];
            var bytesWritten = InternalTryParseHexStringToByteArray(s, 0, s.Length, result, 0, resultLength);

            if (bytesWritten < 0)
                throw new FormatException("Unable to parse hex string: '" + s[~bytesWritten] + "' is not a valid hex digit.");

            return result;
        }

        public static sbyte? TryToSByte(string s)
        {
            if (sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out sbyte result))
                return result;

            return null;
        }

        public static byte? TryToByte(string s)
        {
            if (byte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out byte result))
                return result;

            return null;
        }

        public static short? TryToInt16(string s)
        {
            if (short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out short result))
                return result;

            return null;
        }

        public static ushort? TryToUInt16(string s)
        {
            if (ushort.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out ushort result))
                return result;

            return null;
        }

        public static int? TryToInt32(string s)
        {
            if (int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out int result))
                return result;

            return null;
        }

        public static uint? TryToUInt32(string s)
        {
            if (uint.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out uint result))
                return result;

            return null;
        }

        public static long? TryToInt64(string s)
        {
            if (long.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out long result))
                return result;

            return null;
        }

        public static ulong? TryToUInt64(string s)
        {
            if (ulong.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out ulong result))
                return result;

            return null;
        }

        public static bool? TryToBoolean(string s)
        {
            if (bool.TryParse(s, out bool result))
                return result;

            return null;
        }

        public static float? TryToSingle(string s)
        {
            if (float.TryParse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out float result))
                return result;

            return null;
        }

        public static double? TryToDouble(string s)
        {
            if (double.TryParse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double result))
                return result;

            return null;
        }

        public static decimal? TryToDecimal(string s)
        {
            if (decimal.TryParse(s, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out decimal result))
                return result;

            return null;
        }

        public static byte[] TryToByteArray(string s)
        {
            if (s == null)
                return null;

            var resultLength = (int)((uint)s.Length + 1 >> 1);

            if (resultLength == 0)
                return Array.Empty<byte>();

            var result = new byte[resultLength];
            var bytesWritten = InternalTryParseHexStringToByteArray(s, 0, s.Length, result, 0, resultLength);

            return bytesWritten >= 0 ? result : null;
        }

        public static int ParseHexStringToByteArray(string s, byte[] buffer)
        {
            var bytesWritten = TryParseHexStringToByteArray(s, buffer);

            if (bytesWritten < 0)
                throw new FormatException("Unable to parse hex string: '" + s[~bytesWritten] + "' is not a valid hex digit.");

            return bytesWritten;
        }

        public static int TryParseHexStringToByteArray(string s, byte[] buffer)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            
            return InternalTryParseHexStringToByteArray(s, 0, s.Length, buffer, 0, buffer.Length);
        }

        public static int ParseHexStringToByteArray(string s, int startIndex, int length, byte[] buffer, int offset, int count)
        {
            var bytesWritten = TryParseHexStringToByteArray(s, startIndex, length, buffer, offset, count);

            if (bytesWritten < 0)
                throw new FormatException("Unable to parse hex string: '" + s[~bytesWritten] + "' is not a valid hex digit.");

            return bytesWritten;
        }

        public static int TryParseHexStringToByteArray(string s, int startIndex, int length, byte[] buffer, int offset, int count)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)startIndex > (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if ((uint)length > (uint)s.Length - (uint)startIndex)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalTryParseHexStringToByteArray(s, startIndex, length, buffer, offset, count);
        }
        
        private static string InternalByteArrayToString(byte[] buffer, int offset, int count)
        {
            if (count == 0)
                return string.Empty;
            if (count > int.MaxValue / 2)
                throw new ArgumentOutOfRangeException(nameof(count), FormattableString.Invariant($"The specified length exceeds the maximum value of {int.MaxValue / 2}."));

            var result = new char[2 * count];
            var resultIndex = 0;
            var limit = offset + count;

            for (; offset < limit; offset++)
            {
                result[resultIndex++] = CharUtils.HexValueToChar(buffer[offset] >> 4);
                result[resultIndex++] = CharUtils.HexValueToChar(buffer[offset] & 0xF);
            }

            return new string(result);
        }
        
        private static int InternalTryParseHexStringToByteArray(string s, int startIndex, int length, byte[] buffer, int offset, int count)
        {
            var lastChar = startIndex + length;
            var lastByte = offset + count;

            while (lastByte > offset)
            {
                if (lastChar <= startIndex)
                    break;

                var lo = CharUtils.CharToHexValue(s[--lastChar]);

                if (lo < 0)
                    return ~lastChar;

                if (lastChar <= startIndex)
                {
                    buffer[--lastByte] = (byte)lo;
                    break;
                }

                var hi = CharUtils.CharToHexValue(s[--lastChar]);

                if (hi < 0)
                    return ~lastChar;

                buffer[--lastByte] = (byte)((hi << 4) | (lo << 0));
            }

            return offset + count - lastByte;
        }
    }
}
