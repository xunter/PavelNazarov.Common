using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace PavelNazarov.Common.Tests
{
    [TestClass]
    public class EnumUtilTests
    {
        [TestMethod]
        public void Test_EnumUtil_Parse()
        {
            string ok = "Ok";

            var expected = System.Net.HttpStatusCode.OK;
            var actual = EnumUtil.Parse<HttpStatusCode>(ok);

            Assert.AreEqual(expected, actual);
        }
    }
}
