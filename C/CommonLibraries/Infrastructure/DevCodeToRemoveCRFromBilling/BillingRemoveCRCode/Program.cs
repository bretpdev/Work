using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace BillingRemoveCRCode
{
    class Program
    {
        private static iTextSharp.text.Font StateMailBarcodeFont { get; set; }
        private static iTextSharp.text.Font ReturnMailBarcodeFont { get; set; }
        private static iTextSharp.text.Font Arial { get; set; }
        private static iTextSharp.text.Font ArialLoan { get; set; }
        private static iTextSharp.text.Font ScanLine { get; set; }
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            Console.WriteLine("Test");
            BaseFont customfont = BaseFont.CreateFont(@"C:\Windows\Fonts\" + "IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont customfontScanLine = BaseFont.CreateFont(@"C:\Windows\Fonts\" + "OCRAEXT.TTF", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new iTextSharp.text.Font(customfont, 8);
            ReturnMailBarcodeFont = new iTextSharp.text.Font(customfont, 4);
            Arial = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 8);
            ArialLoan = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 7);
            ReturnMailBarcodeFont = new iTextSharp.text.Font(customfont, 4);
            ScanLine = new iTextSharp.text.Font(customfontScanLine, 12);

            new Program().AddResturnMailBC();

        }

        private List<BillData> GetData()
        {
            List<BillData> data = new List<BillData>();

            data.Add(new BillData() { XCoord = 502f, YCoord = 729.5f, Value = DateTime.Now.ToString("MM/dd/yyyy"), Font = BillData.FontType.Arial });//Statement Date
            data.AddRange(BillData.GetData());
            return data;
        }

        private List<Loans> GetLoans()
        {
            List<Loans> data = new List<Loans>();
            data.Add(new Loans() { LoanType = "Direct Subsidized Consolidation Loan", Details = GetLoanDetail() });
            data.Add(new Loans() { LoanType = "Direct Unsubsidized Consolidation Loan", Details = GetLoanDetail() });
            data.Add(new Loans() { LoanType = "Direct Unsubsidized Consolidation Loan", Details = GetLoanDetail() });

            return data;
        }

        private List<LoanDetail> GetLoanDetail()
        {
            List<LoanDetail> data = new List<LoanDetail>();

            data.Add(new LoanDetail() { DateFirstDisb = "08/25/2015", InterestRate = "4.500%", OrigPrincipal = "$15,000.00", TotalAmtPaidToPri = "$1,000.00", TotalAmtPaidToInt = "$500.00", TotalAmtPaid = "$1,500.00", CurrentBalance = "$14,000.00" });
            data.Add(new LoanDetail() { DateFirstDisb = "08/26/2015", InterestRate = "4.600%", OrigPrincipal = "$16,000.00", TotalAmtPaidToPri = "$2,000.00", TotalAmtPaidToInt = "$500.00", TotalAmtPaid = "$2,500.00", CurrentBalance = "$14,000.00" });
            data.Add(new LoanDetail() { DateFirstDisb = "08/25/2015", InterestRate = "4.500%", OrigPrincipal = "$15,000.00", TotalAmtPaidToPri = "$1,000.00", TotalAmtPaidToInt = "$500.00", TotalAmtPaid = "$1,500.00", CurrentBalance = "$14,000.00" });
            data.Add(new LoanDetail() { DateFirstDisb = "08/26/2015", InterestRate = "4.600%", OrigPrincipal = "$16,000.00", TotalAmtPaidToPri = "$2,000.00", TotalAmtPaidToInt = "$500.00", TotalAmtPaid = "$2,500.00", CurrentBalance = "$14,000.00" });
            data.Add(new LoanDetail() { DateFirstDisb = "08/25/2015", InterestRate = "4.500%", OrigPrincipal = "$15,000.00", TotalAmtPaidToPri = "$1,000.00", TotalAmtPaidToInt = "$500.00", TotalAmtPaid = "$1,500.00", CurrentBalance = "$14,000.00" });
            data.Add(new LoanDetail() { DateFirstDisb = "08/25/2015", InterestRate = "4.500%", OrigPrincipal = "$15,000.00", TotalAmtPaidToPri = "$1,000.00", TotalAmtPaidToInt = "$500.00", TotalAmtPaid = "$1,500.00", CurrentBalance = "$14,000.00" });
            data.Add(new LoanDetail() { DateFirstDisb = "08/25/2015", InterestRate = "4.500%", OrigPrincipal = "$15,000.00", TotalAmtPaidToPri = "$1,000.00", TotalAmtPaidToInt = "$500.00", TotalAmtPaid = "$1,500.00", CurrentBalance = "$14,000.00" });

            return data;
        }

        public void AddResturnMailBC()
        {

            using (PdfReader pdfReader = new PdfReader(@"T:\NewBill\TemplateCSBILL.pdf"))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                int duplexPages = numberOfPages / 2;
                if (duplexPages % 2 == 0)
                    duplexPages++;

                duplexPages++;

                List<string> returnMailBarcodes = DocumentProcessing.GetReturnMailBarcodes("0123456789", "BILSTFED", DocumentProcessing.LetterRecipient.Borrower);
                List<string> stateMailBarcodes = DocumentProcessing.GetStateMailBarcodesforPdf("0123456789", 1, DocumentProcessing.LetterRecipient.Borrower);

                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(string.Format(@"T:\NewBill\TemplateCSBILL_Test_{0}.pdf", Guid.NewGuid()), FileMode.Create)))
                {
                    PdfContentByte pdfContent = AddBarcodes(returnMailBarcodes, stateMailBarcodes, pdfStamper);

                    foreach (BillData item in GetData())
                    {
                        iTextSharp.text.Font font = null;
                        switch (item.Font)
                        {
                            case BillData.FontType.Arial:
                                font = Arial;
                                break;
                            case BillData.FontType.ScanLine:
                                font = ScanLine;
                                break;
                            case BillData.FontType.ArialLoan:
                                font = ArialLoan;
                                break;
                        }

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(item.Value.ToString(), font), item.XCoord, item.YCoord, 0);
                    }
                    //string text = string.Format("Line1 {0} line2 {0} line3", Environment.NewLine);
                    //List<string> item = text.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    //float linepos = 350f;
                    //foreach (string l in item)
                    //{
                    //    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(l, Arial), 300, linepos, 0);
                    //    linepos += 10;
                    //}

                    
                    ColumnText ct = new ColumnText(pdfContent);
                    ct.Alignment = Element.ALIGN_LEFT;
                    ct.SetSimpleColumn(70, 36, PageSize.A4.Width - 36, PageSize.A4.Height - 300);
                    float startLoan = 652f;
                    List<Loans> loans = GetLoans();
                    foreach (Loans loan in loans)
                    {

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(loan.LoanType, Arial), 30, startLoan, 0);

                        PdfPTable table = new PdfPTable(7);
                        float[] wid = new List<float>() { 55f, 35f, 55f, 65f, 60f, 50f, 48f }.ToArray();
                        table.SetTotalWidth(wid);

                        table.AddCell(GetCell("Date First Disbursed", 1, 2, true));
                        table.AddCell(GetCell("Interest Rate", 1, 2, true));
                        table.AddCell(GetCell("Original Principal Amount", 1, 2, true));
                        table.AddCell(GetCell("Total Amount Paid to Principal", 1, 2, true));
                        table.AddCell(GetCell("Total Amount Paid to Interest", 1, 2, true));
                        table.AddCell(GetCell("Total Aggregate Amount Paid", 1, 2, true));
                        table.AddCell(GetCell("Current Balance", 1, 2, true));

                        foreach (LoanDetail item in loan.Details)
                        {
                            table.AddCell(GetCell(item.DateFirstDisb));
                            table.AddCell(GetCell(item.InterestRate, 2));
                            table.AddCell(GetCell(item.OrigPrincipal, 2));
                            table.AddCell(GetCell(item.TotalAmtPaidToPri, 2));
                            table.AddCell(GetCell(item.TotalAmtPaidToInt, 2));
                            table.AddCell(GetCell(item.TotalAmtPaid, 2));
                            table.AddCell(GetCell(item.CurrentBalance, 2));
                        }

                        
                        table.WriteSelectedRows(0, 50, 30, startLoan - 2f, pdfContent);
                        startLoan = startLoan - 2f;
                        startLoan = (startLoan - 50f - ((11f - loans.Count ) * loan.Details.Count));
                    }

                    float startTotals = startLoan - 2f;


                    PdfPTable sumInfo = new PdfPTable(5);
                    float[] wids = new List<float>() { 55f, 65f, 60f, 50f, 48f }.ToArray();
                    sumInfo.SetTotalWidth(wids);

                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase("These represent the grand totals in these categories.", Arial), 30, startTotals, 0);

                    sumInfo.AddCell(GetCell("Original Principal Amount", 1, 2, true));
                    sumInfo.AddCell(GetCell("Total Amount Paid to Principal", 1, 2, true));
                    sumInfo.AddCell(GetCell("Total Amount Paid to Interest", 1, 2, true));
                    sumInfo.AddCell(GetCell("Total Aggregate Amount Paid", 1, 2, true));
                    sumInfo.AddCell(GetCell("Current Balance", 1, 2, true));
                    sumInfo.AddCell(GetCell("$15,000.00", 2));
                    sumInfo.AddCell(GetCell("$15,000.00", 2));
                    sumInfo.AddCell(GetCell("$1,000.00", 2));
                    sumInfo.AddCell(GetCell("$500.00", 2));
                    sumInfo.AddCell(GetCell("$1,500.00", 2));
                    sumInfo.AddCell(GetCell("$14,000.00", 2));

                    sumInfo.WriteSelectedRows(0, 50, 30, (startTotals - 2f), pdfContent);

                    ct.Go();

                }


            }





        }

        private static PdfContentByte AddBarcodes(List<string> returnMailBarcodes, List<string> stateMailBarcodes, PdfStamper pdfStamper)
        {
            int index = 0;
            int page = 1;
            float linePosition = 735f;
            PdfContentByte pdfContent = pdfStamper.GetOverContent(page);
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 18, linePosition, 0);
            linePosition -= 8.05f;
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 18, linePosition, 0);
            linePosition -= 8.05f;
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 18, linePosition, 0);
            linePosition -= 8.05f;
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 18, linePosition, 0);
            linePosition -= 8.05f;
            if (page == 1)
            {
                float statemailPosition = 95f;
                for (int mailIndex = 0; mailIndex < 6; mailIndex++)
                {
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(returnMailBarcodes[mailIndex], ReturnMailBarcodeFont), 220, statemailPosition, 0);
                    statemailPosition -= 4.05f;
                }
            }
            return pdfContent;
        }

        private PdfPCell GetCell(string text, int horizontalAlign = 1, int verticalAlign = 2, bool isHeader = false)
        {
            PdfPCell cell = new PdfPCell();
            if (isHeader)
            {
                cell.AddHeader(new PdfPHeaderCell());
                cell.BackgroundColor = new BaseColor(211, 211, 211);
            }
            cell.HorizontalAlignment = horizontalAlign;
            cell.VerticalAlignment = verticalAlign;
            cell.Phrase = new Phrase(new Chunk(text, ArialLoan));
            return cell;
        }
    }
}
