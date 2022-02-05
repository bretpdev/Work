using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace AACACHUHEA
{
    public class AacAchUheaa : BatchScript
    {
        private const string EojTotalRecords = "Total number of records in the file";
        private const string EojProcessed = "Total number of records processed";
        private const string EojMissingFields = "Required fields were missing from the file";
        private const string EojErrorAddingACH = "There was an error adding ACH";
        private const string EojErrorChangingEbill = "There was an error changing the EBill";
        private const string EojErrorAddingArc = "There was an error adding an ARC";
        private static readonly string[] EOJ_FIELDS = { EojTotalRecords, EojProcessed, EojMissingFields, EojErrorAddingACH, EojErrorChangingEbill, EojErrorAddingArc };

        private bool IsInRecovery;

        public AacAchUheaa(ReflectionInterface ri)
            : base(ri, "AACACHUHEA", "ERR_BU01", "EOJ_BU01", EOJ_FIELDS, DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            StartupMessage("This script loads direct debit records to the system for recently loaded accounts. Click OK to continue, or Cancel to quit.");

            //Set up the recovery fields
            IsInRecovery = !string.IsNullOrEmpty(Recovery.RecoveryValue);
            int recoveryCount = 0;
            int recoveryStep = 0;
            if (IsInRecovery)
            {
                string[] recValues = Recovery.RecoveryValue.Split(',');
                recoveryCount = int.Parse(recValues[0]);
                recoveryStep = int.Parse(recValues[1]);
            }

            //Add a counter for recovery
            int counter = 0;

            string fileName = GetFileLocation();

            List<BorrowerData> data = LoadBorrowerData(fileName);

            //Groupd the borrowers together so each borrower gets processed once
            foreach (var bor in data.GroupBy(p => p.BorrowerSSN))
            {
                var borrower = bor.First();

                //Check if in recovery and skip every account until the last account processed is found
                if (IsInRecovery)
                {
                    if (recoveryCount < counter)
                    {
                        counter++;
                        continue;
                    }
                }


                bool ach = false;
                bool ebill = false;
                bool arc = false;

                if (IsInRecovery)
                {
                    if (recoveryStep == 0 && !string.IsNullOrEmpty(borrower.Email))
                    {
                        ebill = ChangeEbill(borrower);
                        Recovery.RecoveryValue = counter + ",1";
                        if (!ebill)
                        {
                            Eoj.Counts[EojErrorChangingEbill].Increment();
                            Err.AddRecord(EojErrorChangingEbill, bor);
                        }
                    }

                    if (recoveryStep != 2)
                    {
                        arc = AddArc(borrower);
                        Recovery.RecoveryValue = counter + ",2";
                        if (!arc)
                        {
                            Eoj.Counts[EojErrorAddingArc].Increment();
                            Err.AddRecord(EojErrorAddingArc, bor);
                            continue;
                        }

                        //If there are no errors, increment the processed count and reset the recovery
                        Eoj.Counts[EojProcessed].Increment();
                    }
                    IsInRecovery = false;
                }
                else
                {
                    //Add ACH and set EOJ counters
                    ach = AddAch(borrower);
                    Recovery.RecoveryValue = counter + ",0";
                    if (!ach)
                    {
                        Eoj.Counts[EojErrorAddingACH].Increment();
                        Err.AddRecord(EojErrorAddingACH, bor);
                        continue;
                    }

                    //Set up E-Bill and set the EOJ counters
                    if (!string.IsNullOrEmpty(borrower.Email))
                    {
                        ebill = ChangeEbill(borrower);
                        Recovery.RecoveryValue = counter + ",1";
                        if (!ebill)
                        {
                            Eoj.Counts[EojErrorChangingEbill].Increment();
                            Err.AddRecord(EojErrorChangingEbill, bor);
                        }
                    }

                    //Add ARC and set EOJ counters
                    arc = AddArc(borrower);
                    Recovery.RecoveryValue = counter + ",2";
                    if (!arc)
                    {
                        Eoj.Counts[EojErrorAddingArc].Increment();
                        Err.AddRecord(EojErrorAddingArc, bor);
                        continue;
                    }

                    //Increment the processed count if the ach, ebill and arc were successful
                    Eoj.Counts[EojProcessed].Increment();
                }
                //Increment the counter
                counter++;
                //Increment the total
                Eoj.Counts[EojTotalRecords].Increment();
            }

#if !DEBUG
            if (File.Exists(fileName))
                File.Delete(fileName);
#endif

            if (Err.HasErrors)
                ProcessingComplete("Processing Complete\r\n\r\nThere were errors found in processing. Please see the error report");
            else
                ProcessingComplete();
        }

        /// <summary>
        /// Moves the file from the FTP folder to the T: drive
        /// </summary>
        /// <returns>The location and name of the file</returns>
        private string GetFileLocation()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            while (true)
            {
                dialog.InitialDirectory = IsInRecovery ? EnterpriseFileSystem.TempFolder : EnterpriseFileSystem.FtpFolder;
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    EndDllScript();
                if (dialog.FileName.ToUpper().Contains("UHEAA AAC ACH"))
                    break;
                else
                    MessageBox.Show("You must choose a file name UHEAA AAC ACH SAS to process", "Invalid file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string fileName = EnterpriseFileSystem.TempFolder + dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1, dialog.FileName.Length - dialog.FileName.LastIndexOf("\\") - 1);

            if (!IsInRecovery)
            {
                if (File.Exists(fileName))
                {
                    MessageBox.Show("There is already a file named UHEAA AAC ACH SAS in the T: drive. Please remove the file and try again.", "File already exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EndDllScript();
                }
                else
                    File.Move(dialog.FileName, fileName);
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("There was an error moving the file from " + dialog.FileName + " to the T: drive.", "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }
            }
            return fileName;
        }

        /// <summary>
        /// Loads the file into a List of BorrowerData and creates an error report for missing data
        /// </summary>
        /// <param name="fileName">The file holding all the data</param>
        /// <returns>List of BorrowerData</returns>
        private List<BorrowerData> LoadBorrowerData(string fileName)
        {
            List<BorrowerData> data = new List<BorrowerData>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    BorrowerData bor = new BorrowerData();
                    List<string> line = sr.ReadLine().SplitAndRemoveQuotes(",");
                    if (!string.IsNullOrEmpty(line[0]))
                    {
                        bor.BorrowerSSN = line[0];
                        bor.RoutingNumber = line[1];
                        bor.AccountNumber = line[2];
                        bor.AccountType = line[3];
                        bor.AdditionalAmount = line[4];
                        bor.DueDate = line[5];
                        bor.Email = line[6];

                        //Add the borrower info to the error report if any data is missing
                        if (string.IsNullOrEmpty(line[0]) || string.IsNullOrEmpty(line[1]) || string.IsNullOrEmpty(line[2]) ||
                            string.IsNullOrEmpty(line[3]) || string.IsNullOrEmpty(line[4]) || string.IsNullOrEmpty(line[5]))
                        {
                            Err.AddRecord(EojMissingFields, bor);
                            Eoj.Counts[EojMissingFields].Increment();
                        }
                        else
                            data.Add(bor);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Adds an ACH to the borrower account
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        /// <returns>True if the ACH was successfully added to borrower</returns>
        private bool AddAch(BorrowerData bor)
        {
            FastPath("TX3ZATS7O");
            if (CheckForText(1, 71, "TSX7I"))
            {
                PutText(8, 42, bor.BorrowerSSN);
                PutText(12, 42, bor.RoutingNumber);
                PutText(14, 42, bor.AccountNumber);
                PutText(16, 42, bor.DueDate, ReflectionInterface.Key.Enter);
            }

            //If the account is eligible, add the ACH
            if (CheckForText(1, 72, "TSX7K"))
            {
                PutText(9, 18, bor.AccountType);
                PutText(12, 57, "Y");
                while (!CheckForText(23, 2, "90007"))
                {
                    for (int i = 17; i < 22; i++)
                    {
                        if (CheckForText(i, 3, "_"))
                        {
                            PutText(i, 3, "A");
                        }
                    }
                    Hit(ReflectionInterface.Key.F8);
                }
#if !DEBUG
                Hit(ReflectionInterface.Key.Enter);
#endif
            }
            else
                return false;

            //Change the status to Prenote instead of Initial
            if (CheckForText(23, 2, "01004"))
            {
                PutText(10, 18, "P", ReflectionInterface.Key.Enter);
            }
            else
                return false;

            //Increment the counts if the ACH was successful or not
            if (CheckForText(23, 2, "01005"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Changes the EBill indicator to Y
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        /// <returns>True if indicator changed to Y</returns>
        private bool ChangeEbill(BorrowerData bor)
        {
            FastPath("TX3ZCTS7C" + bor.BorrowerSSN);

            if (RI.ScreenCode == "TSX3S")
            {
                //Loop through each loan and change the billing method
                while (!CheckForText(23, 2, "90007"))
                {
                    for (int i = 7; i < 20; i++)
                    {
                        string selection = GetText(i, 3, 2);
                        if (!string.IsNullOrEmpty(selection))
                        {
                            PutText(22, 19, selection, ReflectionInterface.Key.Enter, true);
                            if (RI.ScreenCode == "TSX7D")
                            {
                                if (!ChangeEbill())
                                {
                                    Eoj.Counts[EojErrorChangingEbill].Increment();
                                    Err.AddRecord(EojErrorChangingEbill, bor);
                                    return false;
                                }
                            }
                            else
                            {
                                Eoj.Counts[EojErrorChangingEbill].Increment();
                                Err.AddRecord(EojErrorChangingEbill, bor);
                                return false;
                            }
                        Hit(ReflectionInterface.Key.F12);
                        }
                    }
                    Hit(ReflectionInterface.Key.F8);
                }
            }
            else if (RI.ScreenCode == "TSX7D")
            {
                if (!ChangeEbill())
                {
                    Eoj.Counts[EojErrorChangingEbill].Increment();
                    Err.AddRecord(EojErrorChangingEbill, bor);
                    return false;
                }
            }
            else
            {
                Eoj.Counts[EojErrorChangingEbill].Increment();
                Err.AddRecord(EojErrorChangingEbill, bor);
                return false;
            }

            return true;
        }

        private bool ChangeEbill()
        {
            //Change EBill indicator to Y
            PutText(19, 42, "Y");

            //Figure out the number of grace months by loan type
            if (CheckForText(18, 19, "__"))
                PutText(18, 19, CheckForText(6, 38, "STFFRD", "UNSTFD") ? "6" : "0");

            //If EXT TRM DEBT IND is blank, set it to N
            if (CheckForText(14, 48, "_"))
                PutText(14, 48, "N");

            Hit(ReflectionInterface.Key.Enter);

            return RI.MessageCode == "01005";
        }

        /// <summary>
        /// Add arc to borrower account
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        /// <returns>True if the arc was added successfully</returns>
        private bool AddArc(BorrowerData bor)
        {
            string comment = "Autopay was successfully added from conversion file.";
            string Arc = "AAuto";
            if (Atd22AllLoans(bor.BorrowerSSN, Arc, comment, "", ScriptId, false))
            {
                Eoj.Counts[EojProcessed].Increment();
                return true;
            }
            else
            {
                return false;
            }
        }

    }//class
}//namespace
