using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using PavelNazarov.Common.Xml.Linq;
using System.Reflection;
using System.Xml.XPath;

namespace PavelNazarov.Common.Tests.Xml.Linq
{
    [TestClass]
    public class SelectElementsRecursiveExtensionTests
    {
        private XDocument GetXDoc()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PavelNazarov.Common.Tests.Xml.Linq.elements_recursive_2013-03-14.xml"))
            {
                return XDocument.Load(stream);
            }
        }

        [TestMethod]
        public void Test_SelectElementsRecursiveExtension_ElementsRecursive()
        {
            var xdoc = GetXDoc();
            var groupElementsRecursive = xdoc.Root.ElementsRecursive();
            XPathDocument xpathDoc = new XPathDocument(xdoc.CreateReader());
            var xpathNav = xpathDoc.CreateNavigator();
            var xpathExpression = XPathExpression.Compile(@"count(/root//*)");
            int elementsCount = Convert.ToInt32(xpathNav.Evaluate(xpathExpression));
            Assert.AreEqual(elementsCount, groupElementsRecursive.Count());
        }

        [TestMethod]
        public void Test_SelectElementsRecursiveExtension_ElementsRecursive_NameSpecified()
        {
            var xdoc = GetXDoc();
            var groupElementsRecursive = xdoc.Root.Element("main_menu").ElementsRecursive("group");
            Assert.AreEqual(1, groupElementsRecursive.Count());
        }
    }
}
