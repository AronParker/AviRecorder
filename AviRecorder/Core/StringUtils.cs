using System.Diagnostics;
using System.Text;

namespace AviRecorder.Core
{
    internal static class StringUtils
    {
        public static void FindLine(string s, int oldIndex, int newIndex, ref int line, ref int column)
        {
            Debug.Assert(oldIndex <= newIndex);

            while (oldIndex < newIndex)
            {
                switch (s[oldIndex++])
                {
                    case '\n':
                        line++;
                        column = 1;
                        break;
                    case '\r':
                        line++;
                        column = 1;

                        if (oldIndex < newIndex && s[oldIndex] == '\n')
                            oldIndex++;

                        break;
                    default:
                        column++;
                        break;
                }
            }
        }

        public static void EscapeString(string s, int startIndex, int limit, StringBuilder sb)
        {
            for (; startIndex < limit; startIndex++)
            {
                var c = s[startIndex];

                switch (c)
                {
                    case '\0':
                        sb.Append("\\0");
                        break;
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    default:
                        if (CharUtils.RequiresEscaping(c))
                        {
                            sb.Append("\\u");
                            sb.Append(CharUtils.HexValueToChar((c >> 12) & 0xF));
                            sb.Append(CharUtils.HexValueToChar((c >> 8) & 0xF));
                            sb.Append(CharUtils.HexValueToChar((c >> 4) & 0xF));
                            sb.Append(CharUtils.HexValueToChar((c >> 0) & 0xF));
                            continue;
                        }

                        sb.Append(c);
                        break;
                }
            }
        }

        public static int UnescapeEscapeSequence(string s, int startIndex, int limit, StringBuilder sb)
        {
            if (startIndex >= limit)
                throw new ParseException("Escape sequence missing.", startIndex, 0);

            switch (s[startIndex++])
            {
                case '"':
                    sb.Append('"');
                    break;
                case '0':
                    sb.Append('\0');
                    break;
                case 'U':
                    startIndex = ParseUtf32EscapeSequence(s, startIndex, limit, sb);
                    break;
                case '\\':
                    sb.Append('\\');
                    break;
                case 'a':
                    sb.Append('\a');
                    break;
                case 'b':
                    sb.Append('\b');
                    break;
                case 'f':
                    sb.Append('\f');
                    break;
                case 'n':
                    sb.Append('\n');
                    break;
                case 'r':
                    sb.Append('\r');
                    break;
                case 't':
                    sb.Append('\t');
                    break;
                case 'u':
                    startIndex = ParseUtf16EscapeSequence(s, startIndex, limit, sb);
                    break;
                case 'v':
                    sb.Append('\v');
                    break;
                case 'x':
                    startIndex = ParseHexEscapeSequence(s, startIndex, limit, sb);
                    break;
                default:
                    throw new ParseException("Unrecognized escape sequence.", startIndex - 1, 1);
            }

            return startIndex;
        }

        private static int ParseUtf16EscapeSequence(string s, int startIndex, int limit, StringBuilder sb)
        {
            if (limit - startIndex < 4)
                throw new ParseException("UTF-16 escape sequence missing.", startIndex, 0);

            var utf16 = CharUtils.CharToHexValue(s[startIndex++]) << 12 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 8 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 4 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 0;

            if (utf16 < 0)
                throw new ParseException("Invalid UTF-16 escape sequence.", startIndex - 4, 4);

            sb.Append((char)utf16);

            return startIndex;
        }

        private static int ParseUtf32EscapeSequence(string s, int startIndex, int limit, StringBuilder sb)
        {
            if (limit - startIndex < 8)
                throw new ParseException("UTF-32 escape sequence missing.", startIndex, 0);

            var utf32 = CharUtils.CharToHexValue(s[startIndex++]) << 28 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 24 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 20 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 16 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 12 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 8 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 4 |
                        CharUtils.CharToHexValue(s[startIndex++]) << 0;
            
            const int highSurrogateStart = 0xd800;
            const int highSurrogateEnd = 0xdbff;
            const int lowSurrogateStart = 0xdc00;
            const int lowSurrogateEnd = 0xdfff;

            if (utf32 < 0 || utf32 > 0x10ffff || (utf32 >= highSurrogateStart && utf32 <= lowSurrogateEnd))
                throw new ParseException("Invalid UTF-32 escape sequence.", startIndex - 8, 8);

            if (utf32 < 0x10000)
            {
                sb.Append((char)utf32);
            }
            else
            {
                var highSurrogate = (char)(((utf32 >> 10) - 0x40) + highSurrogateStart);
                var lowSurrogate = (char)((utf32 & 0x3FF) + lowSurrogateStart);

                Debug.Assert(highSurrogate >= highSurrogateStart && highSurrogate <= highSurrogateEnd);
                Debug.Assert(lowSurrogate >= lowSurrogateStart && lowSurrogate <= lowSurrogateEnd);

                sb.Append(highSurrogate);
                sb.Append(lowSurrogate);
            }

            return startIndex;
        }

        private static int ParseHexEscapeSequence(string s, int startIndex, int limit, StringBuilder sb)
        {
            if (startIndex == limit)
                throw new ParseException("hex escape sequence missing.", startIndex, 0);

            var hexResult = CharUtils.CharToHexValue(s[startIndex++]);

            if (hexResult == -1)
                throw new ParseException("Invalid hex escape sequence.", startIndex - 1, 1);

            for (; startIndex < limit; startIndex++)
            {
                var hexDigit = CharUtils.CharToHexValue(s[startIndex]);

                if (hexDigit == -1)
                    break;

                hexResult = (hexResult << 4) | hexDigit;
            }

            sb.Append((char)hexResult);

            return startIndex;
        }
    }
}
