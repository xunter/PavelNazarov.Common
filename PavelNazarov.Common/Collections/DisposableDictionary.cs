using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common.Collections
{
    public class DisposableDictionary<T, V> : IDictionary<T, V>, IDisposable
    {
        private readonly IDictionary<T, V> _original;

        public DisposableDictionary(IDictionary<T, V> original)
        {
            _original = original;
        }

        

        public void Dispose()
        {
            foreach (var kv in this)
            {
                var key = kv.Key;
                var value = kv.Value;
                if (key is IDisposable)
                {
                    ((IDisposable)key).Dispose();
                }

                if (value is IDisposable)
                {
                    ((IDisposable)value).Dispose();
                }
            }
        }

        public void Add(T key, V value)
        {
            _original.Add(key, value);
        }

        public bool ContainsKey(T key)
        {
            return _original.ContainsKey(key);
        }

        public ICollection<T> Keys
        {
            get { return _original.Keys; }
        }

        public bool Remove(T key)
        {
            return _original.Remove(key);
        }

        public bool TryGetValue(T key, out V value)
        {
            return _original.TryGetValue(key, out value);
        }

        public ICollection<V> Values
        {
            get { return _original.Values; }
        }

        public V this[T key]
        {
            get
            {
                return _original[key];
            }
            set
            {
                _original[key] = value;
            }
        }

        public void Add(KeyValuePair<T, V> item)
        {
            _original.Add(item);
        }

        public void Clear()
        {
            _original.Clear();
        }

        public bool Contains(KeyValuePair<T, V> item)
        {
            return _original.Contains(item);
        }

        public void CopyTo(KeyValuePair<T, V>[] array, int arrayIndex)
        {
            _original.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _original.Count; }
        }

        public bool IsReadOnly
        {
            get { return _original.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<T, V> item)
        {
            return _original.Remove(item);
        }

        public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
        {
            return _original.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _original.GetEnumerator();
        }
    }
}
