using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PanelBase
{
    public class ScreenShot
    {
        //------------------------------------------------------------------------------

        //public static Bitmap GetSnapshot()
        //{
        //    var handle = Process.GetCurrentProcess().MainWindowHandle;

        //    IntPtr hdcSrc = User32.GetWindowDC(handle);

        //    var windowRect = new User32.RECT();
        //    User32.GetWindowRect(handle, ref windowRect);

        //    Size borderSize = SystemInformation.FrameBorderSize;
        //    var width = windowRect.right - windowRect.left;
        //    var height = windowRect.bottom - windowRect.top;
        //    var hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
        //    var hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
        //    var hOld = GDI32.SelectObject(hdcDest, hBitmap);

        //    GDI32.BitBlt(hdcDest, -1 * borderSize.Width, -1 * (borderSize.Height + SystemInformation.CaptionHeight), width - borderSize.Width, height - borderSize.Height, hdcSrc, 0, 0, GDI32.SRCCOPY);

        //    GDI32.SelectObject(hdcDest, hOld);

        //    GDI32.DeleteDC(hdcDest);
        //    User32.ReleaseDC(handle, hdcSrc);

        //    Bitmap img = Image.FromHbitmap(hBitmap);

        //    GDI32.DeleteObject(hBitmap);

        //    return AdjustBrightness(img, -80);
        //}

        //private class GDI32
        //{
        //    public const int SRCCOPY = 0x00CC0020;

        //    [DllImport("gdi32.dll")]
        //    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
        //        int nWidth, int nHeight, IntPtr hObjectSource,
        //        int nXSrc, int nYSrc, int dwRop);
        //    [DllImport("gdi32.dll")]
        //    public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
        //        int nHeight);
        //    [DllImport("gdi32.dll")]
        //    public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        //    [DllImport("gdi32.dll")]
        //    public static extern bool DeleteDC(IntPtr hDC);
        //    [DllImport("gdi32.dll")]
        //    public static extern bool DeleteObject(IntPtr hObject);
        //    [DllImport("gdi32.dll")]
        //    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        //}

        //private class User32
        //{
        //    [StructLayout(LayoutKind.Sequential)]
        //    public struct RECT
        //    {
        //        public int left;
        //        public int top;
        //        public int right;
        //        public int bottom;
        //    }

        //    [DllImport("user32.dll")]
        //    public static extern IntPtr GetDesktopWindow();
        //    [DllImport("user32.dll")]
        //    public static extern IntPtr GetWindowDC(IntPtr hWnd);
        //    [DllImport("user32.dll")]
        //    public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        //    [DllImport("user32.dll")]
        //    public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        //}

        //------------------------------------------------------------------------------

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static Bitmap GetSnapshot(Process proc)
        {
            Rect srcRect;
            if (!proc.MainWindowHandle.Equals(IntPtr.Zero))
            {
                //no good, will get teamviewer button size
                //if (GetWindowRect(proc.MainWindowHandle, out srcRect))

                if (GetWindowRect(GetForegroundWindow(), out srcRect))
                {
                    Size borderSize = SystemInformation.FrameBorderSize;
                    const int c = 2;
                    int width = srcRect.Right - srcRect.Left - borderSize.Width * c;
                    int height = srcRect.Bottom - srcRect.Top - borderSize.Height * c - SystemInformation.CaptionHeight;

                    //size not right
                    if (width <= 0 || height <= 0)
                        return null;

                    var bmp = new Bitmap(width, height);

                    Graphics graphics = Graphics.FromImage(bmp);

                    try
                    {
                        var x = srcRect.Left + borderSize.Width;
                        var y = srcRect.Top + borderSize.Height + SystemInformation.CaptionHeight;
                        graphics.CopyFromScreen(x, y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                    }
                    catch (Exception)
                    {
                        graphics.Dispose();
                        return null;
                    }
                    graphics.Dispose();
                    return AdjustBrightness(bmp, -80);
                }
            }

            return null;
        }

        public static Bitmap AdjustBrightness(Bitmap image, Int32 value)
        {
            try
            {
                var tempBitmap = image;
                var finalValue = value / 255.0f;
                var newBitmap = new Bitmap(tempBitmap.Width, tempBitmap.Height);

                var newGraphics = Graphics.FromImage(newBitmap);
                float[][] floatColorMatrix ={
                     new float[] {1, 0, 0, 0, 0},
                     new float[] {0, 1, 0, 0, 0},
                     new float[] {0, 0, 1, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new[] {finalValue, finalValue, finalValue, 1, 1}
                 };

                var newColorMatrix = new System.Drawing.Imaging.ColorMatrix(floatColorMatrix);
                var attributes = new System.Drawing.Imaging.ImageAttributes();
                attributes.SetColorMatrix(newColorMatrix);
                newGraphics.DrawImage(tempBitmap, new Rectangle(0, 0, tempBitmap.Width, tempBitmap.Height), 0, 0, tempBitmap.Width, tempBitmap.Height, GraphicsUnit.Pixel, attributes);
                attributes.Dispose();
                newGraphics.Dispose();
                return newBitmap;
            }
            catch (Exception)
            {
                return image;
            }
        }
    }
}