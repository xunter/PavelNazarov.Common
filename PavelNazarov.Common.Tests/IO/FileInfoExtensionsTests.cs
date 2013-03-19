using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PavelNazarov.Common.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace PavelNazarov.Common.Tests.IO
{
    [TestClass]
    public class FileInfoExtensionsTests
    {
        [TestMethod]
        public void Test_FileInfoExtensions_Touch()
        {
            string testFilePath = Path.GetTempFileName();
            var fi = new FileInfo(testFilePath);
            fi.Delete();
            Assert.IsFalse(fi.Exists);
            fi.Touch();
            fi.Refresh();
            Assert.IsTrue(fi.Exists);
            var lmd = fi.LastWriteTime;
            fi.Touch();
            Assert.AreNotEqual(lmd, fi.LastWriteTime);
        }
    }
}
