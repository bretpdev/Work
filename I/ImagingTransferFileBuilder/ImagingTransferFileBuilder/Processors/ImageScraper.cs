using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Xml.XPath;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using System.Windows.Forms;

namespace ImagingTransferFileBuilder
{
    public class InvalidCredentialsException : Exception { }
    public static class ImageScraper
    {
        public static void Scrape(string excelLocation, string sheetName, string resultsLocation, string userName, string password, string loanProgramType, DateTime? after)
        {
            Progress.Start("Image Scraper");
            Cookies = new CookieContainer();
            string url = @"http://imguheaaprodiis:7256/catalog%20UHEAA/CatalogSearch.aspx";
            string query = "ctl00$ctl00$WebPartManager1$Global360ApplicationManagerPart$textBoxUserName={0}" +
                "&ctl00$ctl00$WebPartManager1$Global360ApplicationManagerPart$textBoxPassword={1}" +
                "&ctl00$ctl00$WebPartManager1$Global360ApplicationManagerPart$buttonLogin=Login" +
                "&__VIEWSTATE=";
            query = string.Format(query, userName, password);
            //login, we keep track of cookies so this will persist
            string response = Encoding.UTF8.GetString(PostPage(url, query));
            if (response.Contains(">Login<")) //if login link is still there, invalid credentials
            {
                Progress.Failure();
                throw new InvalidCredentialsException();
            }

            List<Borrower> borrowers = ExcelHelper.GetBorrowers(excelLocation, sheetName, false).Values.ToList();
            Progress.Increments = borrowers.Count;
            foreach (Borrower b in borrowers)
            {
                string borrowerDirectory = Path.Combine(resultsLocation, b.FirstName + " " + b.LastName);
                string line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", b.SSN, b.LastName, b.FirstName, "{0}", b.LoanID, "{1}", loanProgramType, b.GuarantyDate, DateTime.Now.ToString("MM/dd/yyyy"), "U0000", "{2}");
                List<Img> images = GetImages(b.SSN, after);
                List<string> indexLines = new List<string>();
                if (!Directory.Exists(borrowerDirectory))
                {
                    Results.LogError("Couldn't find directory {0}.  Directory was created.", borrowerDirectory);
                    Directory.CreateDirectory(borrowerDirectory);
                }
                foreach (Img img in images)
                {
                    string fileName = ProcessImg(img, borrowerDirectory);
                    Results.LogNotification(string.Format("Scraped image for borrower {0} to {1}.", b.SSN, Path.Combine(borrowerDirectory, fileName)));
                    indexLines.Add(string.Format(line, img.DocType, img.DocDate, fileName));
                    Application.DoEvents();
                }

                var indexes = Directory.GetFiles(borrowerDirectory, "*.IDX", SearchOption.TopDirectoryOnly);
                string indexPath = "";
                if (indexes.Length > 1)
                {
                    indexPath = indexes.First();
                    Results.LogError("Found multiple index files at {0}.  Using file {1}.", borrowerDirectory, indexPath);
                }
                else if (indexes.Length == 0)
                {
                    indexPath = Util.Code() + ".IDX";
                    File.CreateText(Path.Combine(borrowerDirectory, indexPath)).Close();
                    Results.LogError("Found no index files at {0}.  Created file {1}", borrowerDirectory, indexPath);
                }
                else
                    indexPath = indexes.First();
                indexPath = Path.Combine(borrowerDirectory, indexPath);
                List<string> originalLines = File.ReadAllLines(indexPath).ToList();
                List<string> totalLines = originalLines.Union(indexLines).ToList();
                File.WriteAllLines(indexPath, totalLines.ToArray());
                Progress.Increment();
            }

            Cookies = null;
            Progress.Finish();
        }
        public class Img
        {
            public enum ImgType
            {
                Image,
                Desktop
            }
            public string Width { get; set; }
            public string Height { get; set; }
            public string DocID { get; set; }
            public int PageCount { get; set; }
            public ImgType Type { get; set; }
            public string Ext { get; set; }
            public string DocDate { get; set; }
            public string DocType { get; set; }
        }
        public static string ProcessImg(Img img, string folder)
        {
            string fileName = Guid.NewGuid().ToString() + ".";
            if (img.Ext != null)
                fileName += img.Ext.ToLower();
            else
                fileName += "pdf";
            string url = "http://imguheaaprodiis:7256/catalog%20UHEAA/DocumentViewHandler.ashx";
            using (FileStream fs = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
            {
                if (img.Type == Img.ImgType.Image)
                {
                    Document d = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(d, fs);
                    d.Open();
                    for (int i = 0; i < img.PageCount; i++)
                    {
                        string query = string.Format("req=IMAGEZOOMED&page={3}&connection=V2ViU2VydmljZTtodHRwOi8vaW1ndWhlYWFwcm9kd3MvUHJvY2VzczM2MFdlYlNlcnZpY2UvVmlld1N0YXJTZXJ2aWNlLmFzbXg=&VSWS_DocId={0}&height={1}&width={2}&rot=0",
                            img.DocID, img.Height, img.Width, i);
                        Image image = Image.GetInstance(GetPage(url + "?" + query));
                        image.ScaleToFit(d.PageSize.Width - d.LeftMargin * 2, d.PageSize.Height - d.TopMargin * 2);
                        d.Add(image);
                        Application.DoEvents();
                    }
                    d.Close();
                }
                else if (img.Type == Img.ImgType.Desktop)
                {
                    string query = string.Format("REQ=DESKTOP&VSWS_WiId=AA51B9DE-073B-49C3-847A-84C65C8AF2D1&connection=V2ViU2VydmljZTtodHRwOi8vaW1ndWhlYWFwcm9kd3MvUHJvY2VzczM2MFdlYlNlcnZpY2UvVmlld1N0YXJTZXJ2aWNlLmFzbXg=&VSWS_DocId={0}&ext=PDF",
                        img.DocID);
                    byte[] bytes = GetPage(url + "?" + query);
                    fs.Write(bytes, 0, (int)bytes.Length);
                }
            }
            return fileName;
        }
        public static Img GetImageSpecs(string docid)
        {
            return GetImageSpecs(docid, true);
        }
        public static Img GetImageSpecs(string docid, bool retryOnError)
        {
            //get the document params
            string url = "http://imguheaaprodiis:7256/catalog%20UHEAA/DocumentViewHandler.ashx";
            string query = string.Format("req=docinfo&VSWS_WiId=968B92F5-AF31-418D-AB1D-24C258BB53EE&connection=V2ViU2VydmljZTtodHRwOi8vaW1ndWhlYWFwcm9kd3MvUHJvY2VzczM2MFdlYlNlcnZpY2UvVmlld1N0YXJTZXJ2aWNlLmFzbXg=&VSWS_DocId={0}&resx=96&resy=96&fld=0&_=1350508847242",
                docid);
            Img img = new Img();
            img.DocID = docid;
            XPathNavigator nav = GetNav(GetPage(url + "?" + query));
            switch (nav.SelectSingleNode("/*").Name)
            {
                case "ImageDocument":
                    img.Type = Img.ImgType.Image;
                    img.Width = nav.SelectSingleNode("//Width").Value;
                    img.Height = nav.SelectSingleNode("//Height").Value;
                    img.PageCount = int.Parse(nav.SelectSingleNode("//PageCount").Value);
                    break;
                case "DesktopDocument":
                    img.Type = Img.ImgType.Desktop;
                    nav.Select("DocumentExtension");
                    img.Ext = nav.SelectSingleNode("//DocumentExtension").Value;
                    break;
                default:
                    if (retryOnError)
                        return GetImageSpecs(docid, false);
                    else
                        Results.LogError("Error downloading {0} twice.  Skipping.", url + "?" + query);
                    break;
            }
            return img;
        }
        public static List<Img> GetImages(string ssn, DateTime? after)
        {
            List<Img> images = new List<Img>();
            //this is pretty messy stuff, tell the page we want federals with this ssn and get all xml results
            string url = @"http://imguheaaprodiis:7256/catalog%20UHEAA/CatalogSearch.aspx";
            string query = "__CALLBACKID=ctl00$ctl00$WebPartManager1$wp389119853" + "&__VIEWSTATE=" +
                "&__CALLBACKPARAM=" + HttpUtility.UrlEncode(string.Format("__rsControlId=ctl00_ctl00_WebPartManager1_wp389119853&__rsRemoteScriptPanelId=ctl00_ctl00_WebPartManager1_wp389119853_ctl00&__rsPropertyPrefix=__rs&__rsServiceType=WebService&__rsSystemPath=http%3A%2F%2Fimguheaaprodws%2FProcess360WebService%2FViewStarService.asmx&__rsAction=CTSearch_Search&__rsContext=%5Bobject%20Object%5D&__rsCatalogName=UHEAA%2FUHEAA_FEDERAL&__rsSS_ctl00_ctl00_WebPartManager1_wp389119853_SSN={0}&__rsLibraryName=VIEWSTAR&__rsPageNumber=1&__rsPageSize=2000&__rsShowPaging=True&__rsWorkItemId=016B04CD-65E1-498F-B8E3-87B6FFE12AEB",
                ssn));
            XPathNavigator nav = GetNav(PostPage(url, query));
            XPathExpression expr = nav.Compile("//Row[Field[@Name=\"SCAN_DATE\" and text()!=\"(null)\"]]");
            XPathNodeIterator iterator = nav.Select(expr);
            while (iterator.MoveNext())
            {
                string docId = iterator.Current.GetAttribute("RowId", nav.NamespaceURI);
                Img img = GetImageSpecs(docId);
                Application.DoEvents();
                string scanDate = iterator.Current.CreateNavigator().SelectSingleNode("Field[@Name=\"SCAN_DATE\"]").Value;
                string docDate = iterator.Current.CreateNavigator().SelectSingleNode("Field[@Name=\"DOC_DATE\"]").Value;
                string docType = iterator.Current.CreateNavigator().SelectSingleNode("Field[@Name=\"DOC_TYPE\"]").Value.ToLower();
                string doc_id = iterator.Current.CreateNavigator().SelectSingleNode("Field[@Name=\"DOC_ID\"]").Value.ToUpper();
                string processedDate = iterator.Current.CreateNavigator().SelectSingleNode("Field[@Name=\"PROCESSED_DATE\"]").Value.ToUpper();
                if (docDate.ToLower() == "(null)") docDate = null;
                if (scanDate.ToLower() == "(null)") scanDate = null;
                if (processedDate.ToLower() == "(null)") processedDate = null;
                DateTime whereDate = DateTime.Parse(processedDate ?? docDate ?? scanDate);
                if (after.HasValue)
                    if (whereDate.Date <= after.Value.Date)
                        continue;

                img.DocDate = DateTime.Parse(docDate ?? scanDate).ToString("MM/dd/yyyy");

                if (docType.ToLower() == "(null)" || string.IsNullOrEmpty(docType))
                {
                    if (DocTypes.ContainsKey(doc_id))
                        docType = DocTypes[doc_id];
                    else
                    {
                        Results.LogError("Couldn't resolve DOCID {0} for SSN {1}.  Skipping entry.", doc_id, ssn);
                        continue;
                    }
                }
                img.DocType = docType;
                images.Add(img);
            }
            return images;
        }
        public static CookieContainer Cookies { get; set; }
        public static XPathNavigator GetNav(byte[] bytes)
        {
            string s = Encoding.UTF8.GetString(bytes).Trim(); //remove BOM if present
            if (s.StartsWith("s"))
                s = s.Substring(1); //remove first char 's'
            s = s.Replace("xmlns=\"http://www.viewstar.com/webservices/2002/11\"", ""); //this namespace screws xpath up
            StringReader reader = new StringReader(s);
            var doc = new XPathDocument(reader);
            var navigator = doc.CreateNavigator();
            return navigator;
        }

        public static byte[] GetPage(string urlAndQuery)
        {
            return Page(urlAndQuery, null, "GET");
        }
        public static byte[] PostPage(string url, string query)
        {
            return Page(url, query, "POST");
        }
        private static byte[] Page(string url, string query, string requestMethod)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = Cookies;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = requestMethod;
            if (requestMethod == "POST")
            {
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = Encoding.UTF8.GetBytes(query);
                request.ContentLength = bytes.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(bytes, 0, bytes.Length);
                }
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buffer = new byte[1024];
                            count = stream.Read(buffer, 0, 1024);
                            ms.Write(buffer, 0, count);
                        } while (stream.CanRead && count > 0);
                        return ms.ToArray();
                    }
                }
            }
        }

        private static Dictionary<string, string> DocTypes = new Dictionary<string, string>();
        public static void RebuildCache()
        {
            foreach (DocType dt in DocType.GetAll())
                foreach (DocId di in DocId.GetByDocType(dt))
                    DocTypes[di.DocIdValue] = dt.DocTypeValue;
        }
        static ImageScraper()
        {
            RebuildCache();
        }
    }
}
