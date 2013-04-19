using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PavelNazarov.Common;

namespace PavelNazarov.Common.Tests
{
    [TestClass]
    public class EnumExtensionsTests
    {
        enum SomeEnumTypes
        {
            None = 0,
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 16
        }

        [TestMethod]
        public void Test_EnumExtensions_CompositeAsSet()
        {
            SomeEnumTypes aEnum = SomeEnumTypes.A | SomeEnumTypes.B | SomeEnumTypes.C;
            var enumMemberSet = aEnum.CompositeAsSet<SomeEnumTypes>();
            Assert.IsTrue(enumMemberSet.Contains(SomeEnumTypes.A));
            Assert.IsTrue(enumMemberSet.Contains(SomeEnumTypes.B));
            Assert.IsTrue(enumMemberSet.Contains(SomeEnumTypes.C));
            Assert.IsFalse(enumMemberSet.Contains(SomeEnumTypes.D));
            Assert.IsFalse(enumMemberSet.Contains(SomeEnumTypes.E));
        }
    }
}
