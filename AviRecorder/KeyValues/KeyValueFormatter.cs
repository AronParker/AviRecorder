using System;
using System.Text;
using AviRecorder.Core;

namespace AviRecorder.KeyValues
{
    public class KeyValueFormatter
    {
        private const int ValueIdentation = 2;

        private StringBuilder _sb;
        private int _indentation;

        public KeyValueFormatter()
            : this(true)
        {
        }

        public KeyValueFormatter(bool useQuotes)
        {
            _sb = new StringBuilder();
            _indentation = 0;

            UseQuotes = useQuotes;
        }

        public bool UseQuotes { get; set; }

        public string Print(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            _sb.Clear();
            _indentation = 0;

            AppendKeyValue(kv);

            return _sb.ToString();
        }

        private static bool MustQuote(string token)
        {
            if (token.Length == 0)
                return true;

            for (var i = 0; i < token.Length; i++)
            {
                switch (token[i])
                {
                    case ' ':
                    case '{':
                    case '}':
                        return true;
                    case '/':
                        if (i + 1 < token.Length && (token[i + 1] == '/' || token[i + 1] == '*'))
                            return true;

                        break;
                }
            }

            return false;
        }

        private void AppendKeyValue(KeyValue kv)
        {
            _sb.Append('\t', _indentation);
            AppendToken(kv.Key);

            if (kv.HasValue)
            {
                _sb.Append('\t', ValueIdentation);
                AppendToken(kv.Value);
            }

            _sb.AppendLine();

            if (kv.HasChildren)
            {
                BeginChildren();

                foreach (var child in kv.GetAll())
                    AppendKeyValue(child);

                EndChildren();
            }
        }

        private void AppendToken(string token)
        {
            if (UseQuotes)
                AppendQuotedToken(token);
            else
                AppendNonQuotedToken(token);
        }

        private void AppendQuotedToken(string token)
        {
            _sb.Append('"');
            StringUtils.EscapeString(token, 0, token.Length, _sb);
            _sb.Append('"');
        }

        private void AppendNonQuotedToken(string token)
        {
            if (MustQuote(token))
                AppendQuotedToken(token);
            else
                StringUtils.EscapeString(token, 0, token.Length, _sb);
        }

        private void BeginChildren()
        {
            _sb.Append('\t', _indentation);
            _sb.Append('{');
            _sb.AppendLine();

            _indentation++;
        }

        private void EndChildren()
        {
            _indentation--;

            _sb.Append('\t', _indentation);
            _sb.Append('}');
            _sb.AppendLine();
        }
    }
}
