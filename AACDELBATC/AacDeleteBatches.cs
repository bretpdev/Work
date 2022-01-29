using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace AACDELBATC
{
    public class AacDeleteBatches : BatchScript
    {
        public AacDeleteBatches(ReflectionInterface ri)
            : base(ri, "AACDELBATC", "ERR_BU01", "EOJ_BU01", new string[] { "" }, DataAccessHelper.Region.Uheaa)
        {
            ;
        }

        public override void Main()
        {
            List<MajorBatchInfo> batchData = CheckRecovery();
            //If the count is greater than 1 we know that we are in recovery.
            if (batchData.Count > 0)
            {
                if (MessageBox.Show("The script has detected it is in recovery.  Do you wish to continue processing?", "Continue", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    if (MessageBox.Show("By not continuing processing you will delete the recovery for this user id.  Do you wish to continue?", "Continue", MessageBoxButtons.YesNo) == DialogResult.No)
                        EndDllScript();
                    else
                    {
                        DataAccess.DeleteAllBatchesForUserId(UserId);
                        batchData = new List<MajorBatchInfo>();
                    }
                }
            }

            if(batchData.Count < 1)
            {
                using (EnterBatches batches = new EnterBatches(batchData))
                {
                    if (batches.ShowDialog() == DialogResult.Cancel)
                        EndDllScript();

                    DataAccess.InsertbatchesToDelete(batchData, UserId);
                    Recovery.RecoveryValue = UserId;
                }
            }

            GatherMinorBatcheInfo(batchData);
            ProcessingComplete();
        }

        private List<MajorBatchInfo> CheckRecovery()
        {
            return DataAccess.GetRecoveryValues(UserId);
        }

        private void GatherMinorBatcheInfo(List<MajorBatchInfo> batchData)
        {
            foreach (MajorBatchInfo item in batchData)
            {
                bool firstRun = true;
                while (true)
                {
                    FastPath("TX3Z/ITA0M");
                    PutText(5, 19, item.MajorBatchToDelete, ReflectionInterface.Key.Enter, true);
                    Thread.Sleep(2000);
                    if (CheckForText(1, 72, "TAX0Z"))
                    {
                        if (firstRun)
                        {
                            Err.AddRecord("Unable to locate major batch.", new { MajorBatch = item.MajorBatchToDelete, MinorBatch = string.Empty, SSN = "" });
                            break;
                        }
                        else
                            break;
                    }
                    firstRun = false;

                    if (CheckForText(1, 72, "TAX0Q"))
                    {
                        //There are multiple minor batches
                        for (int row = 9; !CheckForText(23, 2, "90007"); row++)
                        {
                            if (CheckForText(row, 6, " ") || row > 21)
                            {
                                Hit(ReflectionInterface.Key.F8);
                                row = 8;
                                continue;
                            }

                            PutText(22, 18, GetText(row, 3, 3), ReflectionInterface.Key.Enter, true);
                            item.MinorBatchesToDelete.Add(GetMinorBatchInfo());
                            Hit(ReflectionInterface.Key.F12);
                        }
                    }
                    else
                        item.MinorBatchesToDelete.Add(GetMinorBatchInfo());

                    bool canDeleteMajorBatch = true;
                    foreach (MinorBatchInfo minorBatch in item.MinorBatchesToDelete)
                    {
                        if (!DeleteTheBatch(minorBatch, item.MajorBatchToDelete))
                        {
                            canDeleteMajorBatch = false;
                            UnassignMajorMinorBatch();
                        }
                    }

                    if (canDeleteMajorBatch)
                    {
                        FastPath("TX3Z/DTA0I");
                        PutText(5, 19, item.MajorBatchToDelete);
                        PutText(11, 23, " ", ReflectionInterface.Key.Enter);
                        Hit(ReflectionInterface.Key.Enter);

                        if (!CheckForText(23, 2, "01006"))
                            Err.AddRecord("Unable to delete major batch on DTA0I.", new { MajorBatch = item.MajorBatchToDelete, MinorBatch = string.Empty, SSN = "" });
                    }
                    else
                        break;
                }

                DataAccess.DeleteProcessedBatch(item.MajorBatchToDelete);
            }//End foreach
        }

        private bool DeleteTheBatch(MinorBatchInfo batchInfo, string majorBatch)
        {
            bool canDeleteMajorBatch = true;
            FastPath("TX3Z/ITX6T*");
            PutText(6, 41, "", true);
            PutText(6, 45, "", true);
            PutText(6, 48, "", true);
            PutText(8, 41, "", true);
            PutText(10, 41, majorBatch, ReflectionInterface.Key.Enter, true);
            Thread.Sleep(2000);

            if (CheckForText(23, 2, "01020"))
            {
                Err.AddRecord("Unable to find batch in ITX6T", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = "" });
                return false;
            }

            for (int row = 7; !CheckForText(23, 2, "90007"); row += 5)
            {
                if (row > 20 || CheckForText(row, 50, " "))
                {
                    Hit(ReflectionInterface.Key.F8);
                    row = 2;
                    continue;
                }

                if (int.Parse(GetText(row, 50, 5)) == int.Parse(batchInfo.MinorBatch))
                {
                    PutText(21, 18, GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                    if (CheckForText(23, 2, "01848"))
                    {
                        MessageBox.Show("You have a batch in working status.  Please unassign this batch and try again.");
                        EndDllScript();
                    }
                    break;
                }
            }
            foreach (string ssn in batchInfo.SsnsInTheBatch)
            {
                PutText(1, 4, "DTA0B", ReflectionInterface.Key.EndKey);
                Hit(ReflectionInterface.Key.Enter);
                PutText(8, 42, ssn, ReflectionInterface.Key.Enter, true);
                if (!DeleteDefForb())
                {
                    Err.AddRecord("Unable to delete deferment on DTA0B.", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = ssn });
                    canDeleteMajorBatch = false;
                    continue;
                }

                PutText(1, 4, "DTA0C", ReflectionInterface.Key.EndKey);
                Hit(ReflectionInterface.Key.Enter);
                PutText(8, 42, ssn, ReflectionInterface.Key.Enter, true);
                if (!DeleteDefForb())
                {
                    Err.AddRecord("Unable to delete forbearance on DTA0C.", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = ssn });
                    canDeleteMajorBatch = false;
                    continue;
                }

                PutText(1, 4, "DTA3X", ReflectionInterface.Key.EndKey);
                Hit(ReflectionInterface.Key.Enter);
                PutText(8, 42, ssn, ReflectionInterface.Key.Enter, true);


                if (CheckForText(1, 72, "TAX0X"))
                {
                    for (int row = 9; !CheckForText(23, 2, "90007"); row++)
                    {
                        if (CheckForText(row, 30, " ") || row > 21)
                        {
                            Hit(ReflectionInterface.Key.F8);
                            row = 8;
                            continue;
                        }

                        PutText(22, 12, GetText(row, 3, 3), ReflectionInterface.Key.Enter, true);
                        if (!DeleteTa3x())
                        {
                            DeleteTA07();
                            DeleteTA09();
                            PutText(1, 4, "DTA3X", ReflectionInterface.Key.EndKey);
                            Hit(ReflectionInterface.Key.Enter);
                            PutText(8, 42, ssn, ReflectionInterface.Key.Enter, true);
                            for (int row1 = 9; !CheckForText(23, 2, "90007"); row1++)
                            {
                                if (CheckForText(row1, 30, " ") || row1 > 21)
                                {
                                    Hit(ReflectionInterface.Key.F8);
                                    row = 8;
                                    continue;
                                }

                                PutText(22, 12, GetText(row1, 3, 3), ReflectionInterface.Key.Enter, true);

                                if (!DeleteTa3x())
                                {
                                    Err.AddRecord("Unable to delete loans on DTA3X.", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = ssn });
                                    canDeleteMajorBatch = false;
                                }
                                Hit(ReflectionInterface.Key.F12);
                            }
                            break;
                        }
                        Hit(ReflectionInterface.Key.F12);
                    }
                }
                else
                {
                    if (!DeleteTa3x())
                    {
                        DeleteTA07();
                        DeleteTA09();
                        PutText(1, 4, "DTA3X", ReflectionInterface.Key.EndKey);
                        Hit(ReflectionInterface.Key.Enter);
                        PutText(8, 42, ssn, ReflectionInterface.Key.Enter, true);
                        if (!DeleteTa3x())
                        {
                            Err.AddRecord("Unable to delete loans on DTA3X.", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = ssn });
                            canDeleteMajorBatch = false;
                        }
                        continue;
                    }
                }
            }

            if (!canDeleteMajorBatch)
                return false;

            FastPath("TX3Z/DTA0M");
            PutText(5, 19, majorBatch);
            PutText(5, 52, batchInfo.MinorBatch, ReflectionInterface.Key.Enter);

            for (int row = 10; !CheckForText(23, 2, "90007"); row++)
            {
                if (CheckForText(row, 2, " ") || row > 22)
                {
                    Hit(ReflectionInterface.Key.Enter);
                    Hit(ReflectionInterface.Key.F8);
                    row = 9;
                    continue;
                }

                PutText(row, 2, "X");
            }

            if (CheckForText(23, 2, "02116"))
            {
                Err.AddRecord("Unable to delete minor batch on DTA0M.", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = "" });
                UnassignMajorMinorBatch();
                return false;
            }

            FastPath("TX3Z/ITX6X");
            PutText(6, 37, "AD");
            PutText(8, 37, "A1");
            PutText(10, 37, "", ReflectionInterface.Key.Enter, true);

            if (!CheckForText(8, 75, "W"))
            {
                Err.AddRecord("Unable to find queue task in ITX6X to delete.", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = "" });
                return false;
            }

            PutText(21, 18, "01", ReflectionInterface.Key.F2, true);
            PutText(8, 19, "X", ReflectionInterface.Key.Enter);
            FastPath("TX3Z/DTA0L");
            PutText(5, 19, majorBatch);
            PutText(5, 52, batchInfo.MinorBatch, ReflectionInterface.Key.Enter);
            Hit(ReflectionInterface.Key.Enter);

            if (!CheckForText(23, 2, "01006"))
            {
                Err.AddRecord("Unable to delete minor batch on DTA0L.", new { MajorBatch = majorBatch, MinorBatch = batchInfo.MinorBatch, SSN = "" });
                return false;
            }

            return canDeleteMajorBatch;
        }

        private bool DeleteTa3x()
        {
            if (CheckForText(23, 2, "01853", "01852","01527"))
                return true;

            if (!CheckForText(23, 2, "03971"))
            {
                Hit(ReflectionInterface.Key.F2);
                Hit(ReflectionInterface.Key.F4);

                while (!CheckForText(23, 2, "01061"))
                {
                    Hit(ReflectionInterface.Key.Enter);
                    Hit(ReflectionInterface.Key.F8);
                }
                Hit(ReflectionInterface.Key.F12);
            }

            Hit(ReflectionInterface.Key.F10);

            if (!CheckForText(23, 2, "01006"))
                return false;

            return true;
        }

        private bool DeleteDefForb()
        {
            while (true)
            {
                PutText(1, 4, "D", ReflectionInterface.Key.Enter);

                if (CheckForText(23, 2, "01943", "01527", "01852"))
                    return true;
                for (int row = 11; !CheckForText(23, 2, "90007"); row++)
                {
                    if (CheckForText(row, 3, " ") || row > 22)
                    {
                        break;
                    }
                    PutText(row, 3, "X");
                }

                Hit(ReflectionInterface.Key.F10);

                if (!CheckForText(23, 2, "02136"))
                {
                    for (int row = 11; !CheckForText(23, 2, "90007"); row++)
                    {
                        if (CheckForText(row, 3, " ") || row > 22)
                        {
                            Hit(ReflectionInterface.Key.Enter);
                            if (!CheckForText(23, 2, "02115"))
                                return false;
                            Hit(ReflectionInterface.Key.F8);
                            row = 10;
                            continue;
                        }

                        PutText(row, 3, "N");
                    }
                    Hit(ReflectionInterface.Key.F12);
                }
                Hit(ReflectionInterface.Key.Enter);
                if (!CheckForText(23, 2, "02116"))
                    return false;
            }
        }

        private MinorBatchInfo GetMinorBatchInfo()
        {
            List<string> ssnsInMinorBatch = new List<string>();
            for (int row = 10; !CheckForText(23, 2, "90007"); row++)
            {
                if (CheckForText(row, 8, " ") || row > 22)
                {
                    Hit(ReflectionInterface.Key.F8);
                    row = 9;
                    continue;
                }

                ssnsInMinorBatch.Add(GetText(row, 8, 11).Replace(" ", ""));
            }

            return new MinorBatchInfo()
            {
                MinorBatch = GetText(5, 39, 5),
                SsnsInTheBatch = ssnsInMinorBatch
            };
        }

        private bool UnassignMajorMinorBatch()
        {
            FastPath("TX3Z/CTX6J");
            PutText(7, 42, "AD");
            PutText(8, 42, "A1");
            PutText(9, 42, " ", true);
            PutText(13, 42, UserId, ReflectionInterface.Key.Enter);

            if (CheckForText(23, 2, "01020"))
            {
                PutText(7, 42, "AQ");
                PutText(8, 42, "A1");
                PutText(13, 42, UserId, ReflectionInterface.Key.Enter);

                if (CheckForText(23, 2, "01020"))
                    return false;
            }

            PutText(8, 15, " ", ReflectionInterface.Key.Enter, true);

            if (CheckForText(23, 2, "01005"))
                return true;
            else
                return false;
        }

        private void DeleteTA07()
        {
            PutText(1, 4, "DTA07", ReflectionInterface.Key.Enter);
            if (!CheckForText(1, 72, "TAX1G"))
                return;

            Hit(ReflectionInterface.Key.Enter);
            Hit(ReflectionInterface.Key.Enter);
            

        }

        private void DeleteTA09()
        {
            PutText(1, 4, "DTA09", ReflectionInterface.Key.Enter);
            if (!CheckForText(1, 72, "TAX1K"))
                return;

            Hit(ReflectionInterface.Key.Enter);

        }
    }
}
