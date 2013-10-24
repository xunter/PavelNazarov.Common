using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Data;
using PavelNazarov.Common.Data;

namespace PavelNazarov.Common.Tests.Data
{
    [TestClass]
    public class DataReaderExtensionsTests
    {
        [TestMethod]
        public void Test_DataReaderExtensions_GetValueOrReturnDefaultIfDBNull_ShouldReturnStringValueForNonDBNullColumn()
        {
            var dataReaderSubstitute = Substitute.For<IDataReader>();
            string expected = "01";
            dataReaderSubstitute.GetValue(1).Returns(expected);
            dataReaderSubstitute.IsDBNull(1).Returns(false);
            var actual = dataReaderSubstitute.GetValueOrReturnDefaultIfDBNull<string>(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_DataReaderExtensions_GetValueOrReturnDefaultIfDBNull_ShouldReturnNullStringForDBNullColumn()
        {
            var dataReaderSubstitute = Substitute.For<IDataReader>();
            dataReaderSubstitute.IsDBNull(1).Returns(true);
            var actual = dataReaderSubstitute.GetValueOrReturnDefaultIfDBNull<string>(1);
            Assert.IsNull(actual);            
        }

        [TestMethod]
        public void Test_DataReaderExtensions_GetValueOrReturnDefaultIfDBNull_ShouldReturnStringValueForTheSpecifiedColumnName()
        {
            var dataReaderSubstitute = Substitute.For<IDataReader>();
            string expected = "01";
            dataReaderSubstitute.GetValue(1).Returns(expected);
            dataReaderSubstitute.IsDBNull(1).Returns(false);
            dataReaderSubstitute.GetOrdinal("CODE").Returns(1);
            var actual = dataReaderSubstitute.GetValueOrReturnDefaultIfDBNull<string>("CODE");
            Assert.AreEqual(expected, actual);            
        }
    }
}
