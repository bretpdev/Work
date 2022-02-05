using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace AACLOCKUNL
{
    public class LockAndUnlock : ScriptBase
    {
        public enum Action { Lock, Unlock }
        private enum LockResult { Success, FoundVariance, FailedToLock }
        private enum SelectionResult { Success, CouldNotAccessMajorBatch, CouldNotFindMinorBatch, CouldNotSelectMinorBatch }

        private readonly string _errorFile;
        private string _userId;

        public LockAndUnlock(ReflectionInterface ri)
            : base(ri, "AACLOCKFED")
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            _errorFile = string.Format("{0}{1}_Errors.{2:yyyy-MM-dd.HHmm}.txt", EnterpriseFileSystem.TempFolder, ScriptId, DateTime.Now);
        }

        public override void Main()
        {
            //Check access to the batch screens.
            if (!UserHasChangeAccess("TA0H", "TA0L"))
            {
                MessageBox.Show("You do not have the correct access to run this script.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FastPath("PROF");
            _userId = GetText(2, 49, 7);

            //Make sure the user has the batch numbers ready.
            if (new MajorBatchQuestion().ShowDialog() != DialogResult.OK) { return; }

            //Get the batch numbers and desired action.
            Batches batches = new Batches();
            using (BatchEntry batchEntry = new BatchEntry(batches))
            {
                if (batchEntry.ShowDialog() != DialogResult.OK) { return; }
            }

            //Lock or unlock the batches, as requested.
            string finnishMessage;
            if (batches.Action == Action.Lock)
            {
                LockBatches(batches.ToArray());
                finnishMessage = "The batches have been locked.";
            }
            else
            {
                UnlockBatches(batches.ToArray());
                finnishMessage = "The batches have been unlocked.";
            }
            if (File.Exists(_errorFile)) { finnishMessage += string.Format("{0}Please see the {1} file for problems encountered during execution.", Environment.NewLine, _errorFile); }
            MessageBox.Show(finnishMessage, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }//Main()

        private void AddErrorRecord(string majorBatchNumber, string message)
        {
            bool newFile = !File.Exists(_errorFile);
            using (StreamWriter errorWriter = new StreamWriter(_errorFile, true))
            {
                if (newFile) { errorWriter.WriteLine("Major batch number, Error"); }
                errorWriter.WriteLine("{0}, {1}", majorBatchNumber, message);
            }
        }//AddErrorRecord()

        private List<string> GetMinorBatchNumbers(string majorBatchNumber)
        {
            List<string> minorBatchNumbers = new List<string>();
            if (CheckForText(1, 72, "TAX10"))
            {
                //Target screen.
                minorBatchNumbers.Add(GetText(4, 75, 5));
            }
            else if (CheckForText(1, 72, "TAX0Q"))
            {
                //Selection screen.
                while (!CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                {
                    for (int row = 9; !CheckForText(row, 6, "     "); row++)
                    {
                        minorBatchNumbers.Add(GetText(row, 6, 5));
                    }//for
                    if (CheckForText(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
                    { Hit(ReflectionInterface.Key.Enter); }
                    else
                    { Hit(ReflectionInterface.Key.F8); }
                }//while
            }
            return minorBatchNumbers;
        }//GetMinorBatchNumbers()

        private LockResult LockBatch()
        {
            //Go right back into CTA0H, which goes to ITX6T for the just-selected batch.
            PutText(1, 4, "CTA0H", ReflectionInterface.Key.Enter, true);
            //Make sure there's no variance.
            bool hasVariance = false;
            if (!CheckForText(12, 18, " 0")) { hasVariance = true; }
            if (!CheckForText(12, 31, " 0")) { hasVariance = true; }
            if (!CheckForText(20, 36, " 0")) { hasVariance = true; }
            if (!CheckForText(20, 54, " 0")) { hasVariance = true; }
            if (!CheckForText(20, 73, " 0")) { hasVariance = true; }
            string variance = GetText(20, 13, 10).Replace(",", "");
            if (variance.Length > 0 && double.Parse(variance) > 0) { hasVariance = true; }
            if (hasVariance) { return LockResult.FoundVariance; }
            //Hit enter to get to the detail screen and lock the batch.
            Hit(ReflectionInterface.Key.Enter);
            PutText(21, 27, "Y", ReflectionInterface.Key.Enter);
            if (CheckForText(23, 2, "02776 BATCH SUCCESSFULLY LOCKED"))
            { return LockResult.Success; }
            else
            { return LockResult.FailedToLock; }
        }//LockBatch()

        private void LockBatches(IEnumerable<string> majorBatchNumbers)
        {
            foreach (string majorBatchNumber in majorBatchNumbers)
            {
                FastPath("TX3Z/ITA0L");
                PutText(5, 19, majorBatchNumber, ReflectionInterface.Key.Enter);
                if (!CheckForText(23, 2, "     "))
                {
                    AddErrorRecord(majorBatchNumber, "Could not access the major batch.");
                    continue;
                }
                List<string> minorBatchNumbers = GetMinorBatchNumbers(majorBatchNumber);
                List<string> completedMinorBatches = new List<string>();

                while (true)
                {
                    FastPath("TX3Z/CTA0H");
                    PutText(10, 41, majorBatchNumber, ReflectionInterface.Key.Enter);
                    if (CheckForText(23, 2, "01020"))
                    {
                        break;
                    }
                    else if (!CheckForText(23, 2, "     "))
                    {
                        AddErrorRecord(majorBatchNumber, "Could not access the major batch.");
                        break;
                    }

                    string minorBatchNumber = GetText(7, 50, 5);
                    PutText(21, 18, "01", ReflectionInterface.Key.Enter);
                    if (!CheckForText(1, 72, "T1X01"))
                    {
                        MessageBox.Show(string.Format("Error selecting minor batch {0} for major batch {1}. Review error and start script again.", minorBatchNumber, majorBatchNumber), ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        EndDllScript();
                    }

                    LockResult lockResult = LockBatch();
                    if (lockResult != LockResult.Success)
                    {
                        string errorMessage = minorBatchNumber;
                        if (lockResult == LockResult.FoundVariance) { errorMessage += " variance"; }
                        AddErrorRecord(majorBatchNumber, errorMessage);
                        if (lockResult == LockResult.FoundVariance)
                        {
                            ReassignTasks();
                        }
                    }
                    else
                    {
                        completedMinorBatches.Add(minorBatchNumber);
                    }
                }//end while

                foreach (string item in minorBatchNumbers)
                {
                    if (!completedMinorBatches.Contains(item))
                    {
                        AddErrorRecord(majorBatchNumber, item + " Could not lock the minor batch.");
                    }
                }

            }//foreach
        }//LockBatches()

        private void ReassignTasks()
        {
            //If a minor batch was selected but for some reason was unable to be locked, it will show up as a task in the user's queue,
            //which prevents them from going into any other batches. To remedy this, unassign whatever task is open for the user.
            bool somethingGotUnassigned = false;
            foreach (string queue in new string[] { "AD", "AQ" })
            {
                FastPath("TX3Z/CTX6J");
                PutText(7, 42, queue);
                PutText(8, 42, "A1");
                PutText(13, 42, _userId);
                Hit(ReflectionInterface.Key.Enter);
                if (CheckForText(1, 72, "TXX6O"))
                {
                    PutText(8, 15, "UT00026", ReflectionInterface.Key.Enter, true);
                    if (CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED")) { somethingGotUnassigned = true; }
                }
            }//foreach
            if (!somethingGotUnassigned)
            {
                MessageBox.Show("Error unassigning batch with variance.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }
        }//UnassignTasks()

        private bool UnlockBatch()
        {
            //Update the status to UNLOCKED.
            try
            {
                PutText(9, 19, "U", ReflectionInterface.Key.Enter);
            }
            catch (Exception)
            {
                //If the field isn't writeable, the batch is already unlocked.
            }
            return CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED");
        }//UnlockBatch()

        private void UnlockBatches(IEnumerable<string> majorBatchNumbers)
        {

            foreach (string majorBatchNumber in majorBatchNumbers)
            {
                bool accessedBatch = false;
                foreach (string status in new List<string>() { "D", "Q", "V" })
                {
                    FastPath("TX3Z/CTA0L");
                    PutText(5, 19, majorBatchNumber);
                    PutText(15, 26, status, ReflectionInterface.Key.Enter, true);
                    if (CheckForText(1, 72, "TAX10"))
                    {
                        //Target screen.
                        if (!UnlockBatch()) { AddErrorRecord(majorBatchNumber, GetText(4, 75, 5)); }
                        accessedBatch = true;
                    }
                    else if (CheckForText(1, 72, "TAX0Q"))
                    {
                        accessedBatch = true;
                        //The session works funny when you Hit F8 to go to the next screen so we have to code around it.
                        //Selection screen. Go through all minor batches.
                        for (int row = 9; !CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"); row++)
                        {
                            if (CheckForText(row, 6, "  "))
                            {
                                if (CheckForText(2, 69, "20"))
                                {
                                    FastPath("TX3Z/CTA0L*");
                                    PutText(5, 19, majorBatchNumber);
                                    PutText(15, 26, status, ReflectionInterface.Key.Enter, true);
                                    row = 8;
                                    continue;
                                }
                                else
                                {
                                    Hit(ReflectionInterface.Key.F8);
                                    row = 8;
                                    continue;
                                }

                            }
                            PutText(22, 18, GetText(row, 3, 2), ReflectionInterface.Key.Enter, true);
                            if (!UnlockBatch()) { AddErrorRecord(majorBatchNumber, GetText(4, 75, 5)); }
                            Hit(ReflectionInterface.Key.F12);
                        }//for
                    }
                    else
                    {
                        continue;
                    }


                }
                if (!accessedBatch)
                    AddErrorRecord(majorBatchNumber, "Could not access the major batch.");
            }//foreach
        }//UnlockBatches()

        private bool UserHasChangeAccess(params string[] transactionIds)
        {
            FastPath("TX3Z");
            foreach (string transactionId in transactionIds.OrderBy(p => p))
            {
                Coordinate coord = FindText(transactionId);
                while (coord == null)
                {
                    Hit(ReflectionInterface.Key.F8);
                    coord = FindText(transactionId);
                }
                if (!CheckForText(coord.Row, 74, "C")) { return false; }
            }
            return true;
        }//UserHasChangeAccess()
    }
}
