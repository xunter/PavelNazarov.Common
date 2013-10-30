using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Threading;
using System.Text;

namespace PavelNazarov.Common.IO.PersistentStorage
{
    public class XmlFileBasedPersistentStorageSettings
    {
        public string FullFileName { get; set; }
        public bool UseBOM { get; set; }

        public XmlFileBasedPersistentStorageSettings()
        {
            UseBOM = false;
        }
    }

    public class XmlFileBasedPersistentStorage : IPersistentStorage
    {
        const string RootElementName = "PersistentStorage";
        const string ItemElementName = "Item";
        const string KeyAttributeName = "Key";

        const int ReaderWriterLockTimeout = 10000;

        private readonly XmlFileBasedPersistentStorageSettings _originalSettings;
        private readonly BinaryFormatter _binarySer = new BinaryFormatter();
        private readonly ReaderWriterLock _rwLock = new ReaderWriterLock();

        public XmlFileBasedPersistentStorage(XmlFileBasedPersistentStorageSettings settings)
        {
            _originalSettings = settings;
        }

        protected virtual bool IsStorageExists
        {
            get 
            {
                try
                {
                    _rwLock.AcquireReaderLock(ReaderWriterLockTimeout);
                    return File.Exists(_originalSettings.FullFileName);
                }
                finally
                {
                    _rwLock.ReleaseReaderLock();
                }
            }
        }
                
        protected virtual void CreateEmptyStorage()
        {
            var xdoc = new XDocument(new XDeclaration("1.0", null, null),
                new XElement(RootElementName));
            try
            {
                _rwLock.AcquireWriterLock(ReaderWriterLockTimeout);
                using (var outputFileStream = GetOutputStreamForWrite())
                {
                    xdoc.Save(outputFileStream);
                }
            }
            finally
            {
                _rwLock.ReleaseWriterLock();
            }
        }

        protected virtual object FindValue(string key)
        {
            if (!IsStorageExists) return null;
            try
            {
                _rwLock.AcquireReaderLock(ReaderWriterLockTimeout);
                using (var inputFileStream = GetInputStreamForRead())
                {
                    using (var xmlReader = XmlReader.Create(inputFileStream))
                    {
                        while (xmlReader.ReadToFollowing(ItemElementName))
                        {
                            var keyFromElement = xmlReader.GetAttribute(KeyAttributeName);
                            if (key == keyFromElement) return ReadValueFromXmlReader(xmlReader);
                        }
                    }
                }
                return null;
            }
            finally
            {
                _rwLock.ReleaseReaderLock();
            }
        }

        protected virtual object ReadValueFromXmlReader(XmlReader xmlReader)
        {
            xmlReader.MoveToContent();
            using (var memoryStream = new MemoryStream())
            {
                const int BUFFER_SIZE = 4096;
                byte[] buffer = new byte[BUFFER_SIZE];
                int readByteCount = 0;

                while ((readByteCount = xmlReader.ReadElementContentAsBase64(buffer, 0, BUFFER_SIZE)) > 0)
                {
                    memoryStream.Write(buffer, 0, readByteCount);
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                var deserializedObject = _binarySer.Deserialize(memoryStream);
                return deserializedObject;
            }
        }

        protected virtual void WriteValue(string key, object objectToWrite)
        {
            if (!IsStorageExists)
            {
                CreateEmptyStorage();
            }

            XDocument xdoc = null;
            try
            {
                _rwLock.AcquireReaderLock(ReaderWriterLockTimeout);
                using (var inputStream = GetInputStreamForRead())
                {
                    xdoc = XDocument.Load(inputStream);
                }
            }
            finally
            {
                _rwLock.ReleaseReaderLock();
            }

            var existing = (from itemElement in xdoc.Root.Elements(ItemElementName)
                            where itemElement.Attribute(KeyAttributeName).Value == key
                               select itemElement).SingleOrDefault();
            if (existing != null)
            {
                existing.SetValue(String.Empty);
                SerializeObjectToElement(existing, objectToWrite);
            }
            else
            {
                var newElement = new XElement(ItemElementName, new XAttribute(KeyAttributeName, key));
                SerializeObjectToElement(newElement, objectToWrite);
                xdoc.Root.Add(newElement);
            }

            try
            {
                _rwLock.AcquireWriterLock(ReaderWriterLockTimeout);
                using (var outputStream = GetOutputStreamForWrite())
                {
                    xdoc.Save(outputStream);
                }
            }
            finally
            {
                _rwLock.ReleaseWriterLock();
            }
        }

        protected virtual Stream GetOutputStreamForWrite()
        {
            string filename = _originalSettings.FullFileName;
            return CreateNewFileAndReturnStream(filename);            
        }

        protected virtual Stream GetInputStreamForRead()
        {
            string filename = _originalSettings.FullFileName;
            if (File.Exists(filename))
            {
                return File.OpenRead(_originalSettings.FullFileName);
            }
            else
            {
                return CreateNewFileAndReturnStream(filename);
            }
        }

        protected virtual FileStream CreateNewFileAndReturnStream(string filename)
        {
            var fs = File.Create(filename);
            if (_originalSettings.UseBOM)
            {
                byte[] bom = System.Text.Encoding.UTF8.GetPreamble();
                fs.Write(bom, 0, bom.Length);
                fs.Flush();
            }
            return fs;
        }

        protected virtual void SerializeObjectToElement(XElement xel, object objectToSerialize)
        {
            using (var memoryStream = new MemoryStream())
            {
                _binarySer.Serialize(memoryStream, objectToSerialize);
                var bytesAsBase64String = Convert.ToBase64String(memoryStream.GetBuffer());
                xel.SetValue(bytesAsBase64String);
            }
        }

        public virtual object Get(string key)
        {
            try
            {
                return FindValue(key);
            }
            catch (Exception ex)
            {
                throw new PersistentStorageException(String.Format("Unable to get value for key equals \"{0}\"!", key), ex);
            }
        }

        public virtual void Set(string key, object objectToStore)
        {
            try
            {
                WriteValue(key, objectToStore);
            }
            catch (Exception ex)
            {
                throw new PersistentStorageException(String.Format("Unable to set value for key equals \"{0}\"!", key), ex);
            }
        }
    }
}