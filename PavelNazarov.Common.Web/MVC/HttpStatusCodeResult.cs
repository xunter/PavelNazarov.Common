using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class HttpStatusCodeResult<T> : HttpStatusCodeResult where T : class
    {
        private readonly T _dto;
        private readonly string _statusDescription;

        public HttpStatusCodeResult(int httpStatusCode, T dto, string statusDescription = null)
            : base(httpStatusCode)
        {
            _dto = dto;
            _statusDescription = statusDescription;
        }

        public HttpStatusCodeResult(HttpStatusCode httpStatusCode, T dto)
            : this((int)httpStatusCode, dto)
        { }

        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);

            if (!String.IsNullOrEmpty(_statusDescription))
                context.HttpContext.Response.AppendHeader("Content-Description", _statusDescription);

            context.HttpContext.Response.ContentType = "application/json";
            var jsonEncodedResult = System.Web.Helpers.Json.Encode(_dto);
            context.HttpContext.Response.Output.Write(jsonEncodedResult);
        }
    }
}
