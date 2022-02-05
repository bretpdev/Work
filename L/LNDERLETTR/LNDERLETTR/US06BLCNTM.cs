using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml.xmp;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.DataAccess;
using Uheaa.Common;


namespace LNDERLETTR
{
    public static class US06BLCNTM
    {
        private static iTextSharp.text.Font StateMailBarcodeFont { get; set; }

        private static string LetterId { get; } = @"US06BLCNTM";

        static US06BLCNTM()
        {
            BaseFont customfont = BaseFont.CreateFont(@"C:\Windows\Fonts\" + "IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new iTextSharp.text.Font(customfont, 8);
        }


        public static string GenerateUS06BLCNTM(List<string> lenderAddress, List<US06BLCNTMBwrAddr> bwrs, DataAccess da)
        {
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath(LetterId), "US06BLCNTM.pdf");
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToBase64String() + ".pdf");
            using (PdfReader pdfReader = new PdfReader(templatePath))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);

                    // Put a DateStamp on the letter
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString()), 71, 675, 0);//71 is the starting width

                    // ACS KeyLine.
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase("#BWNHDGF"), 71, 652, 0);//71 is the starting width

                    //This is the starting Height, (envelope window)
                    int linePosition = 640;
                    foreach (string line in lenderAddress)
                    {
                        if (line.IsPopulated())
                        {
                            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(line), 71, linePosition, 0);//71 is the starting width
                            linePosition -= 13;//Move down a line
                        }
                    }
                }
            }

            List<string> files = new List<string>();
            files.Add(newFile);
            int skip = 0;
            int take = 3;
            while (skip < bwrs.Count)
            {
                files.Add(CreateUS06BLCNTMForm(bwrs.Skip(skip).Take(take).ToList()));
                skip += take;
            }


            string returnFile = MergePdfs(files, LetterId, "", false, null, Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToBase64String() + ".pdf"));

            foreach (string file in files)
                Repeater.TryRepeatedly(() => File.Delete(file));

            int duplexPages = DuplexPages(returnFile);
            List<string> stateMailBarcodes = DocumentProcessing.GetStateMailBarcodesforPdf(LetterId, duplexPages, DocumentProcessing.LetterRecipient.Other);

            PrintStateMailCoverSheet(duplexPages, false, LetterId, da);

            returnFile = AddBarcodesToPdf(returnFile, "US06BLCNTM_", false, duplexPages, da, stateMailBarcodes);

            return returnFile;
        }

        public static void PrintStateMailCoverSheet(int numberOfPages, bool isForeign, string letterId, DataAccess da)
        {
            //TODO: Cover sheet counts look like they can only be 1 or 0.  Is this intended
            CoverData cover = new CoverData();
            cover = da.GetCoverSheetData(LetterId);
            string coverSheetFolder = EnterpriseFileSystem.GetPath("CoverSheet");
            string coverSheetDataFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}.txt", Guid.NewGuid().ToBase64String()));

            string StateBusinessUnit = cover.BusUnit;
            string StateCostCenter = cover.CostCenter;

            int domesticCount = 0;
            int foreignCount = 0;
            if (isForeign)
                foreignCount++;
            else
                domesticCount++;
            string coverSheetInstructions = da.GetCostCenterInstructions(letterId);

            using (StreamWriter sw = new StreamWriter(coverSheetDataFile))
            {
                sw.WriteCommaDelimitedLine("BU", "Description", "NumPages", "Cost", "Standard", "Foreign", "CoverComment");
                sw.WriteCommaDelimitedLine(StateBusinessUnit, LetterId, numberOfPages.ToString(), StateCostCenter, domesticCount.ToString(), foreignCount.ToString(), coverSheetInstructions);
            }

            DocumentProcessing.PrintDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetDataFile);
            //if (coverSheetInstructions.IsNullOrEmpty())
            //{
            //    DataAccess.AddFederalCostCenterPrintingRecord(letterId, foreignCount, domesticCount, FederalCostCenter);
            //}

            Repeater.TryRepeatedly(() => File.Delete(coverSheetDataFile));
        }


        private static int DuplexPages(string file)
        {
            int duplexPages = 0;

            using (PdfReader pdfReader = new PdfReader(file))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                duplexPages = numberOfPages / 2;
                if (numberOfPages % 2 > 0)
                    duplexPages++;
            }
            return duplexPages;
        }


        private static PdfContentByte AddBarCodesToPage1(List<string> barCodes, PdfStamper pdfStamper)
        {
            
            PdfContentByte pdfContent = pdfStamper.GetOverContent(1);//Adding state mail barcode to the first page.

            float returnMailYCoord = 755f;
            int limit = (barCodes.Count > 36) ? 5 : 4;
            for (int mailIndex = 0; mailIndex < limit; mailIndex++)
            {
                ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(barCodes[mailIndex], StateMailBarcodeFont), 15, returnMailYCoord, 0);
                returnMailYCoord -= (float)8.05f;
            }
            //if (barCodes.Count % 5 == 0)
            //{
            //    returnMailYCoord -= (float)8.05f;
            //    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(barCodes[4], StateMailBarcodeFont), 15, returnMailYCoord, 0);
            //}
            return pdfContent;
        }

        private static string AddBarcodesToPdf(string file, string letterId, bool isForeign, int duplexPages, DataAccess da, List<string> stateMailBarcodes)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, letterId + $"{ Guid.NewGuid().ToBase64String()}.pdf");
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(file))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                string rotatedFile = "";
                rotatedFile = RotateDocument(file);

                string barCodes = string.Empty;

                using (PdfReader rotatedReader = new PdfReader(rotatedFile))
                {
                    //This will create the new PDF
                    using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                    {
                        AddBarCodesToPage1(stateMailBarcodes, pdfStamper);
                        int index = 0;

                        for (int page = 3; page <= numberOfPages; page += 2)
                        {
                            //float linePosition = page == 1 ? 755f : 575f;
                            //float horizontalPosition = page == 1 ? 15f : 690f;
                            //float linePosition = page == 1 ? 755f : 600f;
                            //float horizontalPosition = page == 1 ? 15f : 750f;
                            float linePosition = page == 1 ? 755f : 600f;
                            float horizontalPosition = page == 1 ? 15f : 750f;
                            PdfContentByte pdfContent = pdfStamper.GetOverContent(page);
                            barCodes += stateMailBarcodes[index];
                            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 270, 0, 0);
                            horizontalPosition -= (float)8.05;
                            barCodes += stateMailBarcodes[index];
                            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 270, 0, 0);
                            horizontalPosition -= (float)8.05;
                            barCodes += stateMailBarcodes[index];
                            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 270, 0, 0);
                            horizontalPosition -= (float)8.05;
                            barCodes += stateMailBarcodes[index];
                            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 270, 0, 0);
                            horizontalPosition -= (float)8.05;
                            if (stateMailBarcodes.Count % 5 == 0)
                            {
                                barCodes += stateMailBarcodes[index];
                                ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 270, 0, 0);
                                horizontalPosition -= (float)8.05;
                            }
                        }
                    }
                }
            }

            Repeater.TryRepeatedly(() => File.Delete(file));
            return newFile;
        }


        public static string CreateUS06BLCNTMForm(List<US06BLCNTMBwrAddr> dts)
        {
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("US06BLCNTM"), "US06BLCNTM_Form.pdf");
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToBase64String() + ".pdf");
            using (PdfReader pdfReader = new PdfReader(templatePath))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);

                    //This is the starting Height
                    float last4 = 568f; //559f;
                    float address = 516f; //505f;
                    float phone = 467f; //459.5f;
                    float name = 568f;//559f;
                    float dy = 0;

                    Font f = new Font(Font.FontFamily.HELVETICA, 9);

                    foreach (US06BLCNTMBwrAddr dt in dts)
                    {

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Name, f), 320, name, 0);//71 is the starting width
                        name -= 11f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.PrevName, f), 320, name, 0);//71 is the starting width

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.SsnLastFour, f), 168, last4, 0);//71 is the starting width

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Address1, f), 76, address, 0);//71 is the starting width
                        address -= 11f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Address2, f), 76, address, 0);//71 is the starting width
                        address -= 11f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(string.Format("{0} {1}, {2}", dt.City, dt.State, dt.Zip), f), 76, address, 0);//71 is the starting width
                        address -= 11f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Province + dt.Country, f), 76, address, 0);//71 is the starting width

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Phone, f), 100, phone, 0);//71 is the starting width
                        phone -= 15f; //18f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.AltPhone1, f), 100, phone, 0);//71 is the starting width
                        phone -= 15f; //18f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.AltPhone2, f), 100, phone, 0);//71 is the starting width
                        phone -= 16f; //18f;

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.EmailAddress, f), 66, phone, 0);//71 is the starting width

                        phone -= 143;
                        //address -= 168;
                        address -= 157;
                        name -= 180;
                        last4 -= 189f;
                        dy += 1;
                    }
                    pdfStamper.Close();
                }
                pdfReader.Close();
            }
            return newFile;
        }

        public static void PrintPdf(string file)
        {
            Process proc = new Process();
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Verb = "print";
            proc.StartInfo.RedirectStandardOutput = true;


            //Define location of adobe reader/command line
            //switches to launch adobe in "print" mode
            if (Environment.UserName == "slegge")
                proc.StartInfo.FileName = "C:\\Program Files (x86)\\Adobe\\Acrobat 2015\\Acrobat\\Acrobat.exe";
            else
                proc.StartInfo.FileName = EnterpriseFileSystem.GetPath("AcrobatReader");

            if (!File.Exists(proc.StartInfo.FileName))
            {
                double version = proc.StartInfo.FileName.SplitAndRemoveQuotes("\\")[3].Replace("Acrobat ", "").ToDouble();
                proc.StartInfo.FileName = proc.StartInfo.FileName.Replace(version.ToString(), (++version).ToString());
            }
            proc.StartInfo.Arguments = String.Format(@"/T /N /H /P {0}", file);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();

            int id = proc.Id;

            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            proc.EnableRaisingEvents = true;


            proc.WaitForInputIdle();

            proc.Close();

            Repeater.TryRepeatedly(() => KillProcessById(id));
        }
        private static bool KillProcessById(int id)
        {
            foreach (Process clsProcess in Process.GetProcesses().Where(clsProcess => clsProcess.Id == id))
            {
                clsProcess.Kill();
                return true;
            }
            return false;
        }

        private static string CalculateFileName(string letterId, string path = null)
        {
            if (path == null)
                path = EnterpriseFileSystem.TempFolder;

            return string.Format("{0}{1}{2}.pdf", path, letterId, Guid.NewGuid());
        }


        public static string MergePdfs(List<string> files, string letterId, string accountNumber, bool tagPdf, DataAccessHelper.Region? region = null, string saveAs = null)
        {
            //UNDONE this will be needed for 508 but they are not ready for it yet.  Do not delete
            string outputFile = CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("BorrowerServices", region)));
            string mergeFiles = saveAs == null ? CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("BorrowerServices", region)))
                : saveAs;
            //string outputFile = originalOutput;
            string templateFile = Path.Combine(EnterpriseFileSystem.GetPath("BorrowerServices"), letterId + ".pdf");
            PdfReader reader = new PdfReader(templateFile);
            string auth = "";
            string sub = "";
            string title = "";
            string keywords = "";
            try
            {
                auth = reader.Info["Author"];
                sub = reader.Info["Subject"];
                title = reader.Info["Title"];
                keywords = reader.Info["Keywords"];
            }
            catch
            {
                //do nothing some docs do not have these properties
            }



            var t = reader;
            List<PdfReader> readers = new List<PdfReader>();
            using (FileStream outputPdfStream = new FileStream(mergeFiles, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document document = new Document(PageSize.A4))
                {

                    using (PdfCopy copy = new PdfCopy(document, outputPdfStream))
                    {
                        if (tagPdf)
                        {
                            copy.SetTagged();
                            copy.SetLanguage("English");

                            copy.SetFullCompression();
                        }
                        document.Open();

                        foreach (string file in files)
                        {
                            PdfReader r = new PdfReader(file);
                            readers.Add(r);
                            copy.AddDocument(r);
                        }
                    }

                    document.AddAuthor(auth);
                    document.AddTitle(title);
                    document.AddSubject(sub);
                    document.AddKeywords(keywords);

                    document.Close();
                    document.Dispose();
                }
                outputPdfStream.Close();
                outputPdfStream.Dispose();
            }

            foreach (PdfReader r in readers)
                r.Dispose();

            string fileToDelete = outputFile;
            using (var reader1 = new PdfReader(mergeFiles))
            {
                using (var stamper = new PdfStamper(reader1, new FileStream(outputFile, FileMode.Create)))
                {
                    var info = reader.Info;
                    info["Author"] = auth;
                    info["Title"] = title;
                    info["Subject"] = sub;
                    info["Keywords"] = keywords;
                    stamper.AddViewerPreference(PdfName.DISPLAYDOCTITLE, new PdfBoolean(true));
                    stamper.MoreInfo = info;

                    using (var ms = new MemoryStream())
                    {
                        var xmp = new XmpWriter(ms, info);
                        stamper.XmpMetadata = ms.ToArray();
                        xmp.Close();
                    }
                }
            }

            File.Delete(mergeFiles);

            return outputFile;
        }

        private static string RotateDocument(string file)
        {
            PdfReader reader = new PdfReader(file);
            int n = reader.NumberOfPages;
            PdfDictionary page;
            PdfNumber rotate;
            for (int p = 2; p <= n; p++)
            {
                page = reader.GetPageN(p);
                rotate = page.GetAsNumber(PdfName.ROTATE);
                if (rotate == null)
                    page.Put(PdfName.ROTATE, new PdfNumber(180));
                else
                    page.Put(PdfName.ROTATE, new PdfNumber((rotate.IntValue + 270) % 360));
                    //page.Put(PdfName.ROTATE, new PdfNumber((rotate.IntValue + 270) % 360));
            }

            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}.pdf", Guid.NewGuid().ToBase64String()));
            PdfStamper stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create));
            stamper.Close();
            reader.Close();
            return newFile;
        }

    }
}
