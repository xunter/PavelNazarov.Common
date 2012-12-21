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
    }
}
