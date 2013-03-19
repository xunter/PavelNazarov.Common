using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PavelNazarov.Common.IO
{
    public static class FileInfoExtensions
    {
        public static string GetVersionBasedOnLastModifiedDate(this FileInfo fileInfo)
        {
            if (!fileInfo.Exists) return null;
            string version = fileInfo.LastWriteTime.ToString("yyyyMMddHHmmss");
            return version;
        }

        /// <summary>
        /// Touches the file. It is command like a unix touch one. If the file doesn't exist it will be created and, otherwise, the last modified and write time will be changed to the current.
        /// </summary>
        /// <param name="fi"></param>
        public static void Touch(this FileInfo fi)
        {
            if (!fi.Exists)
            {
                var fs = fi.Create();
                fs.Close();
            }
            else
            {
                var nowDate = DateTime.Now;
                fi.LastAccessTime = nowDate;
                fi.LastWriteTime = nowDate;
            }
        }
    }

}
