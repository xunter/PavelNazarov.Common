using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PavelNazarov.Common.Web.MVC
{

    public class HttpStatusCodeContextResult : HttpStatusCodeResult
    {
        private readonly HttpStatusCodeResult _original;

        public Dictionary<string, string> AdditionalHeaders { get; set; }

        public HttpStatusCodeContextResult(HttpStatusCodeResult original)
            : base(original.StatusCode, original.StatusDescription)
        {
            _original = original;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var response = httpContext.Response;
            if (AdditionalHeaders != null && AdditionalHeaders.Count > 0)
            {
                foreach (var kv in AdditionalHeaders)
                {
                    response.AddHeader(kv.Key, kv.Value);
                }
            }
            _original.ExecuteResult(context);
        }
    }
}
