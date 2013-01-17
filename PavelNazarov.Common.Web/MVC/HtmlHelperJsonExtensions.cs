using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PavelNazarov.Common.Web.MVC
{
    public static class HtmlHelperJsonExtensions
    {
        /// <summary>
        /// Serialize an object as json
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="objectToEncode"></param>
        /// <returns></returns>
        public static IHtmlString Json(this HtmlHelper htmlHelper, object objectToEncode)
        {
            return new RawHtmlString(System.Web.Helpers.Json.Encode(objectToEncode));
        }
        
        /// <summary>
        /// Serialize an enum as json map
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IHtmlString EnumAsJsonMap(this HtmlHelper htmlHelper, Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("The specified type must be a enum type!");

            var enumMembersAsMap = System.Enum.GetNames(enumType).ToDictionary(n => n, n => (int)System.Enum.Parse(enumType, n));
            var enumMembersAsJsonString = System.Web.Helpers.Json.Encode(enumMembersAsMap);
            return new RawHtmlString(enumMembersAsJsonString);
        }
    }
}
