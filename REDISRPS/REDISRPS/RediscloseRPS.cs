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


namespace REDISRPS
{
    public class RediscloseRPS : FedBatchScript
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
            : base(ri, "REDISRPS", "TEMP", "TEMP", new string[] { EOJProcessedHeader, EOJErroredHeader, EOJInvalidLinesHeader, EOJOutdatedLinesHeader })
        {
            EOJProcessed = Eoj[EOJProcessedHeader];
            EOJErrored = Eoj[EOJErroredHeader];
            EOJInvalidLines = Eoj[EOJInvalidLinesHeader];
            EOJOutdatedLines = Eoj[EOJOutdatedLinesHeader];
        }

        public override void Main()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "SAS Files (*.sas)|*.sas";
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
                        using (SqlCommand comm = DataAccessHelper.GetCommand("REDISRPS_MonitorBorrowerIsOutdated", DataAccessHelper.Database.Cdw))
                        {
                            comm.Parameters.AddWithValue("AccountIdentifier", bi.SSN);
                            if (!(bool)comm.ExecuteScalar())
                            {
                                ProcessBorrowerInfo(bi);
                            }
                            else
                            {
                                Err.AddRecord(string.Format("Line {0}, borrower has an RPS more recent than the last Monitor execution.", lineCount), bi);
                                EOJOutdatedLines++;
                            }
                        }
                    }
                }
            }

            Recovery.Delete();
        }

        private void AddQueueTask(string ssn, string message)
        {
            //TODO this will be implemented once Parish has time to create a new arc and queue.
            RI.Atd22ByBalance(ssn, "ARC", message, string.Empty, ScriptId, false, false);
        }

        private void ProcessBorrowerInfo(BorrowerInfo bi)
        {
            string ats0n = "TX3Z/ATS0N" + bi.SSN;
            FastPath(ats0n);
            if (!RI.Message.IsNullOrEmpty())
            {
                AddErrorArc(bi);
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

                    RI.PutText(21, 12, GetText(row, 4, 2), ReflectionInterface.Key.Enter, true);

                    if (RI.MessageCode.IsNullOrEmpty()) //chose an invalid selection
                    {
                        bool error = !ProcessTSX0S(bi);
                        if (error) return;
                        FastPath(ats0n);
                    }
                    else if (RI.MessageCode != "01032") //end of selections
                    {
                        AddErrorArc(bi);
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
                AddErrorArc(bi);
                Err.AddRecord("TSX0R, " + RI.Message, bi);
                EOJErrored++;
                return;
            }

            EOJProcessed++;
        }

        private bool ProcessTSX0S(BorrowerInfo bi)
        {
            //////
            //TODO: collect which loan sequences were selected
            //////
            while (!RI.CheckForMessage("90007")) //Error code is for no more pages
            {
                Coordinate c = GetCurrentCursorCoordinate();
                while (c.Column != 4 && c.Row != 1)
                {
                    HitChar('X');
                    c = GetCurrentCursorCoordinate();
                }
                Hit(ReflectionInterface.Key.F8);//go to next page
            }
            Hit(ReflectionInterface.Key.Enter);
            if (RI.ScreenCode != "TSX0T")
                Hit(ReflectionInterface.Key.Enter);
            if (!RI.Message.IsNullOrEmpty())
            {
                AddErrorArc(bi);
                Err.AddRecord("TSX0R, " + RI.Message, bi);
                EOJErrored++;
                return false;
            }

            PutText(8, 14, bi.ScheduleType);
            if (bi.ScheduleType == "FS")
                PutText(14, 3, bi.RepaymentTerm);
            else if (bi.ScheduleType == "FG")
                PutText(9, 23, bi.RepaymentTerm);
            else if (bi.ScheduleType == "PG" || bi.ScheduleType == "PL")
                PutText(9, 23, bi.MaxTerm);
            Hit(ReflectionInterface.Key.Enter);

            if (!RI.CheckForMessage("01840") && !RI.CheckForMessage("06579"))
            {
                AddErrorArc(bi);
                Err.AddRecord(string.Format("TSX0T, {0}", RI.Message), bi);
                EOJErrored++;
                return false;
            }

            Hit(ReflectionInterface.Key.F4);
            Hit(ReflectionInterface.Key.F4);

            if (!RI.CheckForMessage("01832"))
            {
                AddErrorArc(bi);
                Err.AddRecord("TSX0T, " + RI.Message, bi);
                EOJErrored++;
                return false;
            }

            return true;
        }

        private void AddErrorArc(BorrowerInfo bi)
        {
            Func<int, int, bool> checkAndSelect = new Func<int, int, bool>((row, col) =>
            {
                if (CheckForText(row, col, "MONRV"))
                {
                    PutText(row, col - 5, "01", ReflectionInterface.Key.Enter);
                    return true;
                }
                return false;
            });
            string code = RI.ScreenCode;
            string screenMessage = RI.Message;
            FastPath("TX3Z/ATD22" + bi.SSN);
            while (RI.MessageCode != "90007")
            {
                for (int row = 8; row < 23; row++)
                {
                    if (checkAndSelect(row, 8) || checkAndSelect(row, 48))
                    {
                        PutText(11, 3, "X");
                        string message = "There was an error while attempting to disclose on the current repayment schedule, please review. " + code + ", " + screenMessage;
                        message = message.Substring(0, Math.Min(154, message.Length));
                        //RI.ReflectionSession.MoveCursor(21, 2);
                        PutText(21, 2, message);
                        RI.Hit(ReflectionInterface.Key.Enter);
                        return;
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }
    }
}
