using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using iTextSharp.text.xml.xmp;
using Uheaa.Common;
using System.Threading.Tasks;
using System.Text;
using Ionic.Zip;
using System.Threading;
using System.Data.SqlClient;

namespace PAYHISTFED
{
    public class PaymentHistoryReport
    {
        private ProcessLogData PL { get; set; }

        private readonly string _sasFile;
        private string Dir = EnterpriseFileSystem.TempFolder;
        public PaymentHistoryReport()
        {
            _sasFile = EnterpriseFileSystem.TempFolder + "Payment History - Imaging FED.R2";
            PL = ProcessLogger.RegisterApplication("PAYHISTFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
        }

        private static void CreateHeaderCell(string colName, Font font, PdfPTable table, int colspan = 9, int horizontalAlignment = Element.ALIGN_CENTER)
        {
            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddHeader(new PdfPHeaderCell());
            cell.HorizontalAlignment = horizontalAlignment; //center
            cell.VerticalAlignment = Element.ALIGN_CENTER;//center
            //cell.BackgroundColor = new BaseColor(211, 211, 211);
            cell.Phrase = new Phrase(new Chunk(colName, font));

            if (colspan != 1)
            {
                cell.Colspan = colspan;
            }

            table.AddCell(cell);
        }

        public static string AddTable(Borrower bor, string dloFile, string lncFile)
        {
            var fontLargeb = FontFactory.GetFont("ARIAL", 16);
            fontLargeb.SetStyle("bold");
            string cpy = Path.Combine(bor.Ea80Data.LP == 1 ? dloFile : lncFile, $"{bor.SSN}_PayHistory_" + Guid.NewGuid().ToBase64String() + ".pdf");
            //File.Copy(file, cpy);
            string pdfFilePath = cpy;
            var letterOrientation = iTextSharp.text.PageSize.LETTER.Rotate();
            using (Document doc = new Document(letterOrientation, 0, 0, 100, 50))
            {
                //Create Document class object and set its size to letter and give space left, right, Top, Bottom Margin
                using (PdfWriter wri = PdfWriter.GetInstance(doc, new FStream(pdfFilePath, FileMode.Create)))
                {
                    wri.PageEvent = new ITextEvents();
                    wri.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                    wri.CompressionLevel = PdfStream.BEST_COMPRESSION;
                    wri.SetFullCompression();

                    doc.Open();//Open Document to write

                    var font10 = FontFactory.GetFont("ARIAL", 8);
                    var font10b = FontFactory.GetFont("ARIAL", 8);
                    font10b.SetStyle("bold");


                    //Create instance of the pdf table and set the number of column in that table
                    PdfPTable table = new PdfPTable(9);
                    List<PdfPTable> tables = new List<PdfPTable>();
                    PdfPCell cell = null;
                    CreateHeaderCell("Payment History", fontLargeb, table, 9);
                    table.CompleteRow();
                    CreateHeaderCell(" ", fontLargeb, table, 9);
                    table.CompleteRow();
                    CreateHeaderCell($"Borrower: {bor.SSN} {bor.Name}", font10, table, 9, Element.ALIGN_LEFT);
                    table.CompleteRow();
                    //CreateHeaderCell(" ", fontLargeb, table, 9);
                    //table.CompleteRow();
                    foreach (var loan in bor.Loans)
                    {

                        CreateHeaderCell(" ", fontLargeb, table, 9);
                        table.CompleteRow();
                        CreateHeaderCell($"Loan Program: {loan.LoanProgram} Disbursement Date:{loan.DisbursementDate.ToString("MM/dd/yyyy")} AwardId: {loan.AwardId} AwardIdSeq: {loan.AwardIdSeq}", font10b, table, 9, Element.ALIGN_LEFT);
                        table.CompleteRow();
                        CreateHeaderCell(" ", fontLargeb, table, 9);
                        table.CompleteRow();
                        table.HeaderRows = 1;

                        cell = AddCol(font10b, table, "Tran Date");
                        cell = AddCol(font10b, table, "Post Date");
                        cell = AddCol(font10b, table, "Applied Date");
                        cell = AddCol(font10b, table, "Tran Code");
                        cell = AddCol(font10b, table, "Tran Amount");
                        cell = AddCol(font10b, table, "Tran Principal");
                        cell = AddCol(font10b, table, "Tran Interest");
                        cell = AddCol(font10b, table, "Tran Fees");
                        cell = AddCol(font10b, table, "Prin Bal After Tran");

                        table.CompleteRow();

                        //How add the data from datatable to pdf table
                        //for (int rows = 0; rows < dt.Rows.Count; rows++)
                        //{
                        //    for (int column = 0; column < dt.Columns.Count; column++)
                        //    {
                        PdfPCell cell1 = null;
                        PdfPCell cell2 = null;
                        PdfPCell cell3 = null;
                        PdfPCell cell4 = null;
                        PdfPCell cell5 = null;
                        PdfPCell cell6 = null;
                        PdfPCell cell7 = null;
                        PdfPCell cell8 = null;
                        PdfPCell cell9 = null;
                        foreach (var pay in loan.Payments)
                        {
                            cell1 = new PdfPCell(new Phrase(new Chunk(pay.TransactionEffectiveDate, font10))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER };
                            cell2 = new PdfPCell(new Phrase(new Chunk(pay.PostEffectiveDate, font10))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER };
                            cell3 = new PdfPCell(new Phrase(new Chunk(pay.ApplicationDate, font10))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER };
                            cell4 = new PdfPCell(new Phrase(new Chunk(pay.TransactionType, font10))) { HorizontalAlignment = 0, Border = Rectangle.NO_BORDER };
                            cell5 = new PdfPCell(new Phrase(new Chunk(pay.TransactionAmount, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER };
                            cell6 = new PdfPCell(new Phrase(new Chunk(pay.Principal, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER };
                            cell7 = new PdfPCell(new Phrase(new Chunk(pay.Interest, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER };
                            cell8 = new PdfPCell(new Phrase(new Chunk(pay.Fees, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER };
                            cell9 = new PdfPCell(new Phrase(new Chunk(pay.Balance, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER };


                            table.AddCell(cell1);
                            table.AddCell(cell2);
                            table.AddCell(cell3);
                            table.AddCell(cell4);
                            table.AddCell(cell5);
                            table.AddCell(cell6);
                            table.AddCell(cell7);
                            table.AddCell(cell8);
                            table.AddCell(cell9);

                        }

                        table.SpacingBefore = 100f; // Give some space after the text or it may overlap the table
                        tables.Add(table);
                        table = new PdfPTable(9);

                    }
                    foreach (var t in tables)
                    {
                        doc.Add(t); // add pdf table to the document
                        doc.NewPage();
                    }

                    //Close document and writer
                    doc.Close();
                }
            }

            return pdfFilePath;
        }
        private static PdfPCell AddCol(Font font10b, PdfPTable table, string col)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddHeader(new PdfPHeaderCell());
            cell.HorizontalAlignment = 1; //center
            cell.VerticalAlignment = 2;//center
                                       // cell.BackgroundColor = new BaseColor(211, 211, 211);
            cell.Phrase = new Phrase(new Chunk(col, font10b));
            table.AddCell(cell);
            return cell;
        }

        const string SendingServicerCode = "700502";
        const string SendingLenderCode = "898502";
        public string Name(string dealId, string saleDate, string fileName)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

            return Path.Combine(Dir, fileName);
        }

        public void Main()
        {
            if (!Dialog.Info.YesNo("This is the Payment History Report--Imaging FED script. Click OK to continue, or Cancel to quit."))
                return;
            DateTime start = DateTime.Now;
            //Get the SAS records into a collection of Payment objects.
            IEnumerable<Borrower> bors = null;
            try
            {
                bors = GetSasRecords();
            }
            catch (Exception ex)
            {
                ProcessLogger.AddNotification(PL.ProcessLogId, "There was an error loaded the SAS files", NotificationType.ErrorReport, NotificationSeverityType.Critical, PL.ExecutingAssembly, ex);
                Dialog.Error.Ok(ex.Message);
                return;
            }

            string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
            string fileNameDLO = $"COLL_{SendingServicerCode}_{SendingLenderCode}_{Program.DLODeal}_{Program.SaleDate}_{timestamp}";
            string fileNameLNC = $"COLL_{SendingServicerCode}_{SendingLenderCode}_{Program.LNCDeal}_{Program.SaleDate}_{timestamp}";
            string dloName = Name(Program.DLODeal, Program.SaleDate, fileNameDLO);
            string lncName = Name(Program.LNCDeal, Program.SaleDate, fileNameLNC);
            string dloDir = Path.Combine(Dir, dloName);
            string lncDir = Path.Combine(Dir, lncName);
            if (Directory.Exists(dloDir))
            {
                Directory.Delete(dloDir, true);
                Thread.Sleep(2000);
            }
            Directory.CreateDirectory(dloDir);

            if (Directory.Exists(lncDir))
            {
                Directory.Delete(lncDir, true);
                Thread.Sleep(2000);
            }
            Directory.CreateDirectory(lncDir);

            ZipFile DLO = new ZipFile(dloName + ".zip");
            ZipFile LNC = new ZipFile(lncName + ".zip");

            foreach (var bor in bors)
            {

                bor.Ea80Data = DataAccessHelper.ExecuteSingle<EA80Data>("GetEA80DataPaymentHistory", DataAccessHelper.Database.Cdw, new SqlParameter("SSN", bor.SSN));

                Console.WriteLine($"{bor.BorrowerNumber}");
                string file = AddTable(bor, dloDir, lncDir);
                if (bor.Ea80Data.LP == 1)
                {
                    DLO.AddFile(file);
                    bor.Ea80Data.FileName = file;
                    AddToIndexFile(bor.Ea80Data, Path.Combine(dloName, fileNameDLO));
                }
                else
                {
                    LNC.AddFile(file);
                    bor.Ea80Data.FileName = file;
                    AddToIndexFile(bor.Ea80Data, Path.Combine(lncName, fileNameLNC));
                }
            }
            if (DLO.Count > 0)
            {
                DLO.AddFile(Path.Combine(dloName, fileNameDLO) + ".idx");
                DLO.Save();
                File.Move(dloName + ".zip", Path.Combine(EnterpriseFileSystem.TempFolder, Path.GetFileName(dloName + ".zip")));
                File.Delete(Path.Combine(dloName, fileNameDLO) + ".idx");
            }
            if (LNC.Count > 0)
            {
                DLO.AddFile(Path.Combine(lncName, fileNameLNC) + ".idx");
                LNC.Save();
                File.Move(lncName + ".zip", Path.Combine(EnterpriseFileSystem.TempFolder, Path.GetFileName(lncName + ".zip")));
                File.Delete(Path.Combine(lncName, fileNameLNC) + ".idx");
            }

            Thread.Sleep(2000);
            Directory.Delete(dloDir, true);
            Directory.Delete(lncDir, true);

            File.Delete(_sasFile);

            ProcessLogger.LogEnd(PL.ProcessLogId);
            Dialog.Info.Ok("Process Complete");
        }

        private void AddToIndexFile(EA80Data borrower, string file)
        {

            var indexPieces = new string[] { borrower.BF_SSN, borrower.LastName, borrower.FirstName, borrower.DocType,
                               borrower.LoanId , borrower.DocDate, "DIRL", borrower.GuarantyDate, borrower.SaleDate, borrower.DealId, Path.GetFileName(borrower.FileName)  };
            using (StreamWriter sw = new StreamWriter(file + ".idx", true))
            {
                sw.WriteLine(string.Join("|", indexPieces));
            }

        }

        private IEnumerable<Borrower> GetSasRecords()
        {
            if (!File.Exists(_sasFile)) { throw new Exception(_sasFile + " missing. Ending script."); }
            if (new FileInfo(_sasFile).Length == 0) { throw new Exception(_sasFile + " empty. Ending script."); }
            List<Payment> payments = new List<Payment>();
            List<Borrower> Bors = new List<Borrower>();
            using (StreamReader sasReader = new StreamReader(_sasFile))
            {
                int borCount = 1;
                //Get the header row out of the way.
                sasReader.ReadLine();
                //Parse the rest of the file.
                string prevLoanProgram = "";
                string preBwr = "";
                string preAwardId = "";
                string preAwardIdSeq = "";
                string award_id = "";
                string award_id_seq = "";
                DateTime? prevDisbDate = null;
                Borrower bor = new Borrower();
                Loan loan = new Loan();
                while (!sasReader.EndOfStream)
                {



                    string loanProgram = "";
                    DateTime? disbDate = null;
                    string sasLine = sasReader.ReadLine();
                    List<string> data = sasLine.SplitAndRemoveQuotes(",").ToList();
                    if (data[0].ToInt() == borCount)
                    {
                        ProcessLine(ref prevLoanProgram, ref preBwr, ref prevDisbDate, bor, ref loan, out loanProgram, out disbDate, sasLine, data, out award_id, out award_id_seq,ref preAwardId, ref preAwardIdSeq);
                    }
                    else
                    {
                        if (bor.Loans.Any() && loan.Payments.Any())
                        {
                            if (loan.LoanProgram.IsNullOrEmpty() || loan.DisbursementDate < new DateTime(1925, 01, 01))
                            {
                                throw new Exception($"Borrower:{bor.BorrowerNumber} has a loan without program");
                            }
                            bor.Loans.Add(loan);
                        }
                        else if(!bor.Loans.Any() && loan.Payments.Any())
                        {
                            if (loan.LoanProgram.IsNullOrEmpty() || loan.DisbursementDate < new DateTime(1925, 01, 01))
                            {
                                throw new Exception($"Borrower:{bor.BorrowerNumber} has a loan without program");
                            }
                            bor.Loans.Add(loan);
                        }
                        else
                        {
                            throw new Exception($"Have Jarom review this file {bor.BorrowerNumber} {_sasFile}");
                        }
                        if (bor.Loans.Any())
                            Bors.Add(bor);
                        bor = new Borrower();
                        loan = new Loan();
                        borCount++;
                        prevLoanProgram = null;
                        prevDisbDate = null;
                        ProcessLine(ref prevLoanProgram, ref preBwr, ref prevDisbDate, bor, ref loan, out loanProgram, out disbDate, sasLine, data, out award_id, out award_id_seq, ref preAwardId, ref preAwardIdSeq);
                    }
                    if (sasReader.EndOfStream)
                    {
                        if (loan.Payments.Any())
                        {
                            bor.Loans.Add(loan);
                        }
                        if (bor.Loans.Any())
                        {
                            
                            Bors.Add(bor);
                        }
                    }
                }
            }
            return Bors;
        }

        private void ProcessLine(ref string prevLoanProgram, ref string preBwr, ref DateTime? prevDisbDate, Borrower bor, ref Loan loan, out string loanProgram, out DateTime? disbDate, string sasLine, List<string> data, out string award_id, out string award_id_seq, ref string previousAwardId ,ref string previousAwardIdSeq)
        {
            if (bor.Name.IsNullOrEmpty())
            {
                bor.Name = data[2];
                bor.SSN = data[1];
                bor.BorrowerNumber = data[0].ToInt();
            }
            if (prevLoanProgram.IsNullOrEmpty())
            {
                preBwr = data[1];
                loanProgram = data[3];
                disbDate = data[4].ToDate();
                award_id = data[14];
                award_id_seq = data[15];
                loan.LoanProgram = loanProgram;
                loan.DisbursementDate = disbDate.Value;
                loan.AwardId = award_id;
                loan.AwardIdSeq = award_id_seq;
                if (loan.Payments.Any())
                    bor.Loans.Add(loan);//add first loan
            }
            else
            {
                loanProgram = data[3];
                disbDate = data[4].ToDate();
                award_id = data[14];
                award_id_seq = data[15];
                if (previousAwardId != award_id || previousAwardIdSeq != award_id_seq)
                {
                    if (loan.Payments.Any())
                    {
                        if (loan.LoanProgram.IsNullOrEmpty() || loan.DisbursementDate < new DateTime(1925, 01, 01))
                        {
                            throw new Exception($"Borrower:{bor.BorrowerNumber} has a loan without program");
                        }
                        bor.Loans.Add(loan);
                    }
                    loan = new Loan();
                    loan.LoanProgram = loanProgram;
                    loan.DisbursementDate = disbDate.Value;
                    loan.AwardId = award_id;
                    loan.AwardIdSeq = award_id_seq;
                }
            }
            Payment payment = Payment.Parse(sasLine, PL);
            if (payment.TransactionType.IsPopulated())
                loan.Payments.Add(payment);

            prevLoanProgram = loanProgram;
            prevDisbDate = disbDate;
            previousAwardId = award_id;
            previousAwardIdSeq = award_id_seq;
        }
    }
}