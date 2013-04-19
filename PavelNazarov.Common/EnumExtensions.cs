using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PavelNazarov.Common
{
    /// <summary>
    /// It is an extension class for enumerations
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns a set of enum members that are in the specified composite enum member
        /// </summary>
        /// <typeparam name="T">a enum type</typeparam>
        /// <param name="enumMemberComposite">composite enum member</param>
        /// <returns>a set of enum members</returns>
        public static ISet<T> CompositeAsSet<T>(this Enum enumMemberComposite)
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("The specified type must be a enum!");
            }

            var enumMemberSet = new HashSet<T>();

            var enumNames = Enum.GetNames(enumType);
            foreach (string enumName in enumNames)
            {
                var enumMember = (T)Enum.Parse(enumType, enumName);
                if (enumMemberComposite.HasFlag(enumMember as Enum))
                    enumMemberSet.Add((T)enumMember);
            }
            
            return enumMemberSet;
        }
    }
}
