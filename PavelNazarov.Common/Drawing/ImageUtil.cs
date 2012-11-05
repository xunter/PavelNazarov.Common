using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace PavelNazarov.Common.Images
{
    public static class ImageUtil
    {
        public static ImageFormat GetImageFormatByExtension(string ext)
        {
            var extDotless = GetExtDotless(ext);
            switch (extDotless)
            {
                case "png": return ImageFormat.Png;
                case "gif": return ImageFormat.Gif;
                case "jpg": 
                case "jpeg": return ImageFormat.Jpeg;
                case "bmp": return ImageFormat.Bmp;
                default: throw new ArgumentOutOfRangeException("ext");
            }
        }

        private static string GetExtDotless(string ext)
        {
            var extDotless = ext.ToLower().TrimStart('.');
            return extDotless;
        }

        public static string GetMimeTypeByExt(string ext)
        {
            var extDotless = GetExtDotless(ext);
            switch (extDotless)
            {
                case "jpg": return "image/jpeg";
                case "gif": return "image/gif";
                case "png": return "image/png";
                case "bmp": return "image/bmp";
                default: throw new InvalidOperationException("Unsupported extension was occurred!");
            }
        }
    }
}
