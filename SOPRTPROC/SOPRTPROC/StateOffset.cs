using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace SOPRTPROC
{
    public class StateOffset : ScriptBase
    {
        readonly BorrowerDemographic Demo = new BorrowerDemographic();
        readonly List<string> ClaimIds = new List<string>();

        public StateOffset(ReflectionInterface ri)
            : base(ri, "SOPRTPROC", DataAccessHelper.Region.Uheaa)
        { }
        public override void Main()
        {
            StateOffsetForm sos = new StateOffsetForm(this, Demo);
            sos.ShowDialog();
        }

        /// <summary>
        /// Pulls borrower demographic information from LP22
        /// </summary>
        /// <param name="ssn"></param>
        public void LoadBorrowerDemo(string ssn)
        {
            string path = ssn.Length == 9 ? "LP22I" + ssn : "LP22I;;;;L;;" + ssn;
            RI.FastPath(path);
            if (RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
            {
                Demo.Address1 = RI.GetText(11, 9, 35);
                Demo.Address2 = RI.GetText(12, 9, 35);
                Demo.City = RI.GetText(13, 9, 35);
                Demo.State = RI.GetText(13, 52, 2);
                Demo.Zip = RI.GetText(13, 60, 9);
            }
            else
            {
                Demo.Address1 = "";
                Demo.Address2 = "";
                Demo.City = "";
                Demo.State = "";
                Demo.Zip = "";
            }
        }

        /// <summary>
        /// Uses the Account Number to get the SSN
        /// </summary>
        public void GetSSN()
        {
            RI.FastPath("LP22I;;;");
            RI.PutText(6, 33, Demo.AccountNumber);
            Demo.SSN = RI.GetText(3, 34, 9);
            Demo.BorrowerName = RI.GetText(4, 44, 12) + " " + RI.GetText(4, 5, 35);
        }

        /// <summary>
        /// Uses the SSN to get the account number
        /// </summary>
        public void GetAccountNumber()
        {
            RI.FastPath("LP22I" + Demo.SSN);
            Demo.AccountNumber = RI.GetText(3, 60, 12).Replace(" ", "");
            Demo.BorrowerName = RI.GetText(4, 44, 12) + " " + RI.GetText(4, 5, 35);
        }

        /// <summary>
        /// Go to LP50I and check if a batch already exists
        /// </summary>
        /// <param name="demo"></param>
        public bool CheckIfBatchExists(BorrowerDemographic demo)
        {
            RI.FastPath("LP50I" + demo.SSN);
            RI.PutText(9, 20, "DDSOF");
            RI.PutText(18, 29, DateTime.Now.ToString("MMddyyyy"));
            RI.PutText(18, 41, DateTime.Now.ToString("MMddyyyy"), ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(1, 68, "ACTIVITY MENU"))
            {
                MessageBox.Show("Script has already been run for this borrower today. Please wait until tomorrow to run the script.");
                return false;
            }

            CheckBatch(demo.SSN); //Check for active SO batches
            return true;
        }

        /// <summary>
        /// Check to see if there are any active SO batches for SSN
        /// </summary>
        /// <param name="ssn"></param>
        private void CheckBatch(string ssn)
        {
            string date = DateTime.Now.ToString("MMddyyyy");
            RI.FastPath("LC90I" + date + ";" + date);
            if (RI.CheckForText(1, 71, "BATCH MENU")) //No batches today
                return;
            else if (RI.CheckForText(1, 66, "BATCH SELECTION"))
            {
                int row = 7;
                while (!RI.CheckForText(22, 3, "46004"))
                    if (RI.CheckForText(row, 56, "SO") && !RI.CheckForText(row, 61, "INACTIVE"))
                    {
                        while (!RI.CheckForText(22, 3, "46004")) //Search all SO batches for SSN
                            if (RI.CheckForText(row, 56, "SO") && !RI.CheckForText(row, 61, "INACTIVE")) //Batch found
                            {
                                RI.PutText(21, 13, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);
                                TargetScreen(ssn);
                            }
                        //Go back to the original page
                        RI.Hit(ReflectionInterface.Key.F12);
                        RI.Hit(ReflectionInterface.Key.F12);

                        row++;
                        if (RI.CheckForText(row, 3, " "))
                        {
                            row = 7;
                            RI.Hit(ReflectionInterface.Key.F8);
                        }
                    }
            }
            else
                TargetScreen(ssn);
        }

        /// <summary>
        /// Check if there is an active SO batch for SSN
        /// </summary>
        /// <param name="ssn"></param>
        private void TargetScreen(string ssn)
        {
            if (RI.CheckForText(10, 18, "SO") && !RI.CheckForText(14, 18, "INACTIVE"))
            {
                RI.Hit(ReflectionInterface.Key.F10);
                int row = 9;
                while (!RI.CheckForText(22, 3, "46004"))
                    if (RI.GetText(row, 2, 11).Replace(" ", "") == ssn)
                    {
                        MessageBox.Show("Borrower has a pending SO payment please wait until tomorrow to run the script.");
                        EndDllScript();
                    }
            }
        }

        /// <summary>
        /// Checks the borrower account to make sure the SSN is correct
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public bool CheckLC05(string ssn)
        {
            RI.FastPath("LC05I" + ssn);
            if (RI.CheckForText(1, 60, "DEFAULT/CLAIM DISPLAY"))
            {
                MessageBox.Show("LC05 wasn't displayed for that SSN. Please check the number and try again", "Invalid SSN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if (RI.CheckForText(1, 70, "CLAIM RECAP"))
                    RI.PutText(21, 13, "1", ReflectionInterface.Key.Enter);
                CheckIfLoansArePaid();
            }
            return true;
        }

        /// <summary>
        /// Loop through each loan and check if they have been paid in full.
        /// </summary>
        private void CheckIfLoansArePaid()
        {
            bool process = false;
            while (!RI.CheckForText(22, 3, "46004"))
            {
                if (!RI.CheckForText(4, 10, "04"))
                {
                    process = true;
                    if (RI.CheckForText(4, 10, "03") && RI.CheckForText(19, 73, "MMDDCCYY"))
                    {
                        if (RI.CheckForText(4, 26, "07"))
                            Comments("DBSOR", "SOBK");
                        else
                        {
                            RI.Hit(ReflectionInterface.Key.F10);
                            if (RI.CheckForText(17, 2, "14") || RI.CheckForText(17, 2, "17"))
                                if (DateTime.Parse(RI.GetText(18, 11, 8)) > DateTime.Now)
                                    Comments("DPSOR", "SONLE");
                            RI.Hit(ReflectionInterface.Key.F10);
                            if (!RI.CheckForText(4, 21, "04") && !RI.CheckForText(4, 21, "05") && !RI.CheckForText(4, 21, "  "))
                                ClaimIds.Add(RI.GetText(19, 12, 4));
                        }
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            if (!process)
                Comments("DPSOR", "SONLE");
        }

        /// <summary>
        /// Add LP50 comment, Send letter by DocId to centralized printing and ends the application
        /// </summary>
        /// <param name="docId">The Document ID</param>
        private void Comments(string activityType, string docId)
        {
            AddCommentInLP50(Demo.SSN, activityType, "LT", "10", "", ScriptId);
            MessageBox.Show("Process Complete.", "Process Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            EndDllScript();
        }

        /// <summary>
        /// Add LP50 comment for joint offset
        /// </summary>
        public void AddDDSOFComment()
        {
            string message = string.Format("{0:C}, {1}, {2}", Demo.GarnishmentAmount, Demo.WarrantNumber, Demo.ListDate.ToShortDateString());
            AddCommentInLP50(Demo.SSN, "AM", "10", "DDSOF", message, ScriptId);
            ProcessOffset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessOffset()
        {
            string message;
            if (Demo.UpdateAddress)
            {
                message = string.Format("{0},{1},{2},{3},{4}", Demo.Address1, Demo.Address2, Demo.City, Demo.State, Demo.Zip);
                AddQueueTaskInLP9O(Demo.SSN, "SKPSTADD", null, message, "", "", "");
                AddCommentInLP50(Demo.SSN, "AM", "10", "DTOUR", "Possible new address; sent to aux svcs for review", ScriptId);
                message = string.Format("B,{0},{1},{2},{3},{4}", Demo.Address1, Demo.Address2, Demo.City, Demo.State, Demo.Zip);
                AddQueueTaskInLP9O(Demo.SSN, "DSTPARMU", null, message, "", "", "");
            }
            //This will be done for both single and joint
            if (ClaimIds.Count > 0)
                ProcessLC34();
            message = string.Format("{0:C},{1},{2}", Demo.GarnishmentAmount, Demo.WarrantNumber, Demo.ListDate.ToShortDateString());
            AddQueueTaskInLP9O(Demo.SSN, "STOFFSET", Demo.ListDate.AddDays(30), message, "", "", "");
            AddCommentInLP50(Demo.SSN, "AM", "10", "DDSOF", message, ScriptId);
        }

        /// <summary>
        /// Choose all claims to be processed
        /// </summary>
        private void ProcessLC34()
        {
            RI.FastPath("LC34C" + Demo.SSN + "01");
            RI.PutText(6, 49, "04");
            int row = 9;
            while (!RI.CheckForText(22, 3, "46004"))
            {
                for (int i = 0; i < ClaimIds.Count; i++)
                {
                    if (RI.CheckForText(row, 5, ClaimIds[i]))
                        RI.PutText(row, 3, "X");
                }
                row++;
                if (RI.CheckForText(row, 5, "   "))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 9;
                }
            }
            RI.Hit(ReflectionInterface.Key.Enter);
        }
    }
}