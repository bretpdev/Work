using System;
using System.Collections.Generic;
using System.Linq;
using Ionic.Zip;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using BitMiracle.LibTiff.Classic;
using System.Threading;
using System.Runtime.InteropServices;

namespace CommercialArchiveImport
{
    public class NoIndexFileException : Exception { }
    public class MultipleIndexFilesException : Exception { }
    public class NoEOJAccessException : Exception { }
    public partial class MainProcessor : IDisposable
    {
        private ZipFile Zip { get; set; }
        Stack<ZipFile> ZipCopies { get; set; }
        private string ZipLocation { get; set; }
        private string ResultsLocation { get; set; }
        private string BatchNumber { get; set; }
        private readonly int ThreadCount = 8;

        private MainProcessor(ZipFile zf, string zipLocation, string resultsLocation, string batchNumber)
        {
            Zip = zf;
            ZipCopies = new Stack<ZipFile>();
            ZipLocation = zipLocation;
            ResultsLocation = resultsLocation;
            BatchNumber = batchNumber;
        }
        static MainProcessor mp;
        public static EojResults Process(string zipLocation, string resultsLocation, string batchNumber)
        {
            using (ZipFile zf = new ZipFile(zipLocation))
            {
                mp = new MainProcessor(zf, zipLocation, resultsLocation, batchNumber);
                return mp.Process();
            }
        }

        public static void Abort()
        {
            if (mp != null)
                mp.Dispose();
        }

        List<ThreadedDocumentProcessor> threads = new List<ThreadedDocumentProcessor>();
        private EojResults Process()
        {
            var indices = Zip.Where(ze => ze.FileName.ToLower().EndsWith(".idx"));
            if (indices.Count() > 1)
                throw new MultipleIndexFilesException();
            if (indices.Count() < 1)
                throw new NoIndexFileException();

            List<Document> docs = Document.ParseFile(indices.Single().GetStringContents());
            ProgressHelper.Next("Processing");
            ProgressHelper.Increments = docs.Count;

            threads = ThreadedDocumentProcessor.CalculateThreads(this, docs, ThreadCount);
            foreach (ThreadedDocumentProcessor thread in threads)
                thread.Start();
            while (threads.Any(o => o.Running)) Thread.Sleep(100); //wait until all threads are done

            foreach (ZipFile zf in ZipCopies)
                zf.Dispose();

            lock (Zip)
            {
                Eoj eoj = new Eoj(Zip, docs);
                EojResults result = eoj.Publish(ResultsLocation, ZipLocation);
                if (result == null)
                    throw new NoEOJAccessException();
                return result;
            }
        }

        private bool IsMultiPage(ZipEntry ze)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image multi = ze.GetImage(ms);
                FrameDimension frameDimensions = new FrameDimension(multi.FrameDimensionsList[0]);
                int frames = multi.GetFrameCount(frameDimensions);
                if (frames <= 1)
                    return false;
                return true;
            }
        }

        private IEnumerable<string> ExplodeTiffs(ZipEntry ze)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image multi = ze.GetImage(ms);
                FrameDimension frameDimensions = new FrameDimension(multi.FrameDimensionsList[0]);
                int frames = multi.GetFrameCount(frameDimensions);
                string path = ze.FileName.Substring(ze.FileName.LastIndexOf('/') + 1).ToLower();
                path = Path.Combine(ResultsLocation, path);
                List<string> paths = new List<string>();
                for (int f = 0; f < frames; f++)
                {
                    multi.SelectActiveFrame(frameDimensions, f);
                    string pagePath = path.Substring(0, path.LastIndexOf(".tif")) + "_" + f.ToString() + ".tif";
                    paths.Add(Path.GetFileName(pagePath));
                    using (Bitmap page = new Bitmap(multi))
                        page.Save(pagePath, ImageFormat.Tiff);
                }
                return paths;
            }
        }

        private void ValidateDocument(Document doc)
        {
            if (doc.Invalid)
                return;

            var file = Zip.Find(doc.ImagePath);
            if (file == null)
                doc.SetError(string.Format("Couldn't find file {0}.  Line was skipped.", doc.ImagePath), DocumentErrorType.NoImage);
        }

        private static iTextSharp.text.Font font;
        static MainProcessor()
        {
            var windows = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System));
            var fonts = Path.Combine(windows.FullName, "Fonts");
            var cour = Path.Combine(fonts, "cour.ttf"); //load courier new font (probably)
            font = new iTextSharp.text.Font(BaseFont.CreateFont(cour, BaseFont.CP1252, BaseFont.EMBEDDED), 6);
        }

        private void ProcessLine(Document doc, ZipFile copy)
        {
            ZipEntry file = null;

            file = copy.Find(doc.ImagePath);
            string filename = doc.ImagePath;
            if (filename.ToLower().EndsWith(".txt"))
                filename = filename.Substring(0, filename.Length - 4) + ".pdf";
            filename = Path.Combine(ResultsLocation, filename);
            if (doc.Extension == "txt")
            {
                using (FileStream fs = File.Create(filename))
                using (MemoryStream ms = new MemoryStream())
                {
                    iTextSharp.text.Document text = new iTextSharp.text.Document();
                    var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(text, fs);
                    text.Open();
                    file.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(ms))
                        text.Add(new iTextSharp.text.Paragraph(sr.ReadToEnd(), font));
                    text.Close();
                }
                doc.ImagePath = Path.GetFileName(filename);
                CreateCTLFile(doc);
            }
            else if ((doc.Extension == "tif" || doc.Extension == "tiff") && IsMultiPage(file))
            {
                var paths = ExplodeTiffs(file);
                CreateCTLFile(doc, paths);
            }
            else
            {
                using (FileStream fs = File.Create(filename))
                using (MemoryStream ms = new MemoryStream())
                {
                    file.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = new byte[ms.Length];
                    ms.Read(bytes, 0, (int)ms.Length);
                    fs.Write(bytes, 0, (int)ms.Length);
                }
                CreateCTLFile(doc);
            }

            lock (Zip)
                Zip.RemoveEntry(file);
        }

        private bool CreateCTLFile(Document doc)
        {
            return CreateCTLFile(doc, null);
        }
        private bool CreateCTLFile(Document doc, IEnumerable<string> tiffNames)
        {
            string ctlName = doc.ImagePath.Substring(0, doc.ImagePath.LastIndexOf('.')) + ".ctl";
            string path = Path.Combine(ResultsLocation, ctlName);
            List<string> lines = new List<string>();
            if (doc.Extension == "tiff" || doc.Extension == "tif" || doc.Extension == "t001")
            {
                if (tiffNames == null)
                    tiffNames = new string[] { doc.ImagePath };
                lines.Add(
                    "~^Folder~Commercial Packet" +
                    "^Type~FEDERAL_TYPE" +
                    "^Attribute~ACCOUNT_NUMBER~STR~" +
                    "^Attribute~BATCH_NUM~STR~" + BatchNumber +
                    "^Attribute~DOC_DATE~STR~" + doc.DocDate +
                    "^Attribute~DOC_ID~STR~" + doc.DocID +
                    "^Attribute~DOC_TIME~STR~" +
                    "^Attribute~LENDER_CODE~STR~" +
                    "^Attribute~MSG~STR~" +
                    "^Attribute~SCAN_DATE~STR~" +
                    "^Attribute~SSN~STR~" + doc.SSN +
                    "^Attribute~VENDOR_NUM~STR~" +
                    "^Attribute~DESCRIPTION~STR~Commercial Packet"
                );
                lines.Add(
                    "ImageDoc~Scanned Form(s)" +
                    "^Type~FEDERAL_TYPE" +
                    "^Attribute~MSG~STR~" +
                    "^Attribute~SCAN_DATE~STR~" +
                    "^Attribute~SSN~STR~" + doc.SSN +
                    "^Attribute~VENDOR_NUM~STR~" +
                    "^Attribute~ACCOUNT_NUMBER~STR~" +
                    "^Attribute~BATCH_NUM~STR~" + BatchNumber +
                    "^Attribute~DOC_DATE~STR~" + doc.DocDate +
                    "^Attribute~DOC_ID~STR~" + doc.DocID +
                    "^Attribute~DOC_TIME~STR~" +
                    "^Attribute~LENDER_CODE~STR~"
                );
                foreach (string s in tiffNames)
                    lines.Add(
                        @"ImageFile~\\Imgprodkofax\ascent$\UTCROther_imp\" + s
                    );
            }
            else
            {
                lines.Add(
                    "~^Folder~" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + ", Doc 1" +
                    "^Type~UTCR_TYPE" +
                    "^Attribute~SSN~STR~" + doc.SSN +
                    "^Attribute~LENDER_CODE~STR~" +
                    "^Attribute~ACCOUNT_NUM~STR~" +
                    "^Attribute~DOC_ID~STR~" + doc.DocID +
                    "^Attribute~DOC_DATE~STR~" + doc.DocDate +
                    "^Attribute~BATCH_NUM~STR~" + BatchNumber +
                    "^Attribute~VENDOR_NUM~STR~" +
                    "^Attribute~SCAN_DATE~STR~" +
                    "^Attribute~SCAN_TIME~STR~" +
                    "^Attribute~DESCRIPTION~STR~" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")
                );
                lines.Add(
                    @"DesktopDoc~\\Imgprodkofax\ascent$\UTCROther_imp\" + doc.ImagePath + "~" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + ", Doc 1" +
                    "^Type~UTCR_TYPE" +
                    "^Attribute~SSN~STR~" + doc.SSN +
                    "^Attribute~LENDER_CODE~STR~" +
                    "^Attribute~ACCOUNT_NUM~STR~" +
                    "^Attribute~DOC_ID~STR~" + doc.DocID +
                    "^Attribute~DOC_DATE~STR~" + doc.DocDate +
                    "^Attribute~BATCH_NUM~STR~" + BatchNumber +
                    "^Attribute~VENDOR_NUM~STR~" +
                    "^Attribute~SCAN_DATE~STR~" +
                    "^Attribute~SCAN_TIME~STR~"
                );
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (string line in lines)
                    sw.WriteLine(line);
                return true;
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            foreach (ThreadedDocumentProcessor t in this.threads)
                t.Dispose();
        }

        #endregion
    }
}
