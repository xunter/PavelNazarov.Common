using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;
using PavelNazarov.Common.Security;

namespace PavelNazarov.Common.Tests.Security
{
    [TestClass]
    public class SecureStringExtensionsTests
    {
        [TestMethod]
        public void Test_SecureStringExtensions_GetString()
        {
            using (var secureString = new SecureString())
            {
                string pwd = "123456";
                foreach (char chr in pwd)
                {
                    secureString.AppendChar(chr);
                }
                secureString.MakeReadOnly();

                string readPwd = secureString.GetString();
                Assert.AreEqual(pwd, readPwd);
            }
        }
    }
}
