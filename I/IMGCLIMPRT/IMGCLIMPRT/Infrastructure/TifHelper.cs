using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace IMGCLIMPRT
{
    class TifHelper
    {
        public bool IsMultiPageTif(ZipEntry ze)
        {
            if (Path.GetExtension(ze.FileName).ToLower().Trim('.').IsIn("tif", "tiff"))
                using (MemoryStream ms = new MemoryStream())
                {
                    Image multi = GetImage(ze, ms);
                    FrameDimension frameDimensions = new FrameDimension(multi.FrameDimensionsList[0]);
                    int frames = multi.GetFrameCount(frameDimensions);
                    if (frames <= 1)
                        return false;
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Pulls each page from the given tif into a separate file in the given outputdirectory, and returns a list of the names of each file.
        /// </summary>
        public IEnumerable<string> ExplodeTifs(ZipEntry ze, string outputDirectory)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image multi = GetImage(ze, ms);
                FrameDimension frameDimensions = new FrameDimension(multi.FrameDimensionsList[0]);
                int frames = multi.GetFrameCount(frameDimensions);
                string path = ze.FileName.Substring(ze.FileName.LastIndexOf('/') + 1).ToLower();
                path = Path.Combine(outputDirectory, path);
                List<string> paths = new List<string>();
                for (int f = 0; f < frames; f++)
                {
                    multi.SelectActiveFrame(frameDimensions, f);
                    string pagePath = path.Substring(0, path.LastIndexOf(".tif")) + "_" + f.ToString() + "_" + Guid.NewGuid().ToString().Replace("-", "") + ".tif";
                    paths.Add(Path.GetFileName(pagePath));
                    using (Bitmap page = new Bitmap(multi))
                        page.Save(pagePath, ImageFormat.Tiff);
                }
                return paths;
            }
        }

        private Image GetImage(ZipEntry ze, MemoryStream ms)
        {
            ze.Extract(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return Image.FromStream(ms);
        }

    }
}
