using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using PavelNazarov.Common.IO.PersistentStorage;
using PavelNazarov.Common.IO;

namespace PavelNazarov.Common.Tests.IO.PersistentStorage
{
    public class XmlStreamBasedTestStorage : XmlFileBasedPersistentStorage, IDisposable
    {
        public XmlStreamBasedTestStorage()
            : base(new XmlFileBasedPersistentStorageSettings())
        { }

        protected override bool IsStorageExists
        {
            get
            {
                return true;
            }
        }

        protected override System.IO.Stream  GetInputStreamForRead()
        {
            DualStream.Seek(0, SeekOrigin.Begin);
 	         return DualStream;
        }

        protected override Stream GetOutputStreamForWrite()
        {
            DualStream.Seek(0, SeekOrigin.Begin);
            return DualStream;
        }

        public readonly StreamWrapperToManageDisposeOutside DualStream = new StreamWrapperToManageDisposeOutside(new MemoryStream());

        public void Init()
        {
            var xdoc = XDocument.Parse(_testXml);
            DualStream.Seek(0, SeekOrigin.Begin);
            xdoc.Save(DualStream);
            DualStream.Seek(0, SeekOrigin.Begin);
        }

        protected override void CreateEmptyStorage()
        {
            base.CreateEmptyStorage();
            DualStream.Seek(0, SeekOrigin.Begin);
        }

        protected override object FindValue(string key)
        {            
            var foundValue = base.FindValue(key);
            DualStream.Seek(0, SeekOrigin.Begin);
            return foundValue;
        }

        protected override void WriteValue(string key, object objectToWrite)
        {
            base.WriteValue(key, objectToWrite);
            DualStream.Seek(0, SeekOrigin.Begin);
        }

        public void Dispose()
        {
            DualStream.DisposeStreamOutside();
        }

        private readonly string _testXml = "<?xml version=\"1.0\"?><PersistentStorage></PersistentStorage>";
    }

    [TestClass]
    public class PersistentStorageTest
    {
        [TestMethod]
        public void TestPersistentStorageGetSet()
        {
            using (XmlStreamBasedTestStorage storage = new XmlStreamBasedTestStorage())
            {
                storage.Init();

                decimal sum = 123123123.123123123123M;
                
                storage.Set("Cash", sum);
                object givenValue = storage.Get("Cash");
                Assert.IsInstanceOfType(givenValue, typeof(Decimal));
                decimal givenSum = (decimal)givenValue;
                Assert.AreEqual(sum, givenSum);


                var muchText = new String(Enumerable.Repeat('s', 4096 * 128).ToArray());
                storage.Set("MuchText", muchText);
                string givenMuchText = (string)storage.Get("MuchText");
                Assert.AreEqual(muchText, givenMuchText);
                decimal givenSumMore = (decimal)storage.Get("Cash");
                Assert.AreEqual(givenSum, givenSumMore);

                decimal newCash = givenSumMore - 1233;
                storage.Set("Cash", newCash);
                decimal givenNewCash = (decimal)storage.Get("Cash");
                Assert.AreEqual(newCash, givenNewCash);
            }
        }
    }
}
