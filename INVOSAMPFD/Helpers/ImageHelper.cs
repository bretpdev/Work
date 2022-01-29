using MinCap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVOSAMPFD
{
    public static class ImageHelper
    {
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
    }
}
