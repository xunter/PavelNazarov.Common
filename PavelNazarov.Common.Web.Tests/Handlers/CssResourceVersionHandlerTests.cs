using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PavelNazarov.Common.Web.Handlers;

namespace PavelNazarov.Common.Web.Tests
{
    [TestClass]
    public class CssResourceVersionHandlerTests
    {
        [TestMethod]
        public void TestParseResourceUrlThroughRegularExpression()
        {
            string cssLine = "background-image: url(../img/bg.png)";
            string cssLineWithQuotes = "background-image: url(\"../img/bg.png\")";

            var match = CssResourceVersionHandler.ResourceUrlRegularExpression.Match(cssLine);
            Assert.AreEqual("../img/bg.png", match.Groups[2].Value);

            var matchWithQuotes = CssResourceVersionHandler.ResourceUrlRegularExpression.Match(cssLineWithQuotes);
            Assert.AreEqual("../img/bg.png", match.Groups[2].Value);
        }
    }
}
