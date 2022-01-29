using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PdfToText;
using Ionic.Zip;

namespace ImagingTransferFileBuilder
{
    public static class PdfScraper
    {
        public static void Scrape(string pdfLocation, string resultLocation, string excelLocation, string sheetName, string docDate, string saleDate, string dealID, string loanProgramType)
        {
            Results.Clear();
            Progress.Start("PDF Scraper");
            Dictionary<string, Borrower> borrowers = ExcelHelper.GetBorrowers(excelLocation, sheetName, false);
            Progress.Increments = borrowers.Count;
            string code = Util.Code(dealID, DateTime.Parse(saleDate));
            using (ZipFile zf = new ZipFile())
            {
                List<string> indexLines = new List<string>();
                foreach (Borrower b in borrowers.Values)
                {
                    string line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", b.SSN, b.LastName, b.FirstName, "{0}", b.LoanID, docDate, loanProgramType, b.GuarantyDate, saleDate, dealID, "{1}")  ;
                    foreach (string file in Directory.GetFiles(pdfLocation, b.SSN + "*", SearchOption.TopDirectoryOnly))
                    {
                        PDFParser parser = new PDFParser();
                        string result = parser.ExtractText(file);
                        string doctype;
                        if (result.StartsWith("Address/Phone/Email History"))
                        {
                            doctype = "srvh";
                        }
                        else if (result.StartsWith("Payment History"))
                        {
                            doctype = "payh";
                        }
                        else
                            continue;
                        string newLine = string.Format(line, doctype, Path.GetFileName(file));
                        zf.AddFile(file, code);
                        indexLines.Add(newLine);
                    }
                    Progress.Increment();
                }
                zf.AddEntry(Path.Combine(code, code + ".IDX"), string.Join(Environment.NewLine, indexLines.ToArray()));
                zf.Save(Path.Combine(resultLocation, code + ".ZIP"));
            }
            
            Progress.Finish();
        }
    }
}
