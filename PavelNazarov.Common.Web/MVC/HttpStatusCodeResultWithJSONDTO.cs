using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Helpers;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class HttpStatusCodeResultWithJSONDTO : HttpStatusCodeResult
    {
        //private readonly Json _jsonHelper = new Json

        public object DTO { get; private set; }

        public HttpStatusCodeResultWithJSONDTO(int statusCode, string statusDescription = null, object dto = null)
            : base(statusCode, statusDescription)
        {
            DTO = dto;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);

            if (DTO != null)
            {
                context.HttpContext.Response.ContentType = "application/json";
                string json = Json.Encode(DTO);
                context.HttpContext.Response.Output.Write(json);
            }
        }
    }
}
