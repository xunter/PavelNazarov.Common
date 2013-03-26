using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PavelNazarov.Common.Xml
{
    /// <summary>
    /// Decorates an instance of the XmlReader class and parses text node without any whitespaces. It uses the System.String.Trim method.
    /// </summary>
    public class TrimTextXmlReaderDecorator : XmlReader
    {
        private readonly XmlReader _original;

        #region XmlReader implementations

        public TrimTextXmlReaderDecorator(XmlReader original)
        {
            _original = original;
        }

        public override int AttributeCount
        {
            get { return _original.AttributeCount; }
        }

        public override string BaseURI
        {
            get { return _original.BaseURI; }
        }

        public override void Close()
        {
            _original.Close();
        }

        public override int Depth
        {
            get { return _original.Depth; }
        }

        public override bool EOF
        {
            get { return _original.EOF; }
        }

        public override string GetAttribute(int i)
        {
            return _original.GetAttribute(i);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return _original.GetAttribute(name, namespaceURI);
        }

        public override string GetAttribute(string name)
        {
            return _original.GetAttribute(name);
        }

        public override bool IsEmptyElement
        {
            get { return _original.IsEmptyElement; }
        }

        public override string LocalName
        {
            get { return _original.LocalName; }
        }

        public override string LookupNamespace(string prefix)
        {
            return _original.LookupNamespace(prefix);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return _original.MoveToAttribute(name, ns);
        }

        public override bool MoveToAttribute(string name)
        {
            return _original.MoveToAttribute(name);
        }

        public override bool MoveToElement()
        {
            return _original.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return _original.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return _original.MoveToNextAttribute();
        }

        public override XmlNameTable NameTable
        {
            get { return _original.NameTable; }
        }

        public override string NamespaceURI
        {
            get { return _original.NamespaceURI; }
        }

        public override XmlNodeType NodeType
        {
            get { return _original.NodeType; }
        }

        public override string Prefix
        {
            get { return _original.Prefix; }
        }

        public override bool Read()
        {
            return _original.Read();
        }

        public override bool ReadAttributeValue()
        {
            return _original.ReadAttributeValue();
        }

        public override ReadState ReadState
        {
            get { return _original.ReadState; }
        }

        public override void ResolveEntity()
        {
            _original.ResolveEntity();
        }

        public override string Value
        {
            get 
            {
                if (_original.NodeType == XmlNodeType.Text)
                {
                    string originalValue = _original.Value;
                    if (originalValue != null)
                    {
                        var trimmedValue = originalValue.Trim();
                        return trimmedValue; 
                    }
                }
                return _original.Value; 
            }
        }

        #endregion XmlReader implementations
    }
}
