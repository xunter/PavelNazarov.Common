using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PavelNazarov.Common.Collections
{
    public static class CollectionExtensions
    {
        public static IOrderedEnumerable<T> SortByPropertyName<T>(this IEnumerable<T> collection, string propertyName, string dir = "ASC", string defaultSort = null)
        {
            Type productType = typeof(T);
            var propInfo = productType.GetProperty(propertyName);
            if (propInfo == null && defaultSort != null)
            {
                propInfo = productType.GetProperty(defaultSort);
            }
            var productParamExpr = Expression.Parameter(productType, "p");
            var sortingFunc = Expression.Lambda<Func<T, Object>>(Expression.TypeAs(Expression.MakeMemberAccess(productParamExpr, propInfo), typeof(Object)), productParamExpr).Compile();
            return dir == "ASC" ? collection.OrderBy(sortingFunc) : collection.OrderByDescending(sortingFunc);
        }

        public static IEnumerable<T> DoPaging<T>(this IEnumerable<T> collection, int page, int rowsForPage)
        {
            return collection.Skip(rowsForPage * (page - 1)).Take(rowsForPage);
        }
    }
}