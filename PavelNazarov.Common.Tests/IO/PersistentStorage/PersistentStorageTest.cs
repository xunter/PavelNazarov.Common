using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using PavelNazarov.Common.IO.PersistentStorage;

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

        public readonly StreamWrapperForManageDisposeOutside DualStream = new StreamWrapperForManageDisposeOutside(new MemoryStream());

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

    public class StreamWrapperForManageDisposeOutside : Stream
    {
        private readonly Stream _original;

        public StreamWrapperForManageDisposeOutside(Stream original)
        {
            _original = original;
        }

        public override bool CanRead
        {
            get { return _original.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _original.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _original.CanWrite; }
        }

        public override void Flush()
        {
            _original.Flush();
        }

        public override long Length
        {
            get { return _original.Length; }
        }

        public override long Position
        {
            get
            {
                return _original.Position;
            }
            set
            {
                _original.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _original.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _original.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _original.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _original.Write(buffer, offset, count);
        }

        public void DisposeStreamOutside()
        {
            this.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
 	         //base.Dispose(disposing);
        }
    }

    [TestClass]
    public class PersistentStorageTest
    {
        [TestMethod]
        public void TestGeneral()
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
