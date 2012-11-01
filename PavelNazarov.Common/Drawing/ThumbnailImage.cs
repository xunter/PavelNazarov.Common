using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace PavelNazarov.Common.Images
{
    public class ThumbnailImage
    {
        private static ThumbnailImage _instance;

        public static ThumbnailImage GetInstance()
        {
            if (_instance == null)
                _instance = new ThumbnailImage();
            return _instance;
        }

        private ThumbnailImage()
        { }

        /// <summary>
        /// Create thumbnail image
        /// </summary>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="path">source image path</param>
        /// <returns>thumbnail image</returns>
        public Image Create(int width, int height, Stream stream, bool proportional = true)
        {
            using (Image image = Image.FromStream(stream))
            {
                int _width = width;
                int _height = height;
                if (image.Width > image.Height)
                {
                    _height = (int)Math.Floor((double)height * image.Height / image.Width);
                }
                else if (image.Height > image.Width)
                {
                    _width = (int)Math.Floor((double)width * image.Width / image.Height);
                }

                return image.GetThumbnailImage(_width, _height, null, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Create thumbnail omage and write it to stream
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        public void CreateToStream(int width, int height, string path, Stream stream, bool proportional = true)
        {
            using (var fileInputStream = File.OpenRead(path))
            {
                using (Image image = Create(width, height, fileInputStream, proportional))
                {
                    ImageFormat format = ExtractImageFormat(path);
                    image.Save(stream, format);
                }
            }
        }

        public void CreateToStream(int width, int height, Stream inputStream, string imageType, Stream stream, bool proportional = true)
        {
            using (Image image = Create(width, height, inputStream, proportional))
            {
                ImageFormat format = ExtractImageFormatFromExt(imageType);
                image.Save(stream, format);
            }
        }
        
        /// <summary>
        /// Extract image ofrmat from file extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static ImageFormat ExtractImageFormat(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            var imageExt = fileInfo.Extension.ToLower();
            return ExtractImageFormatFromExt(imageExt);
        }

        private static ImageFormat ExtractImageFormatFromExt(string ext)
        {
            switch (ext)
            {
                case ".jpg": return ImageFormat.Jpeg;
                case ".gif": return ImageFormat.Gif;
                case ".png": return ImageFormat.Png;
                case ".bmp": return ImageFormat.Bmp;
                default: throw new InvalidOperationException("Bad extension! Value: " + ext);
            } 
        }
    }
}
