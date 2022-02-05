using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Reflection;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace SpecialProgramWriteOffs
{
    public class SpecialProgramWriteOffs : BatchScriptBase
    {
        //Class constants and read-only identifers
        private const int MAX_LOANS = 25;
        private const string SAS_JOB = "ULWA18.LWA18";
        private readonly string DOC_FOLDER;
        private readonly string ERROR_FILE;
        private readonly string FTP_FOLDER;
        private readonly string LOG_FOLDER;
        private readonly bool TEST_MODE;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri">Instance of Reflection Interface</param>
        public SpecialProgramWriteOffs(ReflectionInterface ri)
            : base(ri, "SPCLWO")
        {
            TestModeResults mode = TestMode(@"X:\PADD\BorrowerServices\");
            DOC_FOLDER = mode.DocFolder;
            ERROR_FILE = string.Format(@"X:\Reports\Operational Accounting\Special Write Offs\Errors {0}.txt", DateTime.Now.ToString("MMddyy"));
            FTP_FOLDER = mode.FtpFolder;
            LOG_FOLDER = mode.LogFolder;
            TEST_MODE = ri.TestMode;
        }

        public override void Main()
        {
            //Move file for finance.
            string report2 = Directory.GetFiles(FTP_FOLDER, "ULWA18.LWA18R2.*").FirstOrDefault();
            if (!string.IsNullOrEmpty(report2))
            {
                StringBuilder destinationFileNameBuilder = new StringBuilder(@"Q:\Finance\Monthend Data\");
                if (TEST_MODE) { destinationFileNameBuilder.Append(@"Test\"); }
                destinationFileNameBuilder.Append(@"Special Write off Detail.");
                destinationFileNameBuilder.Append(DateTime.Now.ToString("yyyyMMdd"));
                File.Copy(report2, destinationFileNameBuilder.ToString(), true);
            }

            //Prompt the user of the script's purpose.
            if (!CalledByMasterBatchScript())
            {
                string message = @"This script will process the Special Write-off file.";
                string caption = @"Special Loan Balance Write-off Program";
                if (MessageBox.Show(message, caption, MessageBoxButtons.OKCancel) != DialogResult.OK) { return; }
            }

            //If in recovery go to the right place to recover from.
            if (Recovery.RecoveryValue.Length > 0)
            {
                string report4 = Directory.GetFiles(FTP_FOLDER, SAS_JOB + "R4.*").FirstOrDefault();
                if (!string.IsNullOrEmpty(report2))
                {
                    //File erred during loan level processing.
                    ProcessLoanFile(report2, report4);
                    ProcessBorrowerFile(report4);
                }
                else
                {
                    if (!string.IsNullOrEmpty(report4))
                    {
                        //File erred during borrower level processing.
                        ProcessBorrowerFile(report4);
                    }
                }
            }
            else //Not in recovery; process normally.
            {
                //Check that SAS files are present and not empty, and remove any old SAS files encountered.
                report2 = GetNewestFile(SAS_JOB + "R2.*");
                string report4 = GetNewestFile(SAS_JOB + "R4.*");
                //Process loan level file.
                ProcessLoanFile(report2, report4);
                ProcessBorrowerFile(report4);
            }

            //Send e-mail if errors were encountered.
            NotifyOfErrors();

            //Delete the recovery log file now that all records are processed.
            Recovery.Delete();
            ProcessingComplete(true);
        }//Main()

        private string GetNewestFile(string searchPattern)
        {
            string newestFile = DeleteOldFilesReturnMostCurrent(FTP_FOLDER, searchPattern, Common.FileOptions.None);
            if (newestFile.Length == 0)
            {
                string message = string.Format("The {0} data file is missing. Please contact Systems Support for assistance.", searchPattern);
                string caption = "Missing SAS file";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                EndDLLScript();
            }
            if (new FileInfo(newestFile).Length == 0)
            {
                File.Delete(newestFile);
                string message = "The data file is empty; there are no records to process.";
                string caption = "Empty data file";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                EndDLLScript();
            }
            return newestFile;
        }//GetNewestFile()

        //Handles write-offs on ATS3Q and calls ATD22AllLoans to add comments.
        private void ProcessLoanFile(string loanFile, string borrowerFile)
        {
            DataTable sasTable = CreateDataTableFromFile(loanFile);
            for (int rowIndex = GetRecoveryRow(0); rowIndex < sasTable.Rows.Count; rowIndex++)
            {
                DataRow sasRow = sasTable.Rows[rowIndex];
                Loan loan = new Loan
                {
                    Ssn = sasRow.Field<string>("BF_SSN"),
                    Sequence = sasRow.Field<string>("LN_SEQ"),
                    Principal = sasRow.Field<string>("LA_CUR_PRI"),
                    Interest = sasRow.Field<string>("LA_NSI_OTS")
                };
                //Find loans on ATS3Q to write off.
                FastPath(string.Format("TX3Z/ATS3Q{0};{1}", loan.Ssn, DateTime.Now.AddDays(-1).ToString("MMddyy")));
                if (Check4Text(1, 72, "TSX3S"))
                {
                    //Selection screen. Find this loan seq and write it off.
                    int row = 7;
                    while (!Check4Text(row, 4, " "))
                    {
                        //Casting the screen scrape to an int and back to a string
                        //has the desired effect of stripping off any leading zeros.
                        if (int.Parse(GetText(row, 20, 4)).ToString() == loan.Sequence)
                        {
                            PutText(22, 19, GetText(row, 3, 2), Key.Enter);
                            if (!Check4Text(23, 2, "02495")) { WriteOffLoan(loan, true, borrowerFile); }
                            break;
                        }
                        row++;
                    }
                }
                else if (Check4Text(1, 72, "TSX3O"))
                {
                    //Target screen.
                    WriteOffLoan(loan, false, borrowerFile);
                }

                //Write the row index out to the log file.
                Recovery.RecoveryValue = rowIndex.ToString();
            }
            //Delete the recovery log and SAS file once processing is complete.
            Recovery.Delete();
            File.Delete(loanFile);
        }//ProcessLoanFile()

        private void NotifyOfErrors()
        {
            if (!File.Exists(ERROR_FILE))
            {
                return;
            }
            List<string> recipients = DataAccess.GetErrorRecipients(TEST_MODE);
            string subject = "Special Write-Off Error Report";
            string body = @"There is a special write-off error report to review in X:\Reports\Operational Accounting\Special Write Offs.";
            SendMail(TEST_MODE, string.Join(",", recipients.ToArray()), string.Empty, subject, body, string.Empty, string.Empty, string.Empty, Common.EmailImportanceLevel.Normal, TEST_MODE);
        }//NotifyOfErrors()

        private void WriteOffLoan(Loan loan, bool returnToSelectionScreen, string report4)
        {
            PutText(8, 42, "X");
            //Update R4 with new data.
            loan.Principal = GetText(12, 32, 11);
            loan.Interest = GetText(13, 36, 6);
            UpdateBorrowerFile(loan.Ssn, loan.Sequence, "$" + loan.Interest, "$" + loan.Principal, report4);
            if (double.Parse(loan.Principal) > 0)
            {
                //Put the PRINCIPAL CURRENT VALUE into the PRINCIPAL AMOUNT OF REQUEST.
                PutText(12, 48, loan.Principal);
            }
            if (double.Parse(loan.Interest) > 0)
            {
                //Put the INTEREST CURRENT VALUE into the INTEREST AMOUNT OF REQUEST.
                PutText(13, 49, loan.Interest);
            }
            //Enter comments.
            PutText(18, 2, "Special Loan Balance Write Off. /SPCLWO");
            //Post the write-off.
            Hit(Key.F11);
            //Check that the adjustment request has been created, and log an error if not.
            if (!Check4Text(23, 2, "02238"))
            {
                using (StreamWriter errorWriter = new StreamWriter(ERROR_FILE, true))
                {
                    errorWriter.WriteLine(string.Format("{0} {1} Failed to create adjustment request.", loan.Ssn, loan.Sequence));
                    errorWriter.Close();
                }
            }
            if (returnToSelectionScreen)
            {
                Hit(Key.F12);
                //The selection screen retains our last selection in the selection field,
                //so blank it out to avoid any problems.
                Hit(Key.EndKey);
            }
        }//WriteOffLoan()

        private void ProcessBorrowerFile(string borrowerFile)
        {
            DataTable sasTable = CreateDataTableFromFile(borrowerFile);
            for (int rowIndex = GetRecoveryRow(0); rowIndex < sasTable.Rows.Count; rowIndex++)
            {
                DataRow sasRow = sasTable.Rows[rowIndex];
                string loanStringForComments = string.Empty;
                List<int> lnSeqs = new List<int>();
                for (int i = 12; i <= 108; i += 4)
                {
                    if (sasRow.Field<string>(i).Length == 0)
                    {
                        if (loanStringForComments.Length > 1)
                        {
                            loanStringForComments = loanStringForComments.Substring(1, loanStringForComments.Length - 2);
                        }
                        break;
                    }
                    else
                    {
                        lnSeqs.Add(int.Parse(sasRow.Field<string>(i)));
                        loanStringForComments += string.Format(" SEQ {0}, PRIN {1}, INT {2},", sasRow.Field<string>(i), sasRow.Field<string>(i + 2), sasRow.Field<string>(i + 3));
                    }
                }

                //Call ATD22 with a comment reflecting whether a letter was sent.
                string commentStart = "Special loan balance write-off; ";
                if (sasRow.Field<string>(10) != "Y")
                {
                    commentStart += "no ";
                }
                commentStart += "letter sent ";
                ATD22ByLoan(sasRow.Field<string>(0), "SPEWO", commentStart + loanStringForComments, lnSeqs, false);
                //Update log.
                Recovery.RecoveryValue = rowIndex.ToString();
            }
            //Prompt the user to switch the printer to duplex before printing letters.
            string message = "The letters generated by this script must be printed two-sided.  Set the default properties of your printer to duplex (contact Computer Services if you are unsure how this is done).  Click OK to resume processing ONLY AFTER you have set up your printer to print duplex.";
            MessageBox.Show(message, "Duplex Printing Setup", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            PrintLettersToValidAddresses(borrowerFile);
            message = "Reset the default properties of your printer if you do not want the setting left to duplex.";
            MessageBox.Show(message, "DUPLEX PRINTING", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            File.Delete(borrowerFile);
        }//ProcessBorrowerFile()

        private void PrintLettersToValidAddresses(string sasFile)
        {
            DataTable sasTable = CreateDataTableFromFile(sasFile).SelectIntoNewDataTable("DI_VLD_ADR = 'Y'");
            //Add a column for today's date and put today's date in all rows.
            sasTable.Columns.Add("Date");
            foreach (DataRow row in sasTable.Rows)
            {
                row.SetField<string>("Date", DateTime.Now.ToString("MMMM dd, yyyy"));
            }
            string dataFile = DataAccess.PersonalDataDirectory + ScriptID + "Dat.txt";
            sasTable.WriteToFile(dataFile, true,
                "BF_SSN", "FirstName", "LastName", "Address1", "Address2", "City",
                "State", "ZIP", "Country", "AccountNumber", "DI_VLD_ADR", "ACSKEY",
                "LOAN1_SEQ", "LOAN1_TYPE", "LOAN1_PRIN_AMT", "LOAN1_INT_AMT",
                "LOAN2_SEQ", "LOAN2_TYPE", "LOAN2_PRIN_AMT", "LOAN2_INT_AMT",
                "LOAN3_SEQ", "LOAN3_TYPE", "LOAN3_PRIN_AMT", "LOAN3_INT_AMT",
                "LOAN4_SEQ", "LOAN4_TYPE", "LOAN4_PRIN_AMT", "LOAN4_INT_AMT",
                "LOAN5_SEQ", "LOAN5_TYPE", "LOAN5_PRIN_AMT", "LOAN5_INT_AMT",
                "LOAN6_SEQ", "LOAN6_TYPE", "LOAN6_PRIN_AMT", "LOAN6_INT_AMT",
                "LOAN7_SEQ", "LOAN7_TYPE", "LOAN7_PRIN_AMT", "LOAN7_INT_AMT",
                "LOAN8_SEQ", "LOAN8_TYPE", "LOAN8_PRIN_AMT", "LOAN8_INT_AMT",
                "LOAN9_SEQ", "LOAN9_TYPE", "LOAN9_PRIN_AMT", "LOAN9_INT_AMT",
                "LOAN10_SEQ", "LOAN10_TYPE", "LOAN10_PRIN_AMT", "LOAN10_INT_AMT",
                "LOAN11_SEQ", "LOAN11_TYPE", "LOAN11_PRIN_AMT", "LOAN11_INT_AMT",
                "LOAN12_SEQ", "LOAN12_TYPE", "LOAN12_PRIN_AMT", "LOAN12_INT_AMT",
                "LOAN13_SEQ", "LOAN13_TYPE", "LOAN13_PRIN_AMT", "LOAN13_INT_AMT",
                "LOAN14_SEQ", "LOAN14_TYPE", "LOAN14_PRIN_AMT", "LOAN14_INT_AMT",
                "LOAN15_SEQ", "LOAN15_TYPE", "LOAN15_PRIN_AMT", "LOAN15_INT_AMT",
                "LOAN16_SEQ", "LOAN16_TYPE", "LOAN16_PRIN_AMT", "LOAN16_INT_AMT",
                "LOAN17_SEQ", "LOAN17_TYPE", "LOAN17_PRIN_AMT", "LOAN17_INT_AMT",
                "LOAN18_SEQ", "LOAN18_TYPE", "LOAN18_PRIN_AMT", "LOAN18_INT_AMT",
                "LOAN19_SEQ", "LOAN19_TYPE", "LOAN19_PRIN_AMT", "LOAN19_INT_AMT",
                "LOAN20_SEQ", "LOAN20_TYPE", "LOAN20_PRIN_AMT", "LOAN20_INT_AMT",
                "LOAN21_SEQ", "LOAN21_TYPE", "LOAN21_PRIN_AMT", "LOAN21_INT_AMT",
                "LOAN22_SEQ", "LOAN22_TYPE", "LOAN22_PRIN_AMT", "LOAN22_INT_AMT",
                "LOAN23_SEQ", "LOAN23_TYPE", "LOAN23_PRIN_AMT", "LOAN23_INT_AMT",
                "LOAN24_SEQ", "LOAN24_TYPE", "LOAN24_PRIN_AMT", "LOAN24_INT_AMT",
                "LOAN25_SEQ", "LOAN25_TYPE", "LOAN25_PRIN_AMT", "LOAN25_INT_AMT",
                "STATE_IND", "COST_CENTER_CODE", "Date"
                );
            const string LETTER_ID = "BALWO";
            DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(dataFile, "AccountNumber", LETTER_ID, false, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TEST_MODE);
            DocumentHandling.CostCenterPrinting("Special Write-off Borrower Notice", DocumentHandling.DestinationOrPageCount.Page1, LETTER_ID, dataFile, "COST_CENTER_CODE", "STATE_IND", DateTime.Now.ToString(), ScriptID, TEST_MODE, false, true);
            Thread.Sleep(30000); //Give Word some time to clean up after itself.
            ImageDocs(11, LETTER_ID, DOC_FOLDER, LETTER_ID, dataFile);
            File.Delete(dataFile);
        }//PrintLettersToValidAddresses()

        //Update the R4 List and then write the data out to the file again.
        private void UpdateBorrowerFile(string ssn, string lnSeq, string newInterest, string newPrincipal, string sasFile)
        {
            //Read the SAS file into a table.
            DataTable report4Table = CreateDataTableFromFile(sasFile);
            //Find the forst sasRow that matches the passed-in SSN.
            DataRow targetRow = report4Table.Select(string.Format("BF_SSN = {0}", ssn))[0];
            //Search for the correct loan. Loan information starts at column 12 and occupies 4 columns per loan.
            int lnSeqIndex = 12;
            while (targetRow.Field<string>(lnSeqIndex) != lnSeq)
            {
                lnSeqIndex += 4;
            }
            //Update the principal and interest for this loan.
            targetRow.SetField<string>(lnSeqIndex + 2, newPrincipal.Replace(",", ""));
            targetRow.SetField<string>(lnSeqIndex + 3, newInterest.Replace(",", ""));
            //Write the table back out to the file.
            report4Table.WriteToFile(sasFile, true);
        }//UpdateBorrowerFile()

        //Add copies of documents to the imaging system.
        private void ImageDocs(int ssnIndex, string docId, string docPath, string doc, string dataFile)
        {
            DocumentHandling.ImageDocs(ssnIndex, docId, docPath, doc, dataFile, TEST_MODE);
            Thread.Sleep(30000); //Give Word some time to clean up after itself.
            //We also need to print a report of the total number of imaged docs.
            int imagedCount = File.ReadAllLines(dataFile).Length - 1; //Number of records minus header sasRow.
            string imageData = DataAccess.PersonalDataDirectory + "iTotalsDat.txt";
            using (StreamWriter imageWriter = new StreamWriter(imageData, false))
            {
                imageWriter.WriteCommaDelimitedLine("ImagingTotals", "Blah");
                imageWriter.WriteCommaDelimitedLine(imagedCount.ToString(), string.Empty);
                imageWriter.Close();
            }
            DocumentHandling.PrintDocs(DOC_FOLDER, "iTotalsSPWO", imageData);
            File.Delete(imageData);
        }
    }//class
}//namespace
