using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.XPath;
using System.Xml;

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

        /// <summary>
        /// Loads the target section from an external xml file based on the config.
        /// </summary>
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
                return null;
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
