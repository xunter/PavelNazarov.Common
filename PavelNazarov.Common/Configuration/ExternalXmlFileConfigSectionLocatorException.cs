using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common.Configuration
{
    
    [Serializable]
    public class ExternalXmlFileConfigSectionLocatorException : ApplicationException
    {
        public ExternalXmlFileConfigSectionLocatorException()
            : base()
        { }

        public ExternalXmlFileConfigSectionLocatorException(string message)
            : base(message)
        { }

        public ExternalXmlFileConfigSectionLocatorException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ExternalXmlFileConfigSectionLocatorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }						
}
