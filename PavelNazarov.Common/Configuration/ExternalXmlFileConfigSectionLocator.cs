using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.XPath;
using System.Xml;
using System.IO;

namespace PavelNazarov.Common.Configuration
{
    [Serializable]
    public class ExternalXmlFileConfigSectionLocator : ConfigurationSection
    {
        private ConfigurationSection _cachedTargetSection;

        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get { return (string)this["Path"]; }
            set { this["Path"] = value; }
        }

        [ConfigurationProperty("XPath", IsRequired = false)]
        public string XPath
        {
            get { return (string)this["XPath"]; }
            set { this["XPath"] = value; }
        }

        [ConfigurationProperty("TargetSectionType", IsRequired = true)]
        public string TargetSectionType
        {
            get { return (string)this["TargetSectionType"]; }
            set { this["TargetSectionType"] = value; }
        }

        protected virtual XmlReader ReadTargetSectionXml()
        {
            var xpathDoc = new XPathDocument(Path);
            var xpathNavigator = xpathDoc.CreateNavigator();
            if (String.IsNullOrEmpty(XPath))
            {
                if (xpathNavigator.NodeType != XPathNodeType.Root) xpathNavigator.MoveToNext(XPathNodeType.Root);
            }
            else
            {
                xpathNavigator = xpathNavigator.SelectSingleNode(XPath);
            }
            return xpathNavigator.ReadSubtree();
        }

        protected virtual XmlDocument LoadExternalXmlAsDocument()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Path);
            return xmlDoc;
        }

        protected virtual XmlElement DeserializeElementForDocument(XmlDocument ownerDocument, Stream source)
        {
            using (var xmlReader = XmlReader.Create(source))
            {
                var element = (XmlElement)ownerDocument.ReadNode(xmlReader);
                var elementForDoc = (XmlElement)ownerDocument.ImportNode(element, true);
                return elementForDoc;
            }
        }

        protected virtual Stream SerializeTargetSectionIntoTemporaryStream(ConfigurationSection targetSection)
        {
            var tempStream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(tempStream, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment }))
            {
                string sectionTagName = (string)targetSection.GetType().GetProperty("ElementTagName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(targetSection, null);
                xmlWriter.WriteStartElement(sectionTagName);
                var serializingResult = (bool)targetSection.GetType().GetMethod("SerializeElement", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(targetSection, new object[] { xmlWriter, false });
                if (!serializingResult) throw new ExternalXmlFileConfigSectionLocatorException("Unable to serialize target section!");
                xmlWriter.WriteEndElement();
            }
            return tempStream;
        }

        protected virtual void ReplaceElement(XmlDocument document, XmlElement elementToReplace, string xpath)
        {
            var originalElement = (XmlElement)document.SelectSingleNode(xpath);
            var parentElement = originalElement.ParentNode;
            parentElement.ReplaceChild(elementToReplace, originalElement);
        }

        protected virtual Stream GetOuputStreamToSaveExternalXml()
        {
            return File.Create(Path);
        }

        public void SaveTargetSection(ConfigurationSection targetSection)
        {
            try
            {
                using (var tempStream = SerializeTargetSectionIntoTemporaryStream(targetSection))
                {                    
                    tempStream.Seek(0, SeekOrigin.Begin);
                    var targetDocument = LoadExternalXmlAsDocument();
                    var targetElement = DeserializeElementForDocument(targetDocument, tempStream);
                    ReplaceElement(targetDocument, targetElement, XPath);
                    using (var streamToSaveDoc = GetOuputStreamToSaveExternalXml())
                    {
                        targetDocument.Save(streamToSaveDoc);
                    }
                }
            }
            catch (ExternalXmlFileConfigSectionLocatorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ExternalXmlFileConfigSectionLocatorException("A fatal error has occurred while saving a target section!", ex);
            }
        }

        /// <summary>
        /// Loads the target section from an external xml file based on the config.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns>a configuration section instance or if anything goes wrong then the method will return null</returns>
        public ConfigurationSection LoadTargetSection()
        {
            try
            {
                var targetConfigSectionType = Type.GetType(TargetSectionType);
                var targetConfigSection = Activator.CreateInstance(targetConfigSectionType);
                var deserializeSectionMethodInfo = targetConfigSectionType.GetMethod("DeserializeSection", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, Type.DefaultBinder, new Type[] { typeof(XmlReader) }, null);

                using (var xmlReader = ReadTargetSectionXml())
                {
                    deserializeSectionMethodInfo.Invoke(targetConfigSection, new object[] { xmlReader });
                }
                return (ConfigurationSection)targetConfigSection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to load the target section!", ex);
            }
        }

        /// <summary>
        /// Gets a target configuration section
        /// </summary>
        /// <returns></returns>
        public ConfigurationSection GetTargetSection()
        {
            if (_cachedTargetSection != null) return _cachedTargetSection;
            var targetSection = LoadTargetSection();
            if (targetSection != null)
            {
                _cachedTargetSection = targetSection;
            }
            return targetSection;
        }
    }
}
