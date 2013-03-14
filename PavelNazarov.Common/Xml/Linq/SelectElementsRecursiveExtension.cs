using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace PavelNazarov.Common.Xml.Linq
{
    /// <summary>
    /// SelectElementsRecursive extension methods. Methods select all matching elements from the source recursive.
    /// </summary>
    public static class SelectElementsRecursiveExtension
    {
        /// <summary>
        /// Selects all elements from the source and all elements for each element too and so on
        /// </summary>
        /// <typeparam name="T">type of the source (The source extends XContainer)</typeparam>
        /// <param name="source">source</param>
        /// <returns>elements</returns>
        public static IEnumerable<XElement> ElementsRecursive<T>(this T source) where T : XContainer
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            var sourceElements = source.Elements();
            var list = new LinkedList<XElement>();
            AddElementsToListRecursive(sourceElements, list, xe => xe.Elements());
            return list;
        }
        
        /// <summary>
        /// Selects all elements from the source and all elements for each element too and so on. Elements must matches the specified name.
        /// </summary>
        /// <typeparam name="T">type of the source (The source extends XContainer)</typeparam>
        /// <param name="source">source</param>
        /// <param name="name">element's name</param>
        /// <returns>elements</returns>
        public static IEnumerable<XElement> ElementsRecursive<T>(this T source, XName name) where T : XContainer
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            var sourceElements = source.Elements(name);
            var list = new LinkedList<XElement>();
            AddElementsToListRecursive(sourceElements, list, xe => xe.Elements(name));
            return list;
        }

        /// <summary>
        /// Selects all elements from the source and all elements for each element too and so on
        /// </summary>
        /// <typeparam name="T">type of the source (The source element extends XContainer)</typeparam>
        /// <param name="source">source</param>
        /// <returns>elements</returns>
        public static IEnumerable<XElement> ElementsRecursive<T>(this IEnumerable<T> source) where T : XContainer
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            var sourceElements = source.Elements();
            var list = new LinkedList<XElement>();
            AddElementsToListRecursive(sourceElements, list, xe => xe.Elements());
            return list;
        }

        /// <summary>
        /// Selects all elements from the source and all elements for each element too and so on. Elements must matches the specified name.
        /// </summary>
        /// <typeparam name="T">type of the source (The source element extends XContainer)</typeparam>
        /// <param name="source">source</param>
        /// <param name="name">element's name</param>
        /// <returns>elements</returns>
        public static IEnumerable<XElement> ElementsRecursive<T>(this IEnumerable<T> source, XName name) where T : XContainer
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            var sourceElements = source.Elements(name);
            var list = new LinkedList<XElement>();
            AddElementsToListRecursive(sourceElements, list, xe => xe.Elements(name));
            return list;
        }

        private static void AddElementsToListRecursive(IEnumerable<XElement> elements, LinkedList<XElement> list, Func<XElement, IEnumerable<XElement>> elementsSelector)
        {
            foreach (var elem in elements)
            {
                list.AddLast(elem);
                var childElements = elementsSelector(elem);
                AddElementsToListRecursive(childElements, list, elementsSelector);
            }
        }
    }
}
