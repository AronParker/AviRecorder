using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using AviRecorder.Core;

namespace AviRecorder.KeyValues
{
    public class KeyValue : IEnumerable<KeyValue>
    {
        private string _key;
        private string _value;

        private KeyValue _parent;
        private KeyValue _next;
        private KeyValue _lastChild;

        public KeyValue(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _key = key;
        }

        public KeyValue(string key, string value)
            : this(key)
        {
            _value = value;
        }

        public KeyValue(string key, sbyte value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, byte value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, short value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, ushort value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, int value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, uint value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, long value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, ulong value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, bool value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, char value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, float value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, double value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, decimal value)
            : this(key, StringConverter.ToString(value))
        {
        }

        public KeyValue(string key, byte[] buffer)
            : this(key, StringConverter.ToString(buffer))
        {
        }

        public KeyValue(string key, params KeyValue[] children)
            : this(key, (IEnumerable<KeyValue>)children)
        {
        }

        public KeyValue(string key, IEnumerable<KeyValue> children)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            _key = key;

            foreach (var child in children)
                AddLast(child);
        }

        public KeyValue(string key, string value, params KeyValue[] children)
            : this(key, value, (IEnumerable<KeyValue>)children)
        {
        }

        public KeyValue(string key, sbyte value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, byte value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, short value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, ushort value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, int value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, uint value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, long value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, ulong value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, bool value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, char value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, float value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, double value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, decimal value, params KeyValue[] children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, byte[] buffer, params KeyValue[] children)
            : this(key, StringConverter.ToString(buffer), children)
        {
        }

        public KeyValue(string key, string value, IEnumerable<KeyValue> children)
            : this(key, children)
        {
            _value = value;
        }

        public KeyValue(string key, sbyte value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, byte value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, short value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, ushort value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, int value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, uint value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, long value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, ulong value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, bool value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, char value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, float value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, double value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, decimal value, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(value), children)
        {
        }

        public KeyValue(string key, byte[] buffer, IEnumerable<KeyValue> children)
            : this(key, StringConverter.ToString(buffer), children)
        {
        }

        public KeyValue(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            _key = kv._key;
            _value = kv._value;

            if (kv._lastChild != null)
            {
                var cur = kv._lastChild;

                do
                {
                    cur = cur._next;
                    InternalAddLast(new KeyValue(cur));
                } while (cur != kv._lastChild);
            }
        }

        public bool HasValue => _value != null;
        public bool HasParent => _parent != null;
        public bool HasChildren => _lastChild != null;

        public string Key
        {
            get => _key;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _key = value;
            }
        }

        public string Value
        {
            get => _value;
            set => _value = value;
        }

        public KeyValue Parent => _parent;

        public KeyValue NextSibling
        {
            get
            {
                if (_parent == null || _parent._lastChild == this)
                    return null;

                return _next;
            }
        }

        public KeyValue PreviousSibling
        {
            get
            {
                if (_parent == null)
                    return null;

                var cur = _parent._lastChild;

                if (cur._next == this)
                    return null;

                while (cur._next != this)
                    cur = cur._next;

                return cur;
            }
        }
        
        public KeyValue FirstChild => _lastChild?._next;
        public KeyValue LastChild => _lastChild;
        public KeyValue this[string key] => Get(key);

        public int Count
        {
            get
            {
                var cur = _lastChild;

                if (cur == null)
                    return 0;

                var count = 0;

                do
                {
                    checked
                    {
                        count++;
                    }

                    cur = cur._next;
                } while (cur != _lastChild);

                return count;
            }
        }

        public long LongCount
        {
            get
            {
                var cur = _lastChild;

                if (cur == null)
                    return 0;

                var count = 0L;

                do
                {
                    checked
                    {
                        count++;
                    }

                    cur = cur._next;
                } while (cur != _lastChild);

                return count;
            }
        }

        public static explicit operator string(KeyValue kv)
        {
            return kv?._value;
        }

        public static explicit operator sbyte(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToSByte(kv._value);
        }

        public static explicit operator byte(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToByte(kv._value);
        }

        public static explicit operator short(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToInt16(kv._value);
        }

        public static explicit operator ushort(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToUInt16(kv._value);
        }

        public static explicit operator int(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToInt32(kv._value);
        }

        public static explicit operator uint(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToUInt32(kv._value);
        }

        public static explicit operator long(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToInt64(kv._value);
        }

        public static explicit operator ulong(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToUInt64(kv._value);
        }

        public static explicit operator bool(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToBoolean(kv._value);
        }

        public static explicit operator float(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToSingle(kv._value);
        }

        public static explicit operator double(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToDouble(kv._value);
        }

        public static explicit operator decimal(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            return StringConverter.ToDecimal(kv._value);
        }

        public static explicit operator byte[](KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToByteArray(kv._value);
        }

        public static explicit operator sbyte? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToSByte(kv._value);
        }

        public static explicit operator byte? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToByte(kv._value);
        }

        public static explicit operator short? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToInt16(kv._value);
        }

        public static explicit operator ushort? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToUInt16(kv._value);
        }

        public static explicit operator int? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToInt32(kv._value);
        }

        public static explicit operator uint? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToUInt32(kv._value);
        }

        public static explicit operator long? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToInt64(kv._value);
        }

        public static explicit operator ulong? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToUInt64(kv._value);
        }

        public static explicit operator bool? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToBoolean(kv._value);
        }

        public static explicit operator float? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToSingle(kv._value);
        }

        public static explicit operator double? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToDouble(kv._value);
        }

        public static explicit operator decimal? (KeyValue kv)
        {
            if (kv == null)
                return null;

            return StringConverter.TryToDecimal(kv._value);
        }

        public static KeyValue Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var text = File.ReadAllText(path, StringConverter.Utf8);

            return InternalParse(text, 0, text.Length);
        }
        
        public static KeyValue Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            return InternalParse(s, 0, s.Length);
        }
        
        public static KeyValue Parse(string s, int startIndex, int length)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)startIndex > (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if ((uint)length > (uint)s.Length - (uint)startIndex)
                throw new ArgumentOutOfRangeException(nameof(length));

            return InternalParse(s, startIndex, length);
        }

        public void AddBefore(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));
            if (kv._parent != null)
                throw new ArgumentException("KeyValue has already been added.", nameof(kv));
            if (_parent == null)
                throw new InvalidOperationException("No parent set.");

            InternalAddBefore(kv);
        }

        public void AddAfter(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));
            if (kv._parent != null)
                throw new ArgumentException("KeyValue has already been added.", nameof(kv));
            if (_parent == null)
                throw new InvalidOperationException("No parent set.");

            InternalAddAfter(kv);
        }

        public void AddFirst(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));
            if (kv._parent != null)
                throw new ArgumentException("KeyValue has already been added.", nameof(kv));

            InternalAddFirst(kv);
        }

        public void AddLast(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));
            if (kv._parent != null)
                throw new ArgumentException("KeyValue has already been added.", nameof(kv));

            InternalAddLast(kv);
        }

        public void AddBefore(params KeyValue[] kvs)
        {
            AddBefore((IEnumerable<KeyValue>)kvs);
        }

        public void AddAfter(params KeyValue[] kvs)
        {
            AddAfter((IEnumerable<KeyValue>)kvs);
        }

        public void AddFirst(params KeyValue[] kvs)
        {
            AddFirst((IEnumerable<KeyValue>)kvs);
        }

        public void AddLast(params KeyValue[] kvs)
        {
            AddLast((IEnumerable<KeyValue>)kvs);
        }

        public void AddBefore(IEnumerable<KeyValue> kvs)
        {
            if (kvs == null)
                throw new ArgumentNullException(nameof(kvs));
            if (_parent == null)
                throw new InvalidOperationException("No parent set.");

            InternalAddBefore(kvs);
        }

        public void AddAfter(IEnumerable<KeyValue> kvs)
        {
            if (kvs == null)
                throw new ArgumentNullException(nameof(kvs));
            if (_parent == null)
                throw new InvalidOperationException("No parent set.");

            InternalAddAfter(kvs);
        }

        public void AddFirst(IEnumerable<KeyValue> kvs)
        {
            if (kvs == null)
                throw new ArgumentNullException(nameof(kvs));

            InternalAddFirst(kvs);
        }

        public void AddLast(IEnumerable<KeyValue> kvs)
        {
            if (kvs == null)
                throw new ArgumentNullException(nameof(kvs));

            InternalAddLast(kvs);
        }

        public KeyValue Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (_lastChild != null)
            {
                var cur = _lastChild;

                do
                {
                    cur = cur._next;

                    if (cur._key == key)
                        return cur;
                } while (cur != _lastChild);
            }

            return null;
        }

        public KeyValue GetOrAdd(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var cur = Get(key);

            if (cur == null)
            {
                cur = new KeyValue(key);
                InternalAddLast(cur);
            }

            return cur;
        }

        public IEnumerable<KeyValue> GetAllBefore()
        {
            return GetAllBefore(null);
        }

        public IEnumerable<KeyValue> GetAllBefore(string key)
        {
            if (_parent == null)
                yield break;

            var cur = _parent._lastChild;
            
            do
            {
                cur = cur._next;

                if (cur == this)
                    break;

                if (key == null || cur._key == key)
                    yield return cur;
            } while (_parent != null && _parent == cur._parent);
        }

        public IEnumerable<KeyValue> GetAllAfter()
        {
            return GetAllAfter(null);
        }

        public IEnumerable<KeyValue> GetAllAfter(string key)
        {
            if (_parent == null)
                yield break;

            var cur = this;

            while (cur._parent != null && cur != cur._parent._lastChild)
            {
                cur = cur._next;

                if (key == null || cur._key == key)
                    yield return cur;
            }
        }

        public IEnumerable<KeyValue> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<KeyValue> GetAll(string key)
        {
            var cur = _lastChild;

            if (cur != null)
            {
                do
                {
                    cur = cur._next;

                    if (key == null || cur._key == key)
                        yield return cur;
                } while (cur._parent == this && cur != _lastChild);
            }
        }

        public IEnumerator<KeyValue> GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        public void ReplaceWith(params KeyValue[] kvs)
        {
            ReplaceWith((IEnumerable<KeyValue>)kvs);
        }

        public void ReplaceAll(params KeyValue[] kvs)
        {
            ReplaceAll((IEnumerable<KeyValue>)kvs);
        }

        public void ReplaceWith(IEnumerable<KeyValue> kvs)
        {
            if (kvs == null)
                throw new ArgumentNullException(nameof(kvs));
            if (_parent == null)
                throw new InvalidOperationException("Unable to replace an element with no parent set.");

            var prev = InternalRemoveAndReturnPrevious();

            if (prev == this)
                _parent.InternalInsertLast(kvs);
            else
                prev.InternalInsertAfter(kvs);
        }

        public void ReplaceAll(IEnumerable<KeyValue> kvs)
        {
            if (kvs == null)
                throw new ArgumentNullException(nameof(kvs));

            RemoveAll();
            InternalInsertLast(kvs);
        }

        public void ReplaceWith(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));
            if (kv._parent != null)
                throw new ArgumentException("KeyValue has already been added.");
            if (_parent == null)
                throw new InvalidOperationException("Unable to replace an element with no parent set.");

            var prev = InternalRemoveAndReturnPrevious();

            if (prev == this)
                _parent.InternalAddLast(kv);
            else
                prev.InternalAddAfter(kv);
        }

        public void ReplaceAll(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));
            if (kv._parent != null)
                throw new ArgumentException("KeyValue has already been added.");

            RemoveAll();
            InternalAddLast(kv);
        }

        public void Remove()
        {
            if (_parent == null)
                throw new InvalidOperationException("Unable to remove an element with no parent set.");

            var prev = _parent._lastChild;

            while (prev._next != this)
                prev = prev._next;

            prev.InternalRemoveNext();
        }

        public KeyValue RemoveBefore()
        {
            return RemoveBefore(null);
        }

        public KeyValue RemoveBefore(string key)
        {
            if (_parent == null)
                return null;

            KeyValue cur;
            var prev = _parent._lastChild;
            var toDelete = (KeyValue)null;

            while ((cur = prev._next) != this)
            {
                if (key == null || cur._key == key)
                    toDelete = prev;

                prev = cur;
            }
            
            return toDelete?.InternalRemoveNext();
        }

        public KeyValue RemoveAfter()
        {
            return RemoveAfter(null);
        }

        public KeyValue RemoveAfter(string key)
        {
            if (_parent == null)
                return null;

            var lastChild = _parent._lastChild;

            if (lastChild == this)
                return null;

            return InternalRemoveAfter(lastChild, key);
        }

        public KeyValue RemoveFirst()
        {
            return RemoveFirst(null);
        }

        public KeyValue RemoveFirst(string key)
        {
            if (_lastChild == null)
                return null;

            return _lastChild.InternalRemoveAfter(_lastChild, key);
        }

        public KeyValue RemoveLast()
        {
            return RemoveLast(null);
        }

        public KeyValue RemoveLast(string key)
        {
            var lastChild = _lastChild;

            if (lastChild == null)
                return null;
            
            var cur = lastChild;
            var toDelete = (KeyValue)null;

            do
            {
                if (key == null || cur._next._key == key)
                    toDelete = cur;

                cur = cur._next;
            } while (cur != lastChild);

            return toDelete?.InternalRemoveNext();
        }

        public int RemoveAllBefore()
        {
            return RemoveAllBefore(null);
        }

        public int RemoveAllBefore(string key)
        {
            if (_parent == null)
                return 0;

            KeyValue cur;
            var prev = _parent._lastChild;
            var removed = 0;

            while ((cur = prev._next) != this)
            {
                if (key == null || cur._key == key)
                {
                    prev.InternalRemoveNext();
                    removed++;
                }
                else
                {
                    prev = cur;
                }
            }

            return removed;
        }

        public int RemoveAllAfter()
        {
            return RemoveAllAfter(null);
        }

        public int RemoveAllAfter(string key)
        {
            if (_parent == null)
                return 0;

            var lastChild = _parent._lastChild;

            if (lastChild == this)
                return 0;

            return InternalRemoveAllAfter(lastChild, key);
        }

        public int RemoveAll()
        {
            return RemoveAll(null);
        }

        public int RemoveAll(string key)
        {
            var lastChild = _lastChild;

            if (lastChild != null)
                return lastChild.InternalRemoveAllAfter(lastChild, key);

            return 0;
        }

        public void Save(string path)
        {
            Save(path, true);
        }

        public void Save(string path, bool useQuotes)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            File.WriteAllText(path, ToString(useQuotes), StringConverter.Utf8);
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool useQuotes)
        {
            return new KeyValueFormatter(useQuotes).Print(this);
        }

        public void SetValue(sbyte value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(short value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(int value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(long value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(byte value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(ushort value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(uint value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(ulong value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(bool value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(float value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(double value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(decimal value)
        {
            _value = StringConverter.ToString(value);
        }

        public void SetValue(byte[] buffer)
        {
            _value = StringConverter.ToString(buffer);
        }

        private void InternalAddBefore(KeyValue kv)
        {
            var prev = _parent._lastChild;

            while (prev._next != this)
                prev = prev._next;

            kv._parent = _parent;
            kv._next = this;
            prev._next = kv;
        }

        private void InternalAddAfter(KeyValue kv)
        {
            kv._parent = _parent;
            kv._next = _next;
            _next = kv;

            if (_parent._lastChild == this)
                _parent._lastChild = kv;
        }

        private void InternalAddFirst(KeyValue kv)
        {
            if (_lastChild == null)
            {
                kv._parent = this;
                kv._next = kv;
                _lastChild = kv;
            }
            else
            {
                _lastChild._next.InternalAddBefore(kv);
            }
        }

        private void InternalAddLast(KeyValue kv)
        {
            if (_lastChild == null)
            {
                kv._parent = this;
                kv._next = kv;
                _lastChild = kv;
            }
            else
            {
                _lastChild.InternalAddAfter(kv);
            }
        }

        private void InternalAddBefore(IEnumerable<KeyValue> kvs)
        {
            var prev = _parent._lastChild;

            while (prev._next != this)
                prev = prev._next;

            prev.InternalInsertAfter(kvs);
        }

        private void InternalAddAfter(IEnumerable<KeyValue> kvs)
        {
            var last = InternalInsertAfter(kvs);

            if (_parent._lastChild == this)
                _parent._lastChild = last;
        }

        private void InternalAddFirst(IEnumerable<KeyValue> kvs)
        {
            if (_lastChild == null)
                InternalInsertLast(kvs);
            else
                _lastChild.InternalInsertAfter(kvs);
        }

        private void InternalAddLast(IEnumerable<KeyValue> kvs)
        {
            if (_lastChild == null)
                InternalInsertLast(kvs);
            else
                _lastChild = _lastChild.InternalInsertAfter(kvs);
        }

        private void InternalInsertLast(IEnumerable<KeyValue> kvs)
        {
            KeyValue prev = null;

            foreach (var kv in kvs)
            {
                if (kv == null)
                    throw new ArgumentException("KeyValue may not be null.", nameof(kvs));
                if (kv._parent != null)
                    throw new ArgumentException("KeyValue has already been added.", nameof(kvs));

                kv._parent = this;

                if (prev == null)
                {
                    kv._next = kv;
                }
                else
                {
                    kv._next = prev._next;
                    prev._next = kv;
                }

                prev = kv;
            }

            _lastChild = prev;
        }

        private KeyValue InternalInsertAfter(IEnumerable<KeyValue> kvs)
        {
            var prev = this;

            foreach (var kv in kvs)
            {
                if (kv == null)
                    throw new ArgumentException("KeyValue may not be null.", nameof(kvs));
                if (kv._parent != null)
                    throw new ArgumentException("KeyValue has already been added.", nameof(kvs));

                kv._parent = _parent;
                kv._next = prev._next;
                prev._next = kv;
                prev = kv;
            }

            return prev;
        }

        private static KeyValue InternalParse(string s, int startIndex, int length)
        {
            var parser = new KeyValueParser();
            parser.InternalParse(s, startIndex, length);
            return parser.Result;
        }

        private KeyValue InternalRemoveAfter(KeyValue toInclusive, string key)
        {
            KeyValue cur;
            var prev = this;

            do
            {
                cur = prev._next;

                if (key == null || cur._key == key)
                    return prev.InternalRemoveNext();

                prev = cur;
            } while (cur != toInclusive);

            return null;
        }

        private int InternalRemoveAllAfter(KeyValue toInclusive, string key)
        {
            KeyValue cur;
            var prev = this;
            var removed = 0;

            do
            {
                cur = prev._next;

                if (key == null || cur._key == key)
                {
                    prev.InternalRemoveNext();
                    removed++;
                }
                else
                {
                    prev = cur;
                }
            } while (cur != toInclusive);

            return removed;
        }

        private KeyValue InternalRemoveAndReturnPrevious()
        {
            var prev = _parent._lastChild;

            while (prev._next != this)
                prev = prev._next;

            prev.InternalRemoveNext();
            return prev;
        }

        private KeyValue InternalRemoveNext()
        {
            var next = _next;

            if (next == this)
            {
                _parent._lastChild = null;
            }
            else
            {
                _next = next._next;

                if (next == _parent._lastChild)
                    _parent._lastChild = this;
            }

            next._parent = null;
            next._next = null;
            return next;
        }
    }
}
