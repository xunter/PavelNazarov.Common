using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PavelNazarov.Common.Linq;

namespace PavelNazarov.Common.Tests.Linq
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void Test_Extensions_FindByIndex()
        {
            var collection = Enumerable.Range(0, 100).ToArray();
            const int indexToTest = 10;

            int foundNum = collection.FindByIndex(indexToTest);
            Assert.AreEqual(collection[indexToTest], foundNum);
        }
    }
}
