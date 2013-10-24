using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace PavelNazarov.Common.Collections
{
    public static class DictionaryExtensions
    {
        public static NameValueCollection ToNameValueCollection<T, V>(this Dictionary<T, V> dict)
        {
            var collection = new NameValueCollection(dict.Count);
            foreach (KeyValuePair<T, V> pair in dict)
            {
                collection.Add(pair.Key.ToString(), pair.Value.ToString());
            }
            return collection;
        }
    }
}
