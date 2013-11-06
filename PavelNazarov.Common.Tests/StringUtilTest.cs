using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PavelNazarov.Common.Tests
{
    [TestClass]
    public class StringUtilTest
    {
        [TestMethod]
        public void Test_StringUtil_ToStringSafe_ShouldReturnStringEmptyWhenNull()
        {
            Assert.AreEqual(String.Empty, StringUtil.ToStringSafe(null));
        }

        [TestMethod]
        public void Test_StringUtil_ToStringSafe_ShouldReturnString()
        {
            DateTime nowDate = DateTime.Now;
            Assert.AreEqual(nowDate.ToString(), StringUtil.ToStringSafe(nowDate));
        }

        [TestMethod]
        public void Test_StringUtil_MaskString()
        {
            string cardNumber = "1234567890123456";
            string maskedCardNumber = "************3456";
            Assert.AreEqual(maskedCardNumber, StringUtil.MaskString(cardNumber, 0, 12));
        }
    }
}
