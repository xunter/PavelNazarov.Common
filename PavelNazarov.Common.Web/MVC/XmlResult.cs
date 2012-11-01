using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class XmlResult : ActionResult
    {
        private readonly string _xmlRaw;

        /// <summary>
        /// Returns raw xml data
        /// </summary>
        public string XmlRaw
        {
            get { return _xmlRaw; }
        }

        public XmlResult(string xmlRaw)
        {
            _xmlRaw = xmlRaw;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            httpContext.Response.ContentType = "text/xml";
            httpContext.Response.Output.Write(_xmlRaw);
        }
    }
}
