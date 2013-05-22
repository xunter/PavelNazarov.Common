using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PavelNazarov.Common.Collections;

namespace PavelNazarov.Common.Tests.Collections
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        [TestMethod]
        public void Test_DictionaryExtensions_ToNameValueCollection()
        {
            var dict = new Dictionary<string, string> { { "name", "value" } };
            var coll = dict.ToNameValueCollection();
            Assert.AreEqual(dict.Count, coll.Count);
            foreach (KeyValuePair<string, string> pair in dict)
            {
                bool keyExists = false;
                for (int i = 0; i < coll.Keys.Count; i++)
                {
                    if (coll.Keys[i] == pair.Key)
                    {
                        keyExists = true;
                        break;
                    }
                }
                Assert.IsTrue(keyExists);
                Assert.AreEqual(pair.Value, coll[pair.Key]);
            }
        }
    }
}
