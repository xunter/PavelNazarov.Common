using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common
{
    /// <summary>
    /// Provides some useful functions for enum types.
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// Parses a enum member from the string ignoring case.
        /// </summary>
        /// <typeparam name="T">a enum type</typeparam>
        /// <param name="val">string value</param>
        /// <returns>a enum member</returns>
        public static T Parse<T>(string val)
        {
            if (String.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("The type parameter T must be a Enum type.");
            }

            foreach (string enumMemberName in Enum.GetNames(enumType))
            {
                if (String.Compare(enumMemberName, val, true) == 0)
                {
                    return (T)Enum.Parse(enumType, enumMemberName);
                }
            }

            return default(T);
        }
    }
}
