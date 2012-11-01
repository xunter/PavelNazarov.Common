using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class FileDownloadResult : FileContentResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            //base.ExecuteResult(context);
            
            response.ContentType = base.ContentType;
            response.AppendHeader("Content-Length", FileContents.Length.ToString());
            response.CacheControl = "no-cache";
            response.OutputStream.Write(FileContents, 0, FileContents.Length);

            var browserName = request.Browser.Browser.ToLower();
            if (browserName == "ie")
            {
                var encodedFileName = HttpUtility.UrlPathEncode(base.FileDownloadName);
                var headerValue = MakeContentDisposition(encodedFileName);
                response.AppendHeader("Content-Disposition", headerValue);
            }
            else if (browserName == "firefox")
            {
                var headerValue = MakeContentDisposition(base.FileDownloadName, true);
                response.AppendHeader("Content-Disposition", headerValue);
            }
            else
            {
                var headerValue = MakeContentDisposition(base.FileDownloadName);
                response.AppendHeader("Content-Disposition", headerValue); 
            }
        }

        /// <summary>
        /// Makes a Content-Disposition header value
        /// </summary>
        /// <param name="filename">filename</param>
        /// <param name="isAddQuotes">add quotes to the header</param>
        /// <returns>a value of the header</returns>
        private string MakeContentDisposition(string filename, bool isAddQuotes = false)
        {
            if (isAddQuotes)
                return String.Format("attachment; filename=\"{0}\"", filename);
            else
                return String.Format("attachment; filename={0}", filename);
        }

        public FileDownloadResult(byte[] fileContents, string contentType, string fileName)
            : base(fileContents, contentType)
        {
            base.FileDownloadName = fileName;
        }
    }
}
