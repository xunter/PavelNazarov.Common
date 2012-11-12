using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PavelNazarov.Common.Xml
{
    public static class XmlDocumentExtensions
    {
        /// <summary>
        /// Gets the XmlDeclaration if it exists, creates a new if not.
        /// Got from http://stackoverflow.com/questions/9687791/net-xmldocument-encoding-issue
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public static XmlDeclaration GetOrCreateXmlDeclaration(this XmlDocument xmlDocument)
        {
            XmlDeclaration xmlDeclaration = null;
            if (xmlDocument.HasChildNodes)
                xmlDeclaration = xmlDocument.FirstChild as XmlDeclaration;

            if (xmlDeclaration != null)
                return xmlDeclaration;
            //Create an XML declaration. 
            xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", null, null);

            //Add the new node to the document.
            XmlElement root = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, root);
            return xmlDeclaration;
        }
    }
}
