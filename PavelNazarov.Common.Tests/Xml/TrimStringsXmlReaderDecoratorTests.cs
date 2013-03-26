using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using PavelNazarov.Common.Xml;
using System.Xml.Linq;

namespace PavelNazarov.Common.Tests.Xml
{
    [TestClass]
    public class TrimStringsXmlReaderDecoratorTests
    {
        [TestMethod]
        public void Test_TrimTextXmlReaderDecorator_ReadTrimmedString()
        {
            string xml = @"<item value='ГосП. за выдачу ФЛ справки, подт-й выдачу ВУ или ВР на управ. ТС'>
ГосП. за выдачу ФЛ справки, подт-й выдачу ВУ или ВР на управ. ТС</item>";
            var xrs = new XmlReaderSettings() { IgnoreWhitespace = true };
            using (var reader = new TrimTextXmlReaderDecorator(XmlReader.Create(new StringReader(xml), xrs)))
            {
                string expectedVal = "ГосП. за выдачу ФЛ справки, подт-й выдачу ВУ или ВР на управ. ТС";
                var xitem = XElement.Load(reader);
                string actual = xitem.Value;
                Assert.AreEqual(expectedVal, actual);
            }
        }
    }
}
