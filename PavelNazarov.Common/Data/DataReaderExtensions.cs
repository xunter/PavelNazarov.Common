using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace PavelNazarov.Common.Data
{
    /// <summary>
    /// Extension methods for the classes implement the System.Data.IDataReader interface
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Get value for the field with the specified ordinal or returns default value of the specified type if it is DBNull
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <exception cref="System.InvalidCastException"></exception>
        /// <returns></returns>
        public static T GetValueOrReturnDefaultIfDBNull<T>(this IDataReader reader, int ordinal)
        {            
            if (!reader.IsDBNull(ordinal))
            {
                var val = (T)reader.GetValue(ordinal);
                return val;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Get value for the field with the specified name or return a default value of the specified type if is is DBNull
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <exception cref="System.InvalidCastException"></exception>
        /// <returns></returns>
        public static T GetValueOrReturnDefaultIfDBNull<T>(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);
            var val = reader.GetValueOrReturnDefaultIfDBNull<T>(ordinal);
            return val;
        }
    }
}
