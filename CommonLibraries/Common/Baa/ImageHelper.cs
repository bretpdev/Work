using MinCap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.Baa
{
    public static class ImageHelper
    {
        public static string GetHtmlImage(Image image)
        {
            return "<img src='data:image/png;base64," +
              Convert.ToBase64String(image.ToBytes(), Base64FormattingOptions.None) + "' />";
        }
        public static byte[] ToBytes(this Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static Image ToImage(this byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
                return new Bitmap(ms);
        }

        public static Image GetImageFromHwnd(IntPtr hwnd)
        {
            WindowSnap ws = WindowSnap.GetWindowSnap(hwnd, true);
            if (ws.Image == null)
                ws = WindowSnap.GetWindowSnap(hwnd, false);
            return ws.Image;
        }

        public static IntPtr GetHwndFromTitle(string title)
        {
            return FindWindowByCaption(IntPtr.Zero, title);
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
    }
}
