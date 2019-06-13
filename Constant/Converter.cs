using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Constant
{
    public static class Converter
    {
        public static Bitmap ImageFromArray(Byte[] array, int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var position = (y * width + x) * 4;
                    bitmap.SetPixel(x, y, Color.FromArgb(array[position + 3], array[position + 2], array[position + 1], array[position]));
                }
            }
            return bitmap;
        }

        public static Byte[] ImageToByte2(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

            var byteArray = new Byte[Math.Abs(bmpData.Stride) * bitmap.Height];
            Marshal.Copy(bmpData.Scan0, byteArray, 0, byteArray.Length);

            bitmap.UnlockBits(bmpData);

            return byteArray;
        }

        public static Bitmap ImageTrim(Bitmap image)
        {
            //get image data
            BitmapData bd = image.LockBits(new Rectangle(Point.Empty, image.Size),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int[] rgbValues = new int[image.Height * image.Width];
            Marshal.Copy(bd.Scan0, rgbValues, 0, rgbValues.Length);
            image.UnlockBits(bd);

            int left = bd.Width;
            int top = bd.Height;
            int right = 0;
            int bottom = 0;

            //determine top
            for (int i = 0; i < rgbValues.Length; i++)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    top = r;
                    break;
                }
            }

            //determine bottom
            for (int i = rgbValues.Length - 1; i >= 0; i--)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    break;
                }
            }

            if (bottom > top)
            {
                for (int r = top + 1; r < bottom; r++)
                {
                    //determine left
                    for (int c = 0; c < left; c++)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (left > c)
                            {
                                left = c;
                                break;
                            }
                        }
                    }

                    //determine right
                    for (int c = bd.Width - 1; c > right; c--)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (right < c)
                            {
                                right = c;
                                break;
                            }
                        }
                    }
                }
            }

            int width = right - left + 1;
            int height = bottom - top + 1;

            //copy image data
            int[] imgData = new int[width * height];
            for (int r = top; r <= bottom; r++)
            {
                Array.Copy(rgbValues, r * bd.Width + left, imgData, (r - top) * width, width);
            }

            //create new image
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData nbd
                = newImage.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(imgData, 0, nbd.Scan0, imgData.Length);
            newImage.UnlockBits(nbd);

            return newImage;
        }

        public static BitmapSource ImageToBitmapSource(Image image)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, image.RawFormat);

                    //if (image.RawFormat.Equals(ImageFormat.Jpeg))
                    //{
                    //    photo.Save(memoryStream, ImageFormat.Jpeg);
                    //} 
                    //else if (image.RawFormat.Equals(ImageFormat.Gif))
                    //{
                    //    photo.Save(memoryStream, ImageFormat.Gif);
                    //} 
                    //else if (image.RawFormat.Equals(ImageFormat.Png))
                    //{
                    //    photo.Save(memoryStream, ImageFormat.Png);
                    //} 
                    //else if (image.RawFormat.Equals(ImageFormat.Bmp))
                    //{
                    //    photo.Save(memoryStream, ImageFormat.Bmp);
                    //}

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return CreateBitmapSourceFromBitmap(memoryStream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static BitmapSource CreateBitmapSourceFromBitmap(Stream stream)
        {
            var bitmapDecoder = BitmapDecoder.Create(
                stream,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnLoad);

            var writable = new WriteableBitmap(bitmapDecoder.Frames.Single());
            writable.Freeze();

            return writable;
        }
    }
}