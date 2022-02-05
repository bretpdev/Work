using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;
using System.Windows.Forms;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using System.Data.SqlClient;

namespace RPSREDISC
{
    public class RediscloseRPS : BatchScript
    {
        private const string EOJProcessedHeader = "Number of borrowers processed";
        private const string EOJErroredHeader = "Number of borrowers errored";
        private const string EOJInvalidLinesHeader = "Invalid lines";
        private const string EOJOutdatedLinesHeader = "Outdated lines";

        private Count EOJProcessed { get; set; }
        private Count EOJErrored { get; set; }
        private Count EOJInvalidLines { get; set; }
        private Count EOJOutdatedLines { get; set; }

        public RediscloseRPS(ReflectionInterface ri)
            : base(ri, "REDISRPS", "TEMP", "TEMP", new string[] { EOJProcessedHeader, EOJErroredHeader, EOJInvalidLinesHeader, EOJOutdatedLinesHeader }, DataAccessHelper.Region.Uheaa)
        {
            EOJProcessed = Eoj[EOJProcessedHeader];
            EOJErrored = Eoj[EOJErroredHeader];
            EOJInvalidLines = Eoj[EOJInvalidLinesHeader];
            EOJOutdatedLines = Eoj[EOJOutdatedLinesHeader];
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
                    else if (firstLine.Split(',').Length != 4)
                    {
                        MessageBox.Show("Invalid file.  Each line must contain 4 parts each separated by a comma.");
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
                    if (parts.Length != 4)
                    {
                        Err.AddRecord(string.Format("Line {0} in file {1} is invalid.  Expecting 4 parts.", lineCount, fileName), new BorrowerInfo());
                        EOJInvalidLines++;
                        continue;
                    }

                    bi.SSN = parts[0];
                    bi.ScheduleType = parts[1];
                    bi.RepaymentTerm = parts[2];
                    bi.MaxTerm = parts[3];

                    if (new string[] { "IB", "IL", "IS", "C1", "C2", "C3", "CA", "CP", "CQ" }.Contains(bi.ScheduleType))
                    {
                        Err.AddRecord(string.Format("Income RPS on line {0} in file {1}.", lineCount, fileName), bi);
                        EOJInvalidLines++;
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
            string ats0n = "TX3Z/ATS0N" + bi.SSN;
            FastPath(ats0n);
            if (!RI.Message.IsNullOrEmpty())
            {
                //AddErrorArc(bi);
                Err.AddRecord(string.Format("ATS0N, Error processing borrower with SSN {0}, {1}", bi.SSN, RI.Message), bi);
                EOJErrored++; ;
                return;
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
                        if (error) return;
                        FastPath(ats0n);
                    }
                    else if (RI.MessageCode != "01032") //end of selections
                    {
                        //AddErrorArc(bi);
                        Err.AddRecord("TSX0P, " + RI.Message, bi);
                        EOJErrored++;
                        return;
                    }
                }

            }
            else if (RI.ScreenCode == "TSX0S")
            {
                if (!ProcessTSX0S(bi))
                    return;
            }
            else if (RI.ScreenCode == "TSX0R")
            {
                //AddErrorArc(bi);
                Err.AddRecord("TSX0R, " + RI.Message, bi);
                EOJErrored++;
                return;
            }

            EOJProcessed++;
        }

        private bool ProcessTSX0S(BorrowerInfo bi, string selection = "")
        {
            if (RI.MessageCode == "02875" || RI.MessageCode == "02876")
            {
                Err.AddRecord(string.Format("TSX0R, {0}", RI.Message), bi);
                EOJErrored++;
                return false;
            }
            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 22)
                {
                    Hit(ReflectionInterface.Key.F8);
                    row = 9;
                    continue;
                }


                if (RI.CheckForText(row, 3, " ") || RI.CheckForText(row, 31, "  /  /"))
                    continue;

                PutText(row, 3, "X", ReflectionInterface.Key.Enter);
                if (!AddSchedule(bi))
                    return false;

                FastPath("TX3Z/ATS0N" + bi.SSN);

                if (!selection.IsNullOrEmpty())
                {
                    MakeSelection(selection);
                    row = 9;
                }

                if (RI.ScreenCode == "TSX0S" && RI.CheckForText(row,3,"X"))
                    Hit(ReflectionInterface.Key.EndKey);

                if (RI.ScreenCode == "TSX0P")
                    Hit(ReflectionInterface.Key.Enter);

            }


            return true;
        }

        private void MakeSelection(string selection)
        {
            PutText(21, 12, selection, ReflectionInterface.Key.Enter, true);
        }

        private bool AddSchedule(BorrowerInfo bi)
        {
            if (RI.ScreenCode != "TSX0T")
                Hit(ReflectionInterface.Key.Enter);
            if (!RI.Message.IsNullOrEmpty())
            {
                Err.AddRecord("TSX0R, " + RI.Message, bi);
                EOJErrored++;
                return false;
            }

            PutText(8, 14, bi.ScheduleType);

            Hit(ReflectionInterface.Key.Enter);

            if (!RI.CheckForMessage("01840") && !RI.CheckForMessage("06579") && RI.MessageCode != "02074")
            {
                Err.AddRecord(string.Format("TSX0T, {0}", RI.Message), bi);
                EOJErrored++;
                return false;
            }

            Hit(ReflectionInterface.Key.F4);
            Hit(ReflectionInterface.Key.F4);

            if (!RI.CheckForMessage("01832"))
            {
                Err.AddRecord("TSX0T, " + RI.Message, bi);
                EOJErrored++;
                return false;
            }

            return true;
        }
    }
}
