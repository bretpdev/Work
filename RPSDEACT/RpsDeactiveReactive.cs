using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace RPSDEACT
{
    public class RpsDeactiveReactive : BatchScript
    {
        private const string EOJProcessedHeader = "Number of borrowers processed";
        private const string EOJErroredHeader = "Number of borrowers errored";


        public RpsDeactiveReactive(ReflectionInterface ri)
            : base(ri, "RPSDEACT", "TEMP", "TEMP", new string[] { EOJProcessedHeader, EOJErroredHeader }, DataAccessHelper.Region.Uheaa)
        {

        }

        public override void Main()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(dialog.FileName))
                {
                    string firstLine = sr.ReadLine();
                    if (firstLine.IsNullOrEmpty())
                    {
                        MessageBox.Show("File has no rows.");
                        Main();
                    }
                    else if (firstLine.Split(',').Length < 2)
                    {
                        MessageBox.Show("Invalid file.  Each line must contain 2 parts each separated by a comma.");
                        Main();
                    }
                    else
                    {
                        ProcessFile(dialog.FileName);
                        base.ProcessingComplete();
                    }
                }
            }
            else
            {
                base.NotifyAndEnd("Must select a valid sas file to process.");
            }
        }

        private void ProcessFile(string fileName)
        {
            int firstLine = 0;
            if (!Recovery.RecoveryValue.IsNullOrEmpty())
                firstLine = int.Parse(Recovery.RecoveryValue);
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                int lineCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lineCount++;
                    if (lineCount < firstLine)
                        continue;
                    Recovery.RecoveryValue = lineCount.ToString();

                    BorrowerInfo bi = new BorrowerInfo();
                    string[] parts = line.Split(',');

                    bi.SSN = parts[0];
                    bi.ScheduleType = parts[1];

                    if (new string[] { "IB", "IL", "IS", "C1", "C2", "C3", "CA", "CP", "CQ" }.Contains(bi.ScheduleType))
                    {
                        Err.AddRecord(string.Format("Income RPS on line {0} in file {1}.", lineCount, fileName), bi);
                        Eoj.Counts[EOJErroredHeader].Increment();
                        continue;
                    }
                    else
                    {
                        ProcessBorrowerInfo(bi);
                    }
                }
            }

            Recovery.Delete();
        }

        private void ProcessBorrowerInfo(BorrowerInfo bi)
        {
            if (AccessDts0nToDeleteTheSchedule(bi))
                if (AddNewSchedule(bi))
                    Eoj.Counts[EOJProcessedHeader].Increment();
        }

        private bool AddNewSchedule(BorrowerInfo bi)
        {
            string ats0n = "TX3Z/ATS0N" + bi.SSN;
            FastPath(ats0n);

            if (!RI.Message.IsNullOrEmpty())
            {
                //AddErrorArc(bi);
                Err.AddRecord(string.Format("ATS0N, Error processing borrower with SSN {0}, {1}", bi.SSN, RI.Message), bi);
                Eoj.Counts[EOJErroredHeader].Increment();
                return false;
            }

            if (RI.ScreenCode == "TSX0P")
            {
                for (int row = 8; !RI.CheckForMessage("90007"); row++)
                {
                    if (row > 20)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                        continue;
                    }
                    else if (RI.GetText(row, 4, 2).IsNullOrEmpty())
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                        continue;
                    }

                    string selection = GetText(row, 4, 2);
                    RI.PutText(21, 12, selection, ReflectionInterface.Key.Enter, true);

                    if (RI.MessageCode.IsNullOrEmpty()) //chose an invalid selection
                    {
                        bool error = !ProcessTSX0S(bi, selection);
                        if (error) return false;
                        FastPath(ats0n);
                    }
                    else if (RI.MessageCode != "01032") //end of selections
                    {
                        Err.AddRecord("TSX0P, " + RI.Message, bi);
                        Eoj.Counts[EOJErroredHeader].Increment();
                        return false;
                    }
                }

            }
            else if (RI.ScreenCode == "TSX0S")
            {
                if (!ProcessTSX0S(bi))
                    return false;
            }
            else if (RI.ScreenCode == "TSX0R")
            {
                if (!ProcessTSX0S(bi))
                    return false;
            }

            return true;
        }

        private bool ProcessTSX0S(BorrowerInfo bi, string selection = "")
        {
            while (RI.MessageCode != "02895")
            {
                int row = 10;
                string oldInterestRate = GetText(row, 42, 5);
                for (; RI.MessageCode != "90007"; row++)
                {
                    if (row > 22 || CheckForText(row, 3, " "))
                    {
                        Hit(ReflectionInterface.Key.F8);
                        row = 9;
                        continue;
                    }

                    if (oldInterestRate == GetText(row, 42, 5))
                        PutText(row, 3, "X");
                }

                Hit(ReflectionInterface.Key.Enter);
                if (!AddSchedule(bi))
                    return false;

                FastPath("TX3Z/ATS0N" + bi.SSN);
                if (!selection.IsNullOrEmpty())
                    PutText(21, 12, selection, ReflectionInterface.Key.Enter, true);

                Hit(ReflectionInterface.Key.Enter);

            }
            return true;
        }

        private bool AddSchedule(BorrowerInfo bi)
        {
            if (RI.ScreenCode != "TSX0T")
                Hit(ReflectionInterface.Key.Enter);
            if (!RI.Message.IsNullOrEmpty())
            {
                //AddErrorArc(bi);
                Err.AddRecord("TSX0R, " + RI.Message, bi);
                Eoj.Counts[EOJErroredHeader].Increment();
                return false;
            }

            PutText(8, 14, bi.ScheduleType);
            //if (bi.ScheduleType == "FS")
            //    PutText(14, 3, bi.RepaymentTerm);
            //else if (bi.ScheduleType == "FG")
            //    PutText(9, 23, bi.RepaymentTerm);
            //else if (bi.ScheduleType == "PG" || bi.ScheduleType == "PL")
            //    PutText(9, 23, bi.MaxTerm);
            Hit(ReflectionInterface.Key.Enter);

            if (!RI.CheckForMessage("01840") && !RI.CheckForMessage("06579") && RI.MessageCode != "02074")
            {
                //AddErrorArc(bi);
                Err.AddRecord(string.Format("TSX0T, {0}", RI.Message), bi);
                Eoj.Counts[EOJErroredHeader].Increment();
                return false;
            }

            Hit(ReflectionInterface.Key.F4);
            Hit(ReflectionInterface.Key.F4);

            if (!RI.CheckForMessage("01832"))
            {
                //AddErrorArc(bi);
                Err.AddRecord("TSX0T, " + RI.Message, bi);
                Eoj.Counts[EOJErroredHeader].Increment();
                return false;
            }

            return true;
        }

        private bool AccessDts0nToDeleteTheSchedule(BorrowerInfo bi)
        {
            FastPath("TX3Z/DTS0N" + bi.SSN);
            //selection screen
            if (RI.ScreenCode == "TSX0P")
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (row > 20 || RI.GetText(row, 4, 3).IsNullOrEmpty())
                    {
                        Hit(ReflectionInterface.Key.F8);
                        row = 7;
                        continue;
                    }
                    PutText(21, 12, GetText(row, 4, 3), ReflectionInterface.Key.Enter, true);
                    if (!SelectAndDeleteSchedulesOnTSX0S(bi))
                        return false;
                }
            }
            else if (RI.ScreenCode == "TSX0S")
            {
                if (!SelectAndDeleteSchedulesOnTSX0S(bi))
                    return false;
            }

            return true;

        }

        private bool SelectAndDeleteSchedulesOnTSX0S(BorrowerInfo bi)
        {
            if (RI.ScreenCode == "TSX0O")
            {
                FastPath("TX3Z/DTS0N" + bi.SSN);
                return true;
            }
            for (int row = 11; RI.MessageCode != "90007"; row++)
            {
                if (row > 22 || RI.CheckForText(row, 3, " "))
                {
                    Hit(ReflectionInterface.Key.F8);
                    row = 11;
                    continue;
                }

                PutText(row, 3, "X");
                if (!Devactive(bi))
                    return false;

            }

            FastPath("TX3Z/DTS0N" + bi.SSN);
            return true;
        }

        private bool Devactive(BorrowerInfo bi)
        {
            Hit(ReflectionInterface.Key.Enter);
            Hit(ReflectionInterface.Key.Enter);
            Hit(ReflectionInterface.Key.F4);

            if (RI.MessageCode != "02082")
            {
                Err.AddRecord(string.Format("DTS0N, Error processing borrower with SSN {0}, {1}", bi.SSN, RI.Message), bi);
                Eoj.Counts[EOJErroredHeader].Increment();
                return false;
            }

            Hit(ReflectionInterface.Key.F12);
            Hit(ReflectionInterface.Key.F12);

            return true;
        }


    }
}
