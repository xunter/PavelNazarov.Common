using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;

namespace PavelNazarov.Common.Web.MVC
{
    public static class HtmlIncludesExtensions
    {
        /// <summary>
        /// Includes an html markup like <link rel=Stylesheet type=text/css href=[YOUR_HREF] />
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="path"></param>
        /// <param name="includeVersionBasedOnLastModifiedDate">adding the query param "v" = [yyyyMMddHHmmss] based on file last modified date</param>
        /// <returns></returns>
        public static IHtmlString IncludeCSS(this HtmlHelper htmlHelper, string path, bool includeVersionBasedOnLastModifiedDate = true)
        {
            string physicalPath = htmlHelper.ViewContext.HttpContext.Server.MapPath(path);
            var fileInfo = new FileInfo(physicalPath);
            if (!fileInfo.Exists) return new RawHtmlString(String.Empty);
            var lastModifiedDate = fileInfo.LastWriteTime;
            var sb = new StringBuilder();
            using (var textWriter = new StringWriter(sb))
            {
                using (var htmlWriter = new HtmlTextWriter(textWriter))
                {
                    var htmlLink = new HtmlLink();
                    htmlLink.Attributes["rel"] = "Stylesheet";
                    htmlLink.Href = String.Format("{0}?v={1:yyyyMMddHHmmss}", VirtualPathUtility.ToAbsolute(path), lastModifiedDate);
                    htmlLink.Attributes["type"] = "text/css";
                    htmlLink.RenderControl(htmlWriter);
                }
            }
            return new RawHtmlString(sb.ToString());
        }

        /// <summary>
        /// Includes an html markup like <script type=text/css src=[YOUR_HREF]></script>
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="path"></param>
        /// <param name="includeVersionBasedOnLastModifiedDate">adding the query param "v" = [yyyyMMddHHmmss] based on file last modified date</param>
        /// <returns></returns>
        public static IHtmlString IncludeJS(this HtmlHelper htmlHelper, string path, bool includeVersionBasedOnLastModifiedDate = true)
        {
            string physicalPath = htmlHelper.ViewContext.HttpContext.Server.MapPath(path);
            var fileInfo = new FileInfo(physicalPath);
            if (!fileInfo.Exists) return new RawHtmlString(String.Empty);
            var lastModifiedDate = fileInfo.LastWriteTime;
            var sb = new StringBuilder();
            using (var textWriter = new StringWriter(sb))
            {
                using (var htmlWriter = new HtmlTextWriter(textWriter))
                {
                    var htmlScript = new HtmlGenericControl("script");
                    htmlScript.Attributes["type"] = "text/javascript";
                    htmlScript.Attributes["src"] = String.Format("{0}?v={1:yyyyMMddHHmmss}", VirtualPathUtility.ToAbsolute(path), lastModifiedDate);
                    htmlScript.RenderControl(htmlWriter);
                }
            }
            return new RawHtmlString(sb.ToString());
        }
    }
}
