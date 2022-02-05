using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;
using static Uheaa.Common.Dialog;

namespace PAYHISTLPP
{
    public class CreateReport
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public Process Proc { get; set; } = new Process();
        public string Dir { get; set; } = null;
        public string FileName { get; set; } = null;
        public int DisplayCount { get; set; }

        public CreateReport(ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun.LDA);
        }

        public void Main()
        {
            BorrowerAccounts borrowerAccounts = new BorrowerAccounts(DA);
            if (borrowerAccounts.ShowDialog() == DialogResult.OK)
            {
                Proc = borrowerAccounts.Proc;
                CheckRecovery();
                WriteLine("Pulling loan data for borrowers in this run");
                int count = 0;
                DisplayCount = Proc.Accounts.Count > 100 ? 100 : 10;
                List<string> AcctNoLoans = new List<string>();
                Parallel.ForEach(Proc.Accounts, acct =>
                {
                    if (acct.FileName.IsNullOrEmpty())
                    {
                        acct.Loans = DA.GetLoans(acct.Ssn, acct.Lender, borrowerAccounts.Proc.Tilp);
                        if (acct.Loans.Count == 0)
                            AcctNoLoans.Add(acct.Ssn);
                        Parallel.ForEach(acct.Loans, loan =>
                        {
                            if (Proc.Tilp)
                                loan.Payments = DA.GetPayments(null, null, loan.LN_SEQ, acct.Ssn, Proc.LenderCode, Proc.Tilp);
                            else
                                loan.Payments = DA.GetPayments(loan.LoanId.Substring(0, loan.LoanId.Length - 2), loan.LoanId.Substring(loan.LoanId.Length - 2, 2) ?? "", loan.LN_SEQ, acct.Ssn);
                        });
                    }
                    count++;
                    if (count % DisplayCount == 0)
                        WriteLine($"{count} borrowers loaded");
                });
                RemoveEmptyAccounts(AcctNoLoans);

                if (Proc.Accounts.Count > 0)
                    SetupReport();
                SetComplete();
                Info.Ok($"Process Complete for {Proc.Accounts.Count} borrowers");
            }
        }

        private void RemoveEmptyAccounts(List<string> acctNoLoans)
        {
            if (acctNoLoans.Count > 0)
                Info.Ok($"There are {acctNoLoans.Count} accounts with no loan information. These accounts will be removed.");
            foreach (string ssn in acctNoLoans)
                Proc.Accounts.Remove(Proc.Accounts.Where(p => p.Ssn == ssn).Single());
        }

        /// <summary>
        /// Gets a list of all the files and checks to make sure they were created. Any report that was not created fully is deleted.
        /// </summary>
        private void CheckRecovery()
        {
            if (Proc.InRecovery)
            {
                WriteLine("Getting recovery files, this might take a few minutes.");
                if (Proc.FileDirectory.IsPopulated())
                {
                    Dir = Path.Combine(EnterpriseFileSystem.TempFolder, Proc.FileDirectory);
                    if (Directory.Exists(Dir))
                    {
                        List<string> files = Directory.GetFiles(Dir).ToList();
                        //Remove all empty files
                        Parallel.ForEach(files, file =>
                        {
                            if (new FileInfo(file).Length < 4800 && !file.EndsWith("idx")) //A fully finished file starts at 4869
                                Repeater.TryRepeatedly(() => File.Delete(file));
                        });
                        //Get all the files that are left
                        files = Directory.GetFiles(Dir).ToList();
                        if (files.Count > 0)
                        {
                            Parallel.ForEach(Proc.Accounts, acct =>
                            {
                                acct.FileName = files.Where(p => Path.GetFileName(p.ToString()).Substring(0, 9) == acct.Ssn).FirstOrDefault();
                            });
                        }
                        FileName = Path.GetFileName(files.Where(p => Path.GetFileName(p.ToString()).StartsWith("COLL")).FirstOrDefault());
                    }
                    else
                        Dir = null;
                }
            }
        }

        private void SetupReport()
        {
            string fileName = (FileName ?? $"COLL_700126_{Proc.LenderCode}_{Program.SaleDate:yyyyMMdd}_{DateTime.Now:yyyyMMddhhmmss}").Replace(".idx", "");
            string name = Path.Combine(EnterpriseFileSystem.TempFolder, fileName);
            Dir ??= Path.Combine(EnterpriseFileSystem.TempFolder, name);
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
            DA.UpdateDirectory(Dir, Proc.RunId);

            ZipFile zFile = new ZipFile(name + ".zip");
            ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

            WriteLine("Creating PDF files for borrowers in this run. This will take a while.");
            int count = 0;
            Parallel.ForEach(Proc.Accounts, bor =>
            {
                try
                {
                    if (bor.FileName.IsNullOrEmpty())
                    {
                        string file = AddTable(bor);
                        lockSlim.EnterWriteLock();
                        zFile.AddFile(file);
                        AddToIndexFile(bor.Ssn, bor.FirstName, bor.LastName, bor.Loans.OrderBy(p => p.DisbursementDate).First(), Path.Combine(name, fileName));
                        lockSlim.ExitWriteLock();
                    }
                    else
                        zFile.AddFile(bor.FileName);
                    count++;
                    if (count % DisplayCount == 0)
                        WriteLine($"{count} documents created");
                }
                catch (Exception ex)
                {
                    string message = $"There was an error adding borrower: {bor.Ssn}, AccountsId: {bor.AccountsId} to the payment history report. EX: {ex.Message}.";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }
            });
            WriteLine("Adding PDF files to new zip file.");
            if (zFile.Count > 0)
            {
                zFile.AddFile(Path.Combine(name, fileName) + ".idx");
                zFile.Save();
                File.Move(name + ".zip", Path.Combine(EnterpriseFileSystem.TempFolder, Path.GetFileName(name + ".zip")));
                File.Delete(Path.Combine(name, fileName) + ".idx");
            }

            Thread.Sleep(2000);
            Repeater.TryRepeatedly(() => Directory.Delete(Dir, true));
        }

        private string AddTable(Accounts bor)
        {
            var fontLargeb = FontFactory.GetFont("ARIAL", 16);
            fontLargeb.SetStyle("bold");
            string cpy = Path.Combine(Dir, $"{bor.Ssn}_PayHistory_" + Guid.NewGuid().ToBase64String() + ".pdf");
            string pdfFilePath = cpy;
            var letterOrientation = PageSize.LETTER.Rotate();
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
                    CreateHeaderCell("Payment History", fontLargeb, table, 9);
                    table.CompleteRow();
                    CreateHeaderCell(" ", fontLargeb, table, 9);
                    table.CompleteRow();
                    CreateHeaderCell($"Borrower: {bor.Ssn} {bor.Name}", font10, table, 9, Element.ALIGN_LEFT);
                    table.CompleteRow();
                    foreach (var loan in bor.Loans)
                    {
                        CreateHeaderCell(" ", fontLargeb, table, 9);
                        table.CompleteRow();
                        CreateHeaderCell($"Loan Program: {loan.LoanProgram} Disbursement Date:{loan.DisbursementDate:MM/dd/yyyy} LoanId: {loan.LoanId}", font10b, table, 9, Element.ALIGN_LEFT);
                        table.CompleteRow();
                        CreateHeaderCell(" ", fontLargeb, table, 9);
                        table.CompleteRow();
                        table.HeaderRows = 1;

                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Tran Date", font10b))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Post Date", font10b))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Applied Date", font10b))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Tran Code", font10b))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Tran Amount", font10b))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Tran Principal", font10b))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Tran Interest", font10b))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Tran Fees", font10b))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(new Chunk("Prin Bal After Tran", font10b))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });

                        foreach (Payment pay in loan.Payments)
                        {
                            table.AddCell(new PdfPCell(new Phrase(new Chunk($"{pay.TransactionEffectiveDate:yyyy-MM-dd}", font10))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk($"{pay.PostEffectiveDate:yyyy-MM-dd}", font10))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk($"{pay.ApplicationDate:yyyy-MM-dd}", font10))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk(pay.TransactionType, font10))) { HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk(pay.TransactionAmount, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk(pay.Principal, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk(pay.Interest, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk(pay.Fees, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
                            table.AddCell(new PdfPCell(new Phrase(new Chunk(pay.Balance, font10))) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
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

        private static void CreateHeaderCell(string colName, Font font, PdfPTable table, int colspan = 9, int horizontalAlignment = Element.ALIGN_CENTER)
        {
            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddHeader(new PdfPHeaderCell());
            cell.HorizontalAlignment = horizontalAlignment; //center
            cell.VerticalAlignment = Element.ALIGN_CENTER;//center
            cell.Phrase = new Phrase(new Chunk(colName, font));

            if (colspan != 1)
                cell.Colspan = colspan;

            table.AddCell(cell);
        }

        private void AddToIndexFile(string ssn, string firstName, string lastName, Loan borrower, string file)
        {
            var indexPieces = new string[] { ssn, lastName, firstName, borrower.DocType, borrower.LoanId , $"{borrower.DocDate.ToDate():MM/dd/yyyy}",
                borrower.LoanProgramType.Trim(), $"{borrower.GuarantyDate:MM/dd/yyyy}", $"{Program.SaleDate:MM/dd/yyyy}", borrower.DealId, Path.GetFileName(file) };
            using StreamWriter sw = new StreamWriter(file + ".idx", true);
            sw.WriteLine(string.Join("|", indexPieces));
        }

        /// <summary>
        /// Sets all of the accounts as processed and sets the final run job as processed
        /// </summary>
        private void SetComplete()
        {
            WriteLine("Setting completed at for each borrower and run job");
            Parallel.ForEach(Proc.Accounts, acct =>
            {
                DA.SetProcessedAt(acct.AccountsId);
            });
            DA.SetCompletedAt(Proc.RunId);
        }
    }
}