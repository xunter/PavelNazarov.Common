using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace PavelNazarov.Common.Xml
{
    public static class XmlUtil
    {
        /// <summary>
        /// Gets a root element content for the specified xml data
        /// </summary>
        /// <param name="xml">xml data</param>
        /// <returns>a root element content</returns>
        public static string GetRootElementContent(string xml)
        {
            var xmlDoc = XDocument.Parse(xml);
            var rootElement = xmlDoc.Root;
            return rootElement.ToString(SaveOptions.DisableFormatting);
        }

        public static string WrapXmlFragment(string xmlFragment, string rootElementName, string prefix = null, string ns = null)
        {
            var xns = XNamespace.Get(ns);            
            var rootElementXName = XName.Get(String.Concat(prefix, ":", rootElementName), ns);
            
            var xDoc = new XDocument(
                new XDeclaration("1.0", null, null),
                new XElement(rootElementXName)
            );

            using (var xmlWriter = xDoc.Root.CreateWriter())
            {
                xmlWriter.WriteRaw(xmlFragment);
            }

            return xDoc.ToString(SaveOptions.DisableFormatting);
        }    
    }
}
