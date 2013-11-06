using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common
{
    /// <summary>
    /// The class contains some usefull miscellaneous functions
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// Invokes the System.Object ToString method if it is not null, otherwise, returns String.Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToStringSafe(Object obj)
        {
            return obj == null ? String.Empty : obj.ToString();
        }

        /// <summary>
        /// Masks a string from startIndex to (startIndex + count) using the specified mask char (default is '*')
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string MaskString(string str, int startIndex, int count, char maskChar = '*')
        {
            var sb = new StringBuilder(str);
            sb.Remove(startIndex, count);
            sb.Insert(startIndex, String.Empty.PadRight(count, maskChar));
            return sb.ToString();
        }
    }
}
