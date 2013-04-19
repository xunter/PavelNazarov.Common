using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using PavelNazarov.Common.IO;
using PavelNazarov.Common.Security;
using System.Security.Cryptography;
using System.Net;

namespace PavelNazarov.Common.Web.Handlers
{
    /// <summary>
    /// It handles an entire css file and appends a version for urls based on its last modified date
    /// </summary>
    public class CssResourceVersionHandler : IHttpHandler
    {
        const string CSS_CONTENT_TYPE = "text/css";
        public static readonly Regex ResourceUrlRegularExpression = new Regex("url(\\(['\"]?)([A-Za-z0-9-_%&\\?\\/\\.]+)(['\"]?\\)[;]?)", RegexOptions.Compiled);

        /// <summary>
        /// Get image version from config file
        /// </summary>
        /// <returns></returns>
        private string GetFileVersion(FileInfo fileInfo)
        {
            return fileInfo.GetVersionBasedOnLastModifiedDate();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Process an http request and perform the requested css file. If it is modified then it will be sent to a browser and will not, otherwise
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            string filePath = context.Request.PhysicalPath;
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                response.StatusCode = 404;
                response.End();
                return;
            }

            //response.CacheControl = "max-age=0, must-revalidate";
            string version = GetFileVersion(fileInfo);
            string[] resourceVersions = null;
            ProcessCssFileAndWriteIt(fileInfo, response.Output, version, out resourceVersions);
            string etagValue = GetETagValueForCssFile(fileInfo, resourceVersions);

            string receivedETag = context.Request.Headers["If-None-Match"];
            if (!String.IsNullOrEmpty(receivedETag) && receivedETag.Trim('\"') == etagValue)
            {
                response.ClearContent();
                response.StatusCode = (int)HttpStatusCode.NotModified;
            }
            else
            {                
                response.AddHeader("Pragma", "no-cache");
                response.AddHeader("Cache-Control", "max-age=0, must-revalidate");
                response.ContentType = CSS_CONTENT_TYPE;
                response.AddHeader("Last-Modified", fileInfo.LastWriteTime.ToString("r"));
                response.AddHeader("ETag", String.Format("\"{0}\"", etagValue));
                response.StatusCode = (int)HttpStatusCode.OK; 
            }
            response.End();
        }

        /// <summary>
        /// Gets a ETag value for an entire css file based on css file and given resource versions
        /// </summary>
        /// <param name="cssFile">css file</param>
        /// <param name="resourceVersions">resource versions</param>
        /// <returns>computed ETag value</returns>
        private string GetETagValueForCssFile(FileInfo cssFile, string[] resourceVersions)
        {
            var sb = new StringBuilder();
            sb.Append(cssFile.LastWriteTime);
            sb.Append(String.Join(String.Empty, resourceVersions));
            string hash = HashFactory<MD5>.CreateText(sb.ToString());
            string etagValue = hash;
            return etagValue;
        }

        /// <summary>
        /// Process css file and write it to response
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="writer">outpur writer</param>
        /// <param name="version">image version</param>
        /// <param name="fileInfo">css file info instance</param>
        /// <param name="resourceVersions">resource versions</param>
        private void ProcessCssFileAndWriteIt(FileInfo fileInfo, TextWriter writer, string version, out string[] resourceVersions)
        {
            resourceVersions = null;
            using (Stream stream = fileInfo.OpenRead())
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    string cssText = reader.ReadToEnd();
                    if (String.IsNullOrEmpty(version))
                    {
                        writer.Write(cssText);
                        resourceVersions = new[] { String.Empty };
                    }
                    else
                    {
                        writer.Write(GetPerformedCssText(cssText, fileInfo, version, out resourceVersions));
                    }
                }
            }
        }

        /// <summary>
        /// Get performed by regular expression css text
        /// </summary>
        /// <param name="cssText">css text</param>
        /// <param name="version">image version</param>
        /// <param name="cssFileInfo">css file info instance</param>
        /// <param name="resourceVersions">resource versions</param>
        /// <returns></returns>
        private string GetPerformedCssText(string cssText, FileInfo cssFileInfo, string version, out string[] resourceVersions)
        {
            var resourceVersionList = new LinkedList<string>();
            string cssDitPath = cssFileInfo.DirectoryName;
            string processedText = ResourceUrlRegularExpression.Replace(cssText, m =>
            {
                StringBuilder sb = new StringBuilder("url");
                for (int i = 1; i < m.Groups.Count; i++)
                {
                    Group group = m.Groups[i];
                    if (i == 2)
                    {
                        string path = group.Value;
                        string resourcePath = Path.Combine(cssDitPath, path);
                        var resFileInfo = new FileInfo(resourcePath);
                        if (resFileInfo.Exists)
                        {
                            string resFileVersion = GetFileVersion(resFileInfo);
                            resourceVersionList.AddLast(resFileVersion);
                            sb.Append(path);
                            if (path.Contains("?"))
                            {
                                sb.AppendFormat("&_v={0}", resFileVersion);
                            }
                            else
                            {
                                sb.AppendFormat("?_v={0}", resFileVersion);
                            }
                        }
                        else
                        {
                            sb.Append(path);
                        }
                    }
                    else
                    {
                        sb.Append(group.Value);
                    }
                }
                return sb.ToString();
            });
            resourceVersions = resourceVersionList.ToArray();
            return processedText;
        }
    }
}
