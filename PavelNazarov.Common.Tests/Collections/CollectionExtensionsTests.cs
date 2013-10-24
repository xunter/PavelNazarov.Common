using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PavelNazarov.Common.Collections;

namespace PavelNazarov.Common.Tests.Collections
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        class Foo
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public static Foo Factory
            {
                get
                {
                    return new Foo { Name = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString() };
                }
            }
        }

        [TestMethod]
        public void Test_CollectionExtensions_SortByPropertyName()
        {
            var foos = Enumerable.Range(0, 100).Select(i => Foo.Factory).ToArray();

            var foosSortedByName = foos.SortByPropertyName("Name").ToArray();
            var foosOrderedByName = foos.OrderBy(f => f.Name).ToArray();

            Assert.AreEqual(foosOrderedByName[50], foosSortedByName[50]);
            Assert.AreNotEqual(foosOrderedByName[50], foosSortedByName[51]);
        }

        [TestMethod]
        public void Test_CollectionExtensions_DoPaging()
        {
            var foos = Enumerable.Range(0, 100).Select(i => Foo.Factory).ToArray();
            var pagedFoos = foos.DoPaging(5, 10);
            var manualPagedFoos = foos.Skip(40).Take(10);
            Assert.IsTrue(pagedFoos.Union(manualPagedFoos).Count() == pagedFoos.Count());
        }
    }
}
