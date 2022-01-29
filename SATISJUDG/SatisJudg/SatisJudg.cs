using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;
using System.IO;
using Reflection;

namespace SatisJudg
{
    public class SatisJudg:ScriptBase
    {
        private TestModeResults _dirResults;
        private FilesToPrintFlags _flags;
        private Dictionary<string,string> CountyCourts = new Dictionary<string,string>();
        const string DOC_PATH = @"X:\PADD\Legal\";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri">Reflection Interface</param>
        public SatisJudg(ReflectionInterface ri)
            : base(ri, "SATISJUDG")
        {
            PopulateCountyCourtsDictionary();
        }

        /// <summary>
        /// work tasks in the LSATISFY queue to create satisfaction of judgment documents for satisfied legal accounts
        /// </summary>
        public override void Main()
        {
            //tell the user what the script will do and end the script if the dialog box is canceled
            if (System.Windows.Forms.MessageBox.Show("This script will work tasks in the LSATISFY queue to create satisfaction of judgment documents.  Click OK to proceed or Cancel to quit.","Satisfaction of Judgment",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) != DialogResult.OK)
            {
                return;
            }

            _dirResults = TestMode(DOC_PATH);
            _flags = new FilesToPrintFlags();

            //get the task
            //access LP9A
            FastPath("LP9ACLSATISFY");
            //warn the user and end the script if there are no tasks
            if (Check4Text(22, 3, "47423", "47420"))
            {
                MessageBox.Show("There are no more tasks in the LSATISFY queue.  Processing is complete."); 
                return;
            }
            //warn the user and end the script is a task for a queue other than LSATSIFY is displayed
            if (Check4Text(1, 9, "LSATISFY") == false)
            {
                MessageBox.Show(string.Format("You have an unresolved task in the {0} queue.  You must complete the task before working the LSATISFY queue.",GetText(1, 9, 8)));
                return;
            }
            if (File.Exists(DataAccessBase.PersonalDataDirectory + "jdgdat.txt") == false) 
            {
                //create sec merge file for satisfaction of judgment with header row if file doesn't exist
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "jdgdat.txt", 2, Common.MSOpenMode.Output);
                VbaStyleFileWriteLine(2,"SSN", "Borrower", "AKA", "Address1", "Address2", "City", "State", "ZIP", "DockNo", "Judge", "FiledDate", "SatisDate", "SatisReason"); 
                //create sec merge file for satisfaction of abstract with header row
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "absdat.txt", 3, Common.MSOpenMode.Output);
                VbaStyleFileWriteLine(3, "SSN", "Borrower", "AKA", "Address1", "Address2", "City", "State", "ZIP", "DockNo", "FiledDate", "AbstractNo", "County", "Court", "SatisDate", "SatisReason");
                //create sec merge file for fee request with header row
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "feedat.txt", 4, Common.MSOpenMode.Output);
                VbaStyleFileWriteLine(4, "SSN", "Borrower", "AKA", "DockNo", "Judge", "Amount", "Balance", "User");
                //create sec merge file for satisfaction of judgment/rehab with header row
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "jdgrhdat.txt", 5, Common.MSOpenMode.Output);
                VbaStyleFileWriteLine(5,"SSN", "Borrower", "AKA", "Address1", "Address2", "City", "State", "ZIP", "DockNo", "Judge");
                //create sec merge file for satisfaction of abstract/rehab with header row
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "absrhdat.txt", 6, Common.MSOpenMode.Output);
                VbaStyleFileWriteLine(6, "SSN", "Borrower", "AKA", "Address1", "Address2", "City", "State", "ZIP", "DockNo", "FiledDate", "AbstractNo", "County", "Court", "SatisDate", "SatisRea");
            }
            else
            {
                //simply reopen the files if they already exist
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "jdgdat.txt", 2, Common.MSOpenMode.Append);
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "absdat.txt", 3, Common.MSOpenMode.Append);
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "feedat.txt", 4, Common.MSOpenMode.Append);
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "jdgrhdat.txt", 5, Common.MSOpenMode.Append);
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "absrhdat.txt", 6, Common.MSOpenMode.Append);
            }
            RecHand();
            Finish(); //finish the script if no more tasks are found
        }

        //process each task
        private void RecHand()
        {
            QueueTaskProcInfo procInfo;
            bool judgmentWasProcessed;
            bool multipleRecsFound;
            bool judgmentProcComplete;
            while (Check4Text(22, 3, "46004") == false)
            {
                procInfo = new QueueTaskProcInfo(GetText(17, 70, 9),GetText(17, 6, 35));
                if (MessageBox.Show("The script will pause for you to examine the task.  Hit Insert when you are ready to resume processing","Hit Insert to Continue",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) != DialogResult.OK) 
                {
                    return;
                }

                RS.WaitForTerminalKey((int) Reflection.Constants.rcIBMInsertKey, "1:0:0");
                //go to LP22 to get the alternate billing name to be used as the AKA
                procInfo.BrwDemos = GetDemographicsFromLP22(procInfo.SSN);
                procInfo.AKA = Common.AKA(RS,procInfo.SSN,procInfo.Name);
                //go to LC05 to determine the satisfaction date and reason
                LC05(procInfo);
                //get the judge's name
                procInfo.Judge = CommonGarnishment.GetJudgesNameFromDJGNMActivityRecord(RI,procInfo.SSN);
                //set judgment proc loop flags
                judgmentWasProcessed = false;
                multipleRecsFound = false;
                judgmentProcComplete = false;
                //get information about the judgement
                while (judgmentWasProcessed == false)
                {
                    JudgInfo(procInfo, ref judgmentWasProcessed, ref multipleRecsFound, ref judgmentProcComplete);
                    if (judgmentWasProcessed && multipleRecsFound)
                    {
                        //if at least one judgement was processed and mutliple records where found then loop until the user says that the judgment processing is complete
                        while (judgmentProcComplete == false)
                        {
                            JudgInfo(procInfo, ref judgmentWasProcessed, ref multipleRecsFound, ref judgmentProcComplete);
                        }
                        break;
                    }
                }
                //access LP9A
                FastPath("LP9ACLSATISFY");
                //complete the task
                Hit(ReflectionInterface.Key.F6);
                //go to the next task
                Hit(ReflectionInterface.Key.F8);
            }
        }

        //go to LC05 to determine the satisfaction date and reason
        private void LC05(QueueTaskProcInfo procInfo)
        {
            FastPath("LC05I" + procInfo.SSN);
            procInfo.Balance = double.Parse(GetText(3, 69, 12));
            //access the first loan
            PutText(21, 13, "01", ReflectionInterface.Key.Enter);
            //compare the status date to the status date for the previous loan and get the status date and code if it is more recent
            while (Check4Text(22, 3, "46004") == false)
            {
                if (procInfo.SatisDate == string.Empty)
                {
                    procInfo.SatisDate = GetText(5, 13, 8).ToDateFormat();
                    procInfo.SatisCd = GetText(4, 26, 2);
                }
                else if ((DateTime.Parse(GetText(5, 13, 8).ToDateFormat())) > DateTime.Parse(procInfo.SatisDate))
                {
                    procInfo.SatisDate = GetText(5, 13, 8).ToDateFormat();
                    procInfo.SatisCd = GetText(4, 26, 2);
                }
                Hit(ReflectionInterface.Key.F8);
            }
            //set values based on how the more recently satisfied loan was satisfied
            if (procInfo.SatisCd == "  " || procInfo.SatisCd == "01" || procInfo.SatisCd == "05")
            {
                procInfo.SatisRea = string.Format("the same was fully paid and satisfied on {0}",DateTime.Parse(procInfo.SatisDate).ToString("MMMM, d, yyyy"));
                procInfo.InactRea = "01";
            }
            else if (procInfo.SatisCd == "02")
            {
                procInfo.SatisRea = string.Format("the cancellation of the debt on {0} due to the death of the defendant",DateTime.Parse(procInfo.SatisDate).ToString("MMMM, d, yyyy"));
                procInfo.InactRea = "09";
            }
            else if (procInfo.SatisCd == "03")
            {
                procInfo.SatisRea = string.Format("the cancellation of the debt on {0} due to the disability of the defendant",DateTime.Parse(procInfo.SatisDate).ToString("MMMM, d, yyyy"));
                procInfo.InactRea = "09";
            }
            else if (procInfo.SatisCd == "07")
            {
                procInfo.SatisRea = string.Format("the discharge of the debt in bankruptcy on {0}",DateTime.Parse(procInfo.SatisDate).ToString("MMMM, d, yyyy"));
                procInfo.InactRea = "05";
            }
            else if (procInfo.SatisCd == "08")
            {
                procInfo.SatisRea = string.Format("the discharge of the debt on {0} due to school closure",DateTime.Parse(procInfo.SatisDate).ToString("MMMM, d, yyyy"));
                procInfo.InactRea = "09";
            }
            else if (procInfo.SatisCd == "09")
            {
                procInfo.SatisRea = string.Format("the discharge of the debt on {0} due to false certification of the defendant's eligibility for the debt",DateTime.Parse(procInfo.SatisDate).ToString("MMMM, d, yyyy"));
                procInfo.InactRea = "09";
            }
            else if (procInfo.SatisCd == "10")
            {
                procInfo.InactRea = "07";
            }
            else if (procInfo.SatisCd == "11" || procInfo.SatisCd == "12")
            {
                procInfo.SatisRea = string.Format("the same was fully paid and satisfied through consolidation on {0}", DateTime.Parse(procInfo.SatisDate).ToString("MMMM, d, yyyy"));
                procInfo.InactRea = "06";
            }
        }

        //get information about the judgement
        private void JudgInfo(QueueTaskProcInfo procInfo, ref bool judgmentWasProcessed, ref bool multipleRecsFound, ref bool judgmentProcComplete)
        {
            SatJdg satJdgForm; 
            //access LC67
            FastPath(string.Format("LC67C{0};SC",procInfo.SSN));
            //prompt the user to select the correct record if more than one is displayed
            if (Check4Text(21, 3, "SEL"))
            {
                DialogResult warnMultipleRecords;
                do
                {
                    warnMultipleRecords = MessageBox.Show("The borrower has more than one Summons and Complaint record.  Click OK to review the records and access the SUMMONS & COMPLAINT DISPLAY screen for the record to process (hit Insert to continue once the record has been selected).  Click Cancel if all of the records have been processed.","Multiple S&C Records",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                    if (warnMultipleRecords != DialogResult.OK && multipleRecsFound)
                    {
                        judgmentProcComplete = true; //multiple records where found and at least one has been processed
                        return;
                    }
                    else if (warnMultipleRecords != DialogResult.OK)
                    {
                        MessageBox.Show("No records have been processed.","No Records Processed",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return; //code will loop back into this method because no records were processed (judgmentWasProcessed is still = false)
                    }
                    else
                    {
                        RS.WaitForTerminalKey((int)Reflection.Constants.rcIBMInsertKey, "1:0:0");
                        RS.TransmitTerminalKey((int)Reflection.Constants.rcIBMInsertKey);
                    }
                } while (Check4Text(1, 54, "SUM") == false);
                if (Check4Text(1, 54, "SUM"))
                {
                    DialogResult warnConfirmRecordSelection = MessageBox.Show("Click OK to process this record or Cancel if all of the records have been processed.","Process Record",MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (warnConfirmRecordSelection != DialogResult.OK && multipleRecsFound)
                    {
                        judgmentProcComplete = true; //multiple records where found and at least one has been processed
                        return;
                    }
                    else if (warnConfirmRecordSelection != DialogResult.OK)
                    {
                        MessageBox.Show("No records have been processed.","No Records Processed",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return; //code will loop back into this method because no records were processed (judgmentWasProcessed is still = false)
                    }
                }
                multipleRecsFound = true;
            }
            //update the inactive date and reason
            if (Check4Text(8, 19, "__") && Check4Text(15, 63, "__"))
            {
                PutText(14, 71, DateTime.Parse(procInfo.SatisDate).ToString("MMddyyyy"));
                PutText(15, 63, procInfo.InactRea, ReflectionInterface.Key.Enter);
            }
            //access the judgment record
            PutText(1, 19,"JD", ReflectionInterface.Key.Enter);            
            //prompt the user to select the correct record if more than one is displayed 
            if (Check4Text(21, 3, "SEL"))
            {
                DialogResult warnMultipleJudgmentRecords;
                do
                {
                    warnMultipleJudgmentRecords = MessageBox.Show("The system has displayed more than one legal record.  Click OK to review the records and access the JUDGMENT DISPLAY screen for the judgment record to process (hit Insert to continue once the record has been selected).  Click Cancel if all of the records have been processed.","Multiple Legal Records",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                    if (warnMultipleJudgmentRecords != DialogResult.OK && multipleRecsFound)
                    {
                        judgmentProcComplete = true; //multiple records where found and at least one has been processed
                        return;
                    }
                    else if (warnMultipleJudgmentRecords != DialogResult.OK)
                    {
                        MessageBox.Show("No records have been processed.","No Records Processed",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return; //code will loop back into this method because no records were processed (judgmentWasProcessed is still = false)
                    }
                    else
                    {
                        RS.WaitForTerminalKey((int)Reflection.Constants.rcIBMInsertKey, "1:0:0");
                        RS.TransmitTerminalKey((int)Reflection.Constants.rcIBMInsertKey);
                    }
                } while (Check4Text(1, 65, "JUD") == false);
                if (Check4Text(1, 65, "JUD"))
                {
                    DialogResult warnConfirmRecordSelection = MessageBox.Show("Click OK to process this record or Cancel if all of the records have been processed.","Process Record",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                    if (warnConfirmRecordSelection != DialogResult.OK && multipleRecsFound)
                    {
                        judgmentProcComplete = true; //multiple records where found and at least one has been processed
                        return;
                    }
                    else if (warnConfirmRecordSelection != DialogResult.OK)
                    {
                        MessageBox.Show("No records have been processed.","No Records Processed",MessageBoxButtons.OK,MessageBoxIcon.Question);
                        return; //code will loop back into this method because no records were processed (judgmentWasProcessed is still = false)
                    }
                }
                multipleRecsFound = true;
            }                                                                    
            judgmentWasProcessed = true; //setting this flag stops the code from looping back into this method
                    
            //get the case number and filed date
            procInfo.DockNo = GetText(5, 66, 15);
            procInfo.FiledDate = GetText(5, 26, 8).ToDateFormat();
            //query out keys from CountyCourts dictionary
            List<string> Counties =
                (from kys in CountyCourts
                 orderby kys.Key
                select kys.Key).ToList();
            //prompt user for complaint date, complaint amount, abstract number, and abstract filing county
            while (true)
            {

                satJdgForm = new SatJdg(Counties, procInfo);
                satJdgForm.ShowDialog();
                //warn the user if the complaint date and complaint amount are not filled in
                if (procInfo.ComplDate == string.Empty || procInfo.ComplAmt == 0)
                {
                    if (MessageBox.Show("A check request to pay filing fees cannot be generated without the complaint date and complaint amount.  Click OK to enter the missing information or click Cancel to continue processing without generating a check request.","Missing Complaint Information",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) != DialogResult.OK)
                    {
                        procInfo.FeeAmt = 0;
                        break;
                    }
                }
                else
                {
                    if (procInfo.ComplDate.IndexOf("/") == -1) 
                    {
                        procInfo.ComplDate = procInfo.ComplDate.ToDateFormat();
                    }
                    //calculate filing fee
                    if (DateTime.Parse(procInfo.ComplDate) < DateTime.Parse("07/01/1992"))
                    {
                        if (procInfo.ComplAmt < 2000) 
                        {
                            procInfo.FeeAmt = 15; 
                        }
                        else
                        {
                            procInfo.FeeAmt = 35;
                        }
                    }
                    else if (DateTime.Parse(procInfo.ComplDate) < DateTime.Parse("04/16/1994"))
                    {
                        if (procInfo.ComplAmt < 2000) 
                        {   
                            procInfo.FeeAmt = 20;
                        }
                        else if (procInfo.ComplAmt < 10000)
                        {
                            procInfo.FeeAmt = 40;
                        }
                        else 
                        {
                            procInfo.FeeAmt = 80;
                        }
                    }
                    else if (DateTime.Parse(procInfo.ComplDate) < DateTime.Parse("05/01/1995"))
                    {
                        if (procInfo.ComplAmt < 2000)
                        {
                            procInfo.FeeAmt = 25;
                        }
                        else if (procInfo.ComplAmt < 10000)
                        {
                            procInfo.FeeAmt = 60;
                        }
                        else
                        {
                            procInfo.FeeAmt = 100;
                        }
                    }
                    else if (DateTime.Parse(procInfo.ComplDate) < DateTime.Parse("01/01/2000"))
                    {
                        if (procInfo.ComplAmt < 2000)
                        {
                            procInfo.FeeAmt = 37;
                        }
                        else if (procInfo.ComplAmt < 10000)
                        {
                            procInfo.FeeAmt = 80;
                        }
                        else
                        {
                            procInfo.FeeAmt = 120;
                        }
                    }
                    else
                    {
                        if (procInfo.ComplAmt < 2000) 
                        {
                            procInfo.FeeAmt = 15;
                        }
                        else
                        {
                            procInfo.FeeAmt = 35;
                        }
                    }
                    break;
                }
            }

            //determine court
            if (procInfo.County != string.Empty)
            {
                procInfo.Court = CountyCourts[procInfo.County];
            }
            else
            {
                procInfo.Court = null;
            }
           
            //write information to files, set comments, and set print indicators based on how the account was satisfied
            if (procInfo.SatisCd == "10")
            {
                VbaStyleFileWriteLine(5, procInfo.SSN.ToSSNFormat(), procInfo.Name, procInfo.AKA, procInfo.BrwDemos.Addr1, procInfo.BrwDemos.Addr2, procInfo.BrwDemos.City, procInfo.BrwDemos.State, procInfo.BrwDemos.Zip, procInfo.DockNo, procInfo.Judge);
                _flags.RehabInd = true;
            }
            else
            {
                VbaStyleFileWriteLine(2, procInfo.SSN.ToSSNFormat(), procInfo.Name, procInfo.AKA, procInfo.BrwDemos.Addr1, procInfo.BrwDemos.Addr2, procInfo.BrwDemos.City, procInfo.BrwDemos.State, procInfo.BrwDemos.Zip, procInfo.DockNo, procInfo.Judge, DateTime.Parse(procInfo.FiledDate).ToString("MMMM d, yyyy"), DateTime.Parse(procInfo.SatisDate).ToString("MMMM d, yyyy"), procInfo.SatisRea);
                _flags.JudgInd = true;
            }
            string comment = "satisfaction of judgment to attorney to sign";
            if (procInfo.AbstractNo != string.Empty)
            {
                if (procInfo.SatisCd == "10")
                {
                    VbaStyleFileWriteLine(6, procInfo.SSN.ToSSNFormat(), procInfo.Name, procInfo.AKA, procInfo.BrwDemos.Addr1, procInfo.BrwDemos.Addr2, procInfo.BrwDemos.City, procInfo.BrwDemos.State, procInfo.BrwDemos.Zip, procInfo.DockNo, DateTime.Parse(procInfo.FiledDate).ToString("MMMM d, yyyy"), procInfo.AbstractNo, procInfo.County, procInfo.Court, DateTime.Parse(procInfo.SatisDate).ToString("MMMM d, yyyy"), procInfo.SatisRea);
                    _flags.AbsrhInd = true;
                }
                else
                {
                    VbaStyleFileWriteLine(3, procInfo.SSN.ToSSNFormat(), procInfo.Name, procInfo.AKA, procInfo.BrwDemos.Addr1, procInfo.BrwDemos.Addr2, procInfo.BrwDemos.City, procInfo.BrwDemos.State, procInfo.BrwDemos.Zip, procInfo.DockNo, DateTime.Parse(procInfo.FiledDate).ToString("MMMM d, yyyy"), procInfo.AbstractNo, procInfo.County, procInfo.Court, DateTime.Parse(procInfo.SatisDate).ToString("MMMM d, yyyy"), procInfo.SatisRea);
                    _flags.AbstInd = true;
                }
                comment = "satisfaction of judgment and abstract to attorney to sign";
            }
            if (procInfo.FeeAmt != 0)
            {
                VbaStyleFileWriteLine(4, procInfo.SSN.ToSSNFormat(), procInfo.Name, procInfo.AKA, procInfo.DockNo, procInfo.Judge, procInfo.FeeAmt.ToString("0.00"), procInfo.Balance.ToString("0.00"), DataAccess.GetUsersName(TestModeProperty));
                comment = string.Format("{0}, check request generated for court fees of ${1}", comment, procInfo.FeeAmt.ToString("0.00"));
                _flags.FeeInd = true;
            }
            //add an activity record
            AddCommentInLP50(procInfo.SSN,"LJDSF","CD","33",comment,false,false);
        }

        //close input files and print letters
        private void Finish()
        {
            //close the input files
            VbaStyleFileClose(2,3,4,5,6);
            //print the letters required
            if (_flags.JudgInd)
            {
                DocumentHandling.PrintDocs(DOC_PATH,"SATJUDG",DataAccessBase.PersonalDataDirectory + "jdgdat.txt");
            }
            if (_flags.RehabInd)
            {
                DocumentHandling.PrintDocs(DOC_PATH, "SATRHJUDG", DataAccessBase.PersonalDataDirectory + "jdgrhdat.txt");
            }
            if (_flags.AbstInd)
            {
                DocumentHandling.PrintDocs(DOC_PATH, "SATABST", DataAccessBase.PersonalDataDirectory + "absdat.txt");
            }
            if (_flags.AbsrhInd)
            {
                DocumentHandling.PrintDocs(DOC_PATH, "SATRHABST", DataAccessBase.PersonalDataDirectory + "absrhdat.txt");
            }
            if (_flags.FeeInd)
            {
                DocumentHandling.PrintDocs(DOC_PATH, "FEEREQ", DataAccessBase.PersonalDataDirectory + "feedat.txt");
            }
            //delete all data files
            File.Delete(DataAccessBase.PersonalDataDirectory + "jdgdat.txt");
            File.Delete(DataAccessBase.PersonalDataDirectory + "jdgrhdat.txt");
            File.Delete(DataAccessBase.PersonalDataDirectory + "absdat.txt");
            File.Delete(DataAccessBase.PersonalDataDirectory + "absrhdat.txt");
            File.Delete(DataAccessBase.PersonalDataDirectory + "feedat.txt");
        }

        private void PopulateCountyCourtsDictionary()
        {
            CountyCourts.Add("BOX ELDER","FIRST");
            CountyCourts.Add("CACHE","FIRST");
            CountyCourts.Add("RICH","FIRST");
            CountyCourts.Add("DAVIS","SECOND");
            CountyCourts.Add("MORGAN","SECOND");
            CountyCourts.Add("WEBER","SECOND");
            CountyCourts.Add("SALT LAKE","THIRD");
            CountyCourts.Add("SUMMIT","THIRD");
            CountyCourts.Add("TOOELE","THIRD");
            CountyCourts.Add("JUAB","FOURTH");
            CountyCourts.Add("MILLARD","FOURTH");
            CountyCourts.Add("UTAH","FOURTH");
            CountyCourts.Add("WASATCH","FOURTH");
            CountyCourts.Add("BEAVER","FIFTH");
            CountyCourts.Add("IRON","FIFTH");
            CountyCourts.Add("WASHINGTON","FIFTH");
            CountyCourts.Add("GARFIELD","SIXTH");
            CountyCourts.Add("KANE","SIXTH");
            CountyCourts.Add("PIUTE","SIXTH");
            CountyCourts.Add("SANPETE","SIXTH");
            CountyCourts.Add("SEVIER","SIXTH");
            CountyCourts.Add("WAYNE","SIXTH");
            CountyCourts.Add("CARBON","SEVENTH");
            CountyCourts.Add("EMERY","SEVENTH");
            CountyCourts.Add("GRAND","SEVENTH");
            CountyCourts.Add("SAN JUAN","SEVENTH");
            CountyCourts.Add("DAGGETT","EIGHTH");
            CountyCourts.Add("DUCHESNE","EIGHTH");
            CountyCourts.Add("UINTAH","EIGHTH");
        }
    }
}
