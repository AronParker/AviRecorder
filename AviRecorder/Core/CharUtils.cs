using System.Globalization;

namespace AviRecorder.Core
{
    internal static class CharUtils
    {
        public static bool RequiresEscaping(char c)
        {
            switch (CharUnicodeInfo.GetUnicodeCategory(c))
            {
                case UnicodeCategory.Control:
                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                    return true;
                default:
                    return false;
            }
        }

        public static int CharToHexValue(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;
            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            return -1;
        }

        public static char HexValueToChar(int i)
        {
            if (i >= 0 && i <= 9)
                return (char)(i + '0');
            if (i >= 10 && i <= 15)
                return (char)(i + 'a' - 10);

            return '\0';
        }
    }
}
