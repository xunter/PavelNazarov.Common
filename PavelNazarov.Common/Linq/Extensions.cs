using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common.Linq
{    
    /// <summary>
    /// The class contains common extensions for linq through an IEnumerable collection
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Finds an element by its index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="index">zero based index</param>
        /// <returns>an element or null if nothing was found</returns>
        public static T FindByIndex<T>(this IEnumerable<T> collection, int index)
        {
            return collection.Skip(index).FirstOrDefault();
        }
    }
}
