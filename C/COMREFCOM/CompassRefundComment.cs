using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace COMREFCOM
{
    public class CompassRefundComment : ScriptBase
    {
        const string CSV_FILE_FOLDER = @"X:\PADD\LPP Accounting\";
        const string COVER_LETTER_FOLDER = @"X:\PADD\General\";
        const string ARC = "SRFCK";

        private readonly string REFUND_FILE;
        private readonly string REFUND_COMMENT_COVER;
        private readonly string LPP_REFUND_CSV_FILE;
        private readonly string ESCROW_DISB_CSV_FILE;
        private readonly string CONSOL_CSV_FILE;
        private readonly string LOCAL_LSC_CHECK_REGISTER;
        private readonly string LOG_FOLDER;

        protected override RecoveryLog Recovery
        {
            get
            {
                return base.Recovery;
            }
            set
            {
                base.Recovery = value;
            }
        }

        public CompassRefundComment(ReflectionInterface ri)
            : base(ri, "COMREFCOM")
        {
            base.Recovery = new RecoveryLog(string.Format("{0}_{1}", ScriptId, UserId));
            var temp = new Func<string, string>(s => Path.Combine(EnterpriseFileSystem.TempFolder, s));
            var csv = new Func<string, string, string>((folder, file) =>
            {
                var path = Path.Combine(CSV_FILE_FOLDER, folder);
                if (DataAccessHelper.TestMode)
                    path = Path.Combine(path, "Test");
                path = Path.Combine(path, file + "_" + DateTime.Now.ToString("MM_dd_yyyy") + ".csv");
                return path;
            });
            REFUND_FILE = temp("refund.txt");
            REFUND_COMMENT_COVER = temp("refundcommentCover.txt");
            LOCAL_LSC_CHECK_REGISTER = temp("LSC Check Register.csv");

            LPP_REFUND_CSV_FILE = csv("Refund", "LPP Refund Check Register");
            ESCROW_DISB_CSV_FILE = csv("Escrow", "Escrow Disbursement Check Register");
            CONSOL_CSV_FILE = csv("Consolidation", "Consolidation Check Register");

            LOG_FOLDER = Path.Combine(EnterpriseFileSystem.LogsFolder, "COMREFCOMLOG.txt");
        }

        public override void Main()
        {
            if (Dialog.Info.OkCancel("This is the Compass Refund Comment script. Click OK to continue, or Cancel to quit.", ScriptId))
            {
                if (FileCheck() && AccountHelper.LoadAccountNumber())
                {
                    SetupCsvFile();
                    if (ProcessRefundFile())
                    {
                        Thread.Sleep(5000);
                        SendEmails();
                        CleanupFiles();
                        Dialog.Info.Ok("Process Complete!", ScriptId);
                    }
                }
            }
        }

        /// <summary>
        /// Ensure necessary files exist and warn user if they don't.
        /// </summary>
        private bool FileCheck()
        {
            //warn the user if the file is missing and end script
            if (!File.Exists(REFUND_FILE))
            {
                Dialog.Warning.Ok(@"The refund.txt file is missing.  Place the files in T:\ and run the script again.", "Files Missing");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Setup the CSV file with headers.
        /// </summary>
        private void SetupCsvFile()
        {
            if (string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                string header1 = "LPP Check Register,,,,,,";
                string header2 = "Account,Check #,Amount,Date,Action,Payee 1,Payee 2";
                File.WriteAllLines(LPP_REFUND_CSV_FILE, new string[] { header1, header2 });
            }
        }

        private bool ProcessRefundFile()
        {
            using (StreamReader sr = new StreamReader(REFUND_FILE))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
                        if (Recovery.RecoveryValue == line)
                            Recovery.RecoveryValue = null;
                    if (string.IsNullOrEmpty(Recovery.RecoveryValue)) //not in recovery
                    {
                        Recovery.RecoveryValue = line;
                        var fields = line.Split('|');

                        Refund refundInfo = new Refund();
                        refundInfo.CheckType = fields[0];
                        refundInfo.CheckNumber = fields[1].Trim();
                        refundInfo.CheckDate = string.Format("{0}/{1}/{2}", fields[2].SafeSubString(5, 2), fields[2].SafeSubString(8, 2), fields[2].SafeSubString(0, 4));
                        refundInfo.SSN = fields[3].Trim();
                        refundInfo.CheckAmount = string.Format("{0:C}", double.Parse(fields[4]));
                        refundInfo.CheckRecipient = fields[5].Trim();

                        //prepopulate and display form to enter transactions
                        if (refundInfo.SSN.StartsWith("B") || refundInfo.SSN.Length == 0 || refundInfo.SSN == "000000000")
                        {
                            bool procResult = NoSsnProcessing(refundInfo);
                            if (!procResult)
                                return false;
                        }
                        else
                        {
                            string comment = string.Format("Send UHEAA Servicer Refund Check to {0} effective {1}, check number {2} for {3}.", refundInfo.CheckRecipient, refundInfo.CheckDate, refundInfo.CheckNumber, refundInfo.CheckAmount);
                            LeaveComment(refundInfo.SSN, comment);
                            //write record to spreadsheet file
                            List<string> values = new List<string>(new string[] {
                                AccountHelper.AccountNumber, refundInfo.CheckNumber, 
                                refundInfo.CheckAmount, refundInfo.CheckDate, "CN"});
                            string[] split = refundInfo.CheckRecipient.Split('|');
                            values.Add(split[0]);
                            values.Add(split.Length > 1 ? split[1] : "");
                            File.AppendAllText(LPP_REFUND_CSV_FILE, Csv(values.ToArray()) + Environment.NewLine);
                        }
                        Recovery.RecoveryValue = null;
                    }
                }
            }

            //print letters
            var line1 = Csv("BU", "DESCRIPTION", "COST", "STANDARD", "FOREIGN", "NUMPAGES", "COVERCOMMENT");
            var line2 = Csv("Operational Accounting", "Refund Checks", "MA4119", "", "", "", "");
            File.WriteAllLines(REFUND_COMMENT_COVER, new string[] { line1, line2 });
            DocumentProcessing.PrintDocs(COVER_LETTER_FOLDER, "Scripted State Mail Cover Sheet", REFUND_COMMENT_COVER);

            return true;
        }

        /// <summary>
        /// Show the manual form for dealing with entries that don't have an SSN.
        /// </summary>
        private bool NoSsnProcessing(Refund refundData)
        {
            //create and use form
            using (CompassRefundCommentFrm frm = new CompassRefundCommentFrm(refundData, UserId))
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
                else
                {
                    foreach (SSNAndRefundAmtCombo combo in frm.UserProvidedDataList)
                    {
                        string comment;
                        if (frm.VoidChecked)
                        {
                            comment = string.Format("void refund check for {0} effective {1}, check number {2} for {3} - Refund Amt for Borrower {4} - {5}", refundData.CheckRecipient, refundData.CheckDate,
                                      refundData.CheckNumber, refundData.CheckAmount, string.Format("{0:C}", double.Parse(combo.RefundAmount)), frm.VoidReason);
                        }
                        else
                        {
                            comment = string.Format("Send UHEAA Servicer Refund Check to {0} effective {1}, check number {2} for {3} - Refund Amt for Borrower {4}", refundData.CheckRecipient, refundData.CheckDate,
                                      refundData.CheckNumber, refundData.CheckAmount, string.Format("{0:C}", double.Parse(combo.RefundAmount)));
                        }

                        LeaveComment(combo.SSN, comment);
                    }

                    //get check voided value
                    string voided = (frm.VoidChecked ? "TRUE" : "FALSE");

                    //write record to spreadsheet file
                    File.AppendAllText(LOCAL_LSC_CHECK_REGISTER, Csv(refundData.CheckDate, refundData.CheckNumber, refundData.CheckRecipient, refundData.CheckAmount, voided) + Environment.NewLine);
                }
            }
            return true;
        }

        /// <summary>
        /// Attempt to leave a comment on Atd22, Atd37, or LP50.
        /// </summary>
        private void LeaveComment(string ssn, string comment)
        {
            if (!RI.Atd22AllLoans(ssn, ARC, comment, "", ScriptId, false))
                if (!RI.Atd37FirstLoan(ssn, ARC, comment, ScriptId, UserId))
                    RI.AddCommentInLP50(ssn, "MS", "80", ARC, comment, ScriptId);
        }

        /// <summary>
        /// Send success email if applicable.
        /// </summary>
        private void SendEmails()
        {
            //send email with spreadsheet files attached
            string currentUsersEmailAddress = string.Format("{0}@utahsbr.edu", Environment.UserName);
            string COMREFCOMSubject = string.Format("LPP Check Register for {0}", DateTime.Today.ToString("MM/dd/yyyy"));

            if (File.Exists(REFUND_FILE))
            {
                string COMREFCOMRecipients = string.Format("{0},{1}", currentUsersEmailAddress, EmailHelper.GetEmailRecipientString("COMREFCOM"));
                string body = string.Format("The LPP Refund Check Register is available on {0} {1} folder.", Environment.NewLine, LPP_REFUND_CSV_FILE);
                SendMail(COMREFCOMRecipients, string.Empty, COMREFCOMSubject, body, string.Empty, string.Empty, string.Empty, EmailImportance.Normal, true);
            }
        }

        /// <summary>
        /// Cleanup any existing files.
        /// </summary>
        private void CleanupFiles()
        {
            if (File.Exists(REFUND_COMMENT_COVER))
                File.Delete(REFUND_COMMENT_COVER);
            if (File.Exists(REFUND_FILE))
                File.Delete(REFUND_FILE);
            Recovery.RecoveryValue = null;
        }

        /// <summary>
        /// Join the given values and return a comma-delimited string.
        /// </summary>
        private string Csv(params string[] values)
        {
            return string.Join(",", values);
        }
    }
}
