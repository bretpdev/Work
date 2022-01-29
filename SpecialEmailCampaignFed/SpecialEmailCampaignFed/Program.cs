using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Q;

namespace SpecialEmailCampaignFed
{
    static class Program
    {
        private static EnterpriseFileSystem Efs;
        private static RecoveryLog Recovery;
        private static ReflectionInterface Ri;
        private static EndOfJobReport Eoj;
        private static string EojRecords = "Total number of Records in the file.";
        private static string EojEmailsSent = "Total Numbers of emails sent.";
        private static string EojErrors = "Total number of errors.";
        private static bool TestMode;

        //Main entry point for the script.
        [STAThread]
        static void Main()
        {
            TestMode = Environment.GetCommandLineArgs().Contains("test");
            Ri = new ReflectionInterface(TestMode, ReflectionInterface.Flag.None, ScriptSessionBase.Region.CornerStone);

            LoginData userInfo = new LoginData();
            using (Login login = new Login(userInfo))
            {
                while (true)
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        if (userInfo != null && !Ri.Login(userInfo.UserId, userInfo.Password, ScriptSessionBase.Region.CornerStone))
                        {
                            MessageBox.Show("You must put in a valid username and password to move forward", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Ri.Hit(ReflectionInterface.Key.Clear);
                            Ri.PutText(1, 2, "LOG", ReflectionInterface.Key.Enter);
                            continue;
                        }
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            Efs = new EnterpriseFileSystem(TestMode, ScriptSessionBase.Region.CornerStone);
            Recovery = new RecoveryLog(string.Format("{0}_{1}", "SpecialEmailCampaignFED", userInfo.UserId), Efs);
            Eoj = new EndOfJobReport(TestMode, "Special Email Campaign FED End Of Job Report", "EOJ_BU35", ScriptSessionBase.Region.CornerStone, new List<string>() { EojRecords, EojEmailsSent, EojErrors });

            Start();

            Ri.CloseSession();
        }

        public static void Start()
        {
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                DialogResult result = MessageBox.Show("The application has detected that it is in recovery.  Do you want to continue processing from Recovery?", "Recovery", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    List<string> recoveryData = Recovery.RecoveryValue.SplitAgnosticOfQuotes(",");
                    CampaignData cData = new CampaignData() { CampID = long.Parse(recoveryData[2]), CornerStone = recoveryData[3].ToLower().Contains("true"), EmailSubjectLine = recoveryData[4], Arc = recoveryData[5], CommentText = recoveryData[6], DataFile = recoveryData[7], HTMLFile = recoveryData[8], EmailFrom = recoveryData[9] };
                    Run(cData, cData.CornerStone);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    Recovery.Delete();
                }
            }

            using (frmChooseCampaign frmChoose = new frmChooseCampaign(GetEmailCampaign(), TestMode))
            {
                if (frmChoose.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
            }

        }

        public static void Run(CampaignData data, bool addArc)
        {
            if (MessageBox.Show("Are you sure you want to run the selcted campaign?", "Email Campaign", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            using (StreamReader fileRead = new StreamReader(data.HTMLFile))
            {
                frmProcessing frmProcessing = new frmProcessing();
                Thread processingThread = new Thread(new ThreadStart(DisplayProcessing)) { IsBackground = true, };
                processingThread.Start();
                string emailFileData = fileRead.ReadToEnd();//read in the email

                ErrorReport err = new ErrorReport(TestMode, "Special Email Campaign FED Error Report", "ERR_BU35", ScriptSessionBase.Region.CornerStone);//create error report object

                IEnumerable<BorrowerDetails> fileData = ReadFile(data.DataFile, data.IncludeAccountNumber);//read in the file data

                if (fileData == null)
                    return;

                for (int recoveryCounter = string.IsNullOrEmpty(Recovery.RecoveryValue) ? 0 : GetRecoveryRow(); recoveryCounter < fileData.Count(); )//Recovery
                {
                    BorrowerDetails dFile = fileData.ElementAt(recoveryCounter);
                    DataAccess da = new DataAccess(TestMode);
                    long recipId = da.CreateAndReturnRecipId(dFile, data);

                    string emailRecipId = recipId.ToString();
                    string emailBody = emailFileData;
                    if (emailBody.Contains("[[[Name]]]")) { emailBody = emailBody.Replace("[[[Name]]]", dFile.BorrowerName); }
                    if (emailBody.Contains("[[[RecipId]]]")) { emailBody = emailBody.Replace("[[[RecipId]]]", emailRecipId); }
                    if (data.IncludeAccountNumber)
                        emailBody = emailBody.Replace("[[[Account Number]]]", dFile.AccountNumber);

                    if (string.IsNullOrEmpty(Recovery.RecoveryValue) || !Recovery.RecoveryValue.SplitAgnosticOfQuotes(",")[1].Contains("Email"))
                    {
                        try
                        {
                            Common.SendMailBatchEmail(TestMode, dFile.EmailAddress, data.EmailFrom, data.EmailSubjectLine, emailBody, "", "", "", Common.EmailImportanceLevel.Normal, TestMode, true);
                        }
                        catch (Exception ex)
                        {
                            Eoj.Counts[EojErrors].Increment();
                            continue;
                        }

                        Recovery.RecoveryValue = string.Format("{0},Email,{1},{2},{3},{4},{5},{6},{7},{8}", recoveryCounter, data.CampID, data.CornerStone, data.EmailSubjectLine, data.Arc, data.CommentText, data.DataFile, data.HTMLFile, data.EmailFrom);
                        Eoj.Counts[EojEmailsSent].Increment();
                    }

                    int emailMessagesSent = 0;

                    if (addArc && !Common.ATD22ByBalance(Ri, dFile.AccountNumber, data.Arc, data.CommentText, string.Empty, false))
                    {
                        err.AddRecord("The following ARC could not be added to the following Account", new { dFile.AccountNumber, data.Arc, data.CommentText });
                        Eoj.Counts[EojErrors].Increment();
                    }

                    recoveryCounter++;
                    Recovery.RecoveryValue = string.Format("{0},Arc,{1},{2},{3},{4},{5},{6},{7},{8}", recoveryCounter, data.CampID, data.CornerStone, data.EmailSubjectLine, data.Arc, data.CommentText, data.DataFile, data.HTMLFile, data.EmailFrom);

                    emailMessagesSent++;
                    if (emailMessagesSent % 5000 == 0)
                    {
                        Thread.Sleep(60000);//sleep for 1 minute 
                        emailMessagesSent = 0;//reset
                    }
                }

                processingThread.Abort();// end the processing thread
                Recovery.Delete();//Delete recovery
                err.Publish();//publish error report if it exsists
                Eoj.Publish();//publish the End of Jon Report

                MessageBox.Show("Processing Complete", "Processing Complete", MessageBoxButtons.OK);
            }
        }

        public static void TestRun(CampaignData data, bool addArc)
        {
            BorrowerDetails bData = new BorrowerDetails();
            TestAcctNum tstAcctNumFrm = new TestAcctNum(bData);

            if (MessageBox.Show("Are you sure you want to run the selcted campaign im Test?  ", " Test Email Campaign", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            else if (tstAcctNumFrm.ShowDialog() != DialogResult.OK) { return; }
            else
            {
                ReflectionInterface ri = new ReflectionInterface(true);
                LoginData userInfo = new LoginData();

                using (Login login = new Login(userInfo))
                {
                    while (true)
                    {
                        if (login.ShowDialog() == DialogResult.OK)
                        {
                            if (userInfo != null && !ri.Login(userInfo.UserId, userInfo.Password, ScriptSessionBase.Region.CornerStone))
                            {
                                MessageBox.Show("You must put in a valid username and password to move forward", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ri.Hit(ReflectionInterface.Key.Clear);
                                ri.PutText(1, 2, "LOG", ReflectionInterface.Key.Enter);
                                continue;
                            }
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                using (StreamReader fileRead = new StreamReader(data.HTMLFile))
                {
                    string emailBody = fileRead.ReadToEnd();
                    DataAccess da = new DataAccess(true);
                    string emailRecipId = "1";//If running test the email recipId will always be 1.  

                    if (emailBody.Contains("[[[Name]]]")) { emailBody = emailBody.Replace("[[[Name]]]", Common.WindowsUserName()); }
                    if (emailBody.Contains("[[[RecipId]]]")) { emailBody = emailBody.Replace("[[[RecipId]]]", emailRecipId); }

                    string testEmail = string.Concat(Common.WindowsUserName(), "@utahsbr.edu");
                    string ssn = da.GetSsnFromAcctNum(bData.AccountNumber).ToString();

                    Common.SendMail(TestMode, testEmail, data.EmailFrom, data.EmailSubjectLine, emailBody, "", "", "", Common.EmailImportanceLevel.Normal, true, true);

                    if (addArc)
                    {
                        Common.ATD22ByBalance(ri, ssn, data.Arc, data.CommentText, string.Empty, false);
                    }

                    ri.CloseSession();
                }//end using
            }//end else
        }//end TestRun

        private static IEnumerable<CampaignData> GetEmailCampaign()
        {
            return new DataAccess(TestMode).GetEmailCampigns();
        }

        public static List<BorrowerDetails> ReadFile(string file, bool includeAccountNumber)
        {
            List<BorrowerDetails> fileData = new List<BorrowerDetails>();
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//read out the ehad row
                int lineNumber = 1;
                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAgnosticOfQuotes(",");

                    if (includeAccountNumber)
                    {
                        switch (temp[0].Length)
                        {
                            case 0:
                                break;
                            case 10:
                                break;
                            default:
                                MessageBox.Show(string.Format("The file you choose has an invalid account number on line {0}.  Please review and try again.", lineNumber));
                                return null;
                        }
                    }

                    fileData.Add(new BorrowerDetails() { AccountNumber = temp[0], BorrowerName = temp[1], EmailAddress = temp[2] });
                    Eoj.Counts[EojRecords].Increment();
                    lineNumber++;
                }
            }

            return fileData;
        }

        public static int GetRecoveryRow()
        {
            int row = 1;
            if (Recovery.RecoveryValue.Length > 0)
            {
                if (!Int32.TryParse(Recovery.RecoveryValue.Split(',')[0], out row))
                {
                    throw new Exception("The script is in recovery, but the first value in the recovery log is not a row number.");
                }
            }
            return row;
        }

        public static void DisplayProcessing()
        {
            frmProcessing processing = new frmProcessing();
            processing.ShowDialog();
            processing.Refresh();
        }
    }
}