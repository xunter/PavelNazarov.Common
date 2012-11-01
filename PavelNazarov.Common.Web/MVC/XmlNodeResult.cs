using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.XPath;
using System.Xml;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class XmlNodeResult : ActionResult
    {
        private readonly XmlNode _xmlNode;

        public XmlNode Node 
        {
            get { return _xmlNode; }
        }

        public XmlNodeResult(XmlNode xml)
        {
            _xmlNode = xml;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            httpContext.Response.ContentType = "text/xml";

            using (XmlWriter xmlWriter = XmlWriter.Create(httpContext.Response.Output))
            {
                _xmlNode.WriteTo(xmlWriter);
            }
        }
    }
}
