using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class TextResult : ActionResult
    {
        private readonly string _text;

        public TextResult(string text)
        {
            _text = text;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Output.Write(_text);
            response.ContentType = "text/plain";
        }
    }
}
