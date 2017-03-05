using System;
using System.Collections.Generic;
using System.Text;
using AviRecorder.Core;

namespace AviRecorder.KeyValues
{
    public class KeyValueParser
    {
        private List<KeyValue> _stack;
        private StringBuilder _sb;

        private KeyValue _lastKv;
        private string _key;

        private string _s;
        private int _startIndex;
        private int _limit;

        public KeyValueParser()
        {
            _stack = new List<KeyValue>();
            _sb = new StringBuilder();
        }

        public KeyValue Result { get; private set; }

        public int Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            return InternalParse(s, 0, s.Length);
        }

        public int Parse(string s, int startIndex, int length)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)startIndex > (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if ((uint)length > (uint)s.Length - (uint)startIndex)
                throw new ArgumentOutOfRangeException(nameof(length));

            return InternalParse(s, startIndex, length);
        }

        internal int InternalParse(string s, int startIndex, int length)
        {
            _stack.Clear();
            _sb.Clear();
            _lastKv = null;
            _key = null;
            _s = s;
            _startIndex = startIndex;
            _limit = startIndex + length;

            Result = null;

            var i = startIndex;

            while (i < _limit)
            {
                switch (_s[i])
                {
                    case '\t':
                    case ' ':
                        i++;
                        break;
                    case '\n':
                    case '\r':
                        if (_key != null)
                            AddKeyValue(null);

                        i++;

                        while (i < _limit && (_s[i] == '\n' || _s[i] == '\r'))
                            i++;

                        break;
                    case '"':
                        if (Result != null && _stack.Count == 0)
                            return i - startIndex;

                        i = ReadQuotedToken(i + 1);
                        AddToken();
                        break;
                    case '/':
                        if (i + 1 >= _limit)
                            goto default;

                        if (_s[i + 1] == '/')
                        {
                            i = ReadSingleLineComment(i + 2);
                            break;
                        }

                        if (_s[i + 1] == '*')
                        {
                            i = ReadMultiLineComment(i + 2);
                            break;
                        }

                        goto default;
                    case '{':
                        if (!BeginChildren())
                            throw new ParseException("No parent KeyValue specified to start add children to.", i, 1);

                        i++;
                        break;
                    case '}':
                        if (!EndChildren())
                            throw new ParseException("No parent KeyValue specified to stop adding children to.", i, 1);

                        i++;
                        break;
                    default:
                        if (Result != null && _stack.Count == 0)
                            return i - startIndex;

                        i = ReadNonQuotedToken(i);
                        AddToken();
                        break;
                }
            }

            if (_key != null)
                AddKeyValue(null);

            return i - startIndex;
        }

        private int ReadSingleLineComment(int i)
        {
            while (i < _limit && _s[i] != '\n' && _s[i] != '\r')
                i++;

            return i;
        }

        private int ReadMultiLineComment(int i)
        {
            while (i < _limit)
            {
                if (_s[i++] != '*')
                    continue;

                if (i >= _limit)
                    break;

                if (_s[i] == '/')
                    return i + 1;
            }

            return i;
        }

        private int ReadNonQuotedToken(int i)
        {
            while (i < _limit)
            {
                switch (_s[i])
                {
                    case '\t':
                    case '\n':
                    case '\r':
                    case ' ':
                    case '"':
                    case '{':
                    case '}':
                        return i;
                    case '/':
                        if (i + 1 < _limit && (_s[i + 1] == '/' || _s[i + 1] == '*'))
                            return i;

                        goto default;
                    case '\\':
                        i = StringUtils.UnescapeEscapeSequence(_s, i + 1, _limit, _sb);
                        break;
                    default:
                        _sb.Append(_s[i++]);
                        break;
                }
            }

            return i;
        }

        private int ReadQuotedToken(int i)
        {
            while (i < _limit)
            {
                switch (_s[i])
                {
                    case '\n':
                    case '\r':
                        i++;
                        break;
                    case '"':
                        return i + 1;
                    case '\\':
                        i = StringUtils.UnescapeEscapeSequence(_s, i + 1, _limit, _sb);
                        break;
                    default:
                        _sb.Append(_s[i++]);
                        break;
                }
            }

            return i;
        }

        private void AddToken()
        {
            var token = _sb.ToString();
            _sb.Clear();

            if (_key == null)
                _key = token;
            else
                AddKeyValue(token);
        }

        private void AddKeyValue(string value)
        {
            _lastKv = new KeyValue(_key, value);

            if (Result == null)
                Result = _lastKv;
            else
                _stack[_stack.Count - 1].AddLast(_lastKv);

            _key = null;
        }

        private bool BeginChildren()
        {
            if (_key != null)
                AddKeyValue(null);
            else if (_lastKv == null)
                return false;

            _stack.Add(_lastKv);
            _lastKv = null;
            return true;
        }

        private bool EndChildren()
        {
            if (_stack.Count == 0)
                return false;

            if (_key != null)
                AddKeyValue(null);

            _stack.RemoveAt(_stack.Count - 1);
            _lastKv = null;
            return true;
        }
    }
}
