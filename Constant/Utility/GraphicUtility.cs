using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Constant.Utility
{
    public static class GraphicUtility
    {
        public static byte[] ToBytes(this Image image)
        {
            if (image == null) return new byte[] { };

            using (var memory = new MemoryStream())
            {
                image.Save(memory, image.RawFormat);

                var data = memory.ToArray();

                return data;
            }
        }

        public static byte[] ToBytes(this Image image, ImageFormat format)
        {
            if (image == null) return new byte[] { };

            using (var memory = new MemoryStream())
            {
                image.Save(memory, format);

                var data = memory.ToArray();

                return data;
            }
        }

        public static Image ScaleImage(this Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static Image Resize(this Image image, int width, int height)
        {
            var newImage = new Bitmap(width, height);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);
            return newImage;
        }

        public static Image ToImage(this byte[] buffer)
        {
            if (buffer == null || !buffer.Any()) return null;

            MemoryStream oMemoryStream = null;
            Bitmap oBitmap;
            //建立副本
            byte[] data = (byte[])buffer.Clone();
            try
            {
                oMemoryStream = new MemoryStream(data) { Position = 0 };
                //設定資料流位置
                var oImage = Image.FromStream(oMemoryStream);
                //建立副本
                oBitmap = new Bitmap(oImage);
            }
            finally
            {
                if (oMemoryStream != null)
                {
                    oMemoryStream.Close();
                    oMemoryStream.Dispose();
                }
            }

            return oBitmap;
        }

        public static Bitmap ToBitmap(this byte[] data)
        {
            Bitmap image = null;
            if (data != null && data.Any())
            {
                using (var memStream = new MemoryStream(data))
                {
                    image = new Bitmap(memStream, true);
                }
            }

            return image;
        }

        public static Image ToImage(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return null;

            byte[] fileBytes = Convert.FromBase64String(base64String);

            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                Image streamImage = Image.FromStream(ms);

                return streamImage;
            }
        }

        public static string ToBase64String(this Image image, ImageFormat format)
        {
            using (var memStream = new MemoryStream())
            {
                image.Save(memStream, format);

                byte[] data = memStream.ToArray();

                var base64Image = Convert.ToBase64String(data);

                return base64Image;
            }
        }

        public static GraphicFormat GetFormat(this Image image)
        {
            if (image.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return GraphicFormat.Jpeg;
            }

            if (image.RawFormat.Equals(ImageFormat.Png))
            {
                return GraphicFormat.Png;
            }

            if (image.RawFormat.Equals(ImageFormat.Gif))
            {
                return GraphicFormat.Gif;
            }

            if (image.RawFormat.Equals(ImageFormat.Bmp))
            {
                return GraphicFormat.Bmp;
            }

            if (image.RawFormat.Equals(ImageFormat.Icon))
            {
                return GraphicFormat.Icon;
            }

            if (image.RawFormat.Equals(ImageFormat.Emf))
            {
                return GraphicFormat.Emf;
            }

            if (image.RawFormat.Equals(ImageFormat.Exif))
            {
                return GraphicFormat.Exif;
            }

            if (image.RawFormat.Equals(ImageFormat.Tiff))
            {
                return GraphicFormat.Tiff;
            }

            if (image.RawFormat.Equals(ImageFormat.Wmf))
            {
                return GraphicFormat.Wmf;
            }

            return GraphicFormat.Unknown;
        }

        /// <summary>
        /// Create a deep copy and keep the original Pixelformat
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap DeepClone(this Bitmap bitmap)
        {
            return bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
        }
    }

    public enum GraphicFormat { Unknown, Png, Jpeg, Wmi, Bmp, Emf, Exif, Icon, Gif, Tiff, Wmf }
}
