using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;

namespace ORCPAYPOST
{
    public class ORCCPaymentPosting : ScriptBase
    {
        public ORCCPaymentPosting(ReflectionInterface ri)
            : base(ri, "ORCPAYPOST")
        {
        }

        public override void Main()
        {
            string[] fileToProcess = Directory.GetFiles(EnterpriseFileSystem.TempFolder, "ORCC Payments*");

            if (fileToProcess.Count() > 1)
            {
                MessageBox.Show("Multiple ORCC Payments.csv files exist.  Please resolve and try again.");
                EndDllScript();
            }
            else if (fileToProcess.Count() < 1)
            {
                MessageBox.Show("ORCC Payments.csv file was not found.  Please resolve and try again.");
                EndDllScript();
            }

            List<FileData> dataFromFile = ReadCsvFile(fileToProcess[0]);
            CreatePostingBatch(dataFromFile);

            File.Delete(fileToProcess[0]);
            MessageBox.Show("Processing Complete.");
        }

        private List<FileData> ReadCsvFile(string fileToProcess)
        {
            using (Processing pro = new Processing(fileToProcess))
            {
                pro.Process();

                if (pro.Data.Count < 1)
                {
                    MessageBox.Show("The ORCC Payments.csv file on your T drive was empty.  Please investigate and try again.");
                    EndDllScript();
                }

                decimal total = pro.Data.Sum(p => p.PaymentAmount);

                return pro.Data;
            }
        }


        private void CreatePostingBatch(List<FileData> data)
        {
            if (data.Count > 9999)
            {
                int numberOfBatches = data.Count / 9999;
                if (data.Count % 9999 != 0)
                {
                    numberOfBatches++;
                }

                for (int i = 0; i < numberOfBatches; i++)
                {
                    CreateTheBatch(data.Skip(i * 9999).Take(9999).ToList(), false);
                }
            }
            else
            {
                CreateTheBatch(data, true);
            }

        }

        private void CreateTheBatch(List<FileData> data, bool singleBatch)
        {
            FastPath("TX3Z/ATS1G");

            if (CheckForText(23, 2, "01473"))
                PutText(10, 32, " ", ReflectionInterface.Key.Enter, true);

            PutText(6, 51, "2");
            PutText(10, 28, data.Count.ToString());
            PutText(11, 28, data.Sum(p => p.PaymentAmount).ToString());
            PutText(12, 28, "10");
            PutText(13, 28, DateTime.Now.ToString("MMddyy"));
            PutText(15, 28, UserId, ReflectionInterface.Key.Enter);

            if (!CheckForText(23, 2, "01004"))
            {
                MessageBox.Show("An error has occured.  Please fix the error and press insert to continue the script.");
                PauseForInsert();
            }

            string batchNumber = GetText(6, 18, 14);
            EnterIndividualPayments(data, batchNumber);
            if (!VerifyBatchTotals(batchNumber, data))
            {
                string message = singleBatch ? "Batch {0} is not in balance.  Please review this batch and correct any errors, the script will now end." : "Batch {0} is not in balance.  The script will continue processing other batches, please review this batch and correct any errors.";
                MessageBox.Show(message);
                if (singleBatch)
                    EndDllScript();
            }
        }

        private void EnterIndividualPayments(List<FileData> data, string batchNumber)
        {
            FastPath("TX3Z/ATS1J");
            if (CheckForText(1, 72, "T1X03"))
                PutText(10, 32, batchNumber, ReflectionInterface.Key.Enter);

            int row = 8;
            foreach (FileData fileLine in data)
            {
                if (row > 19)
                {
                    row = 8;
                    Hit(ReflectionInterface.Key.F8);
                }
                PutText(row, 5, fileLine.AccountNumber);
                PutText(row, 17, fileLine.PaymentAmount.ToString());
                PutText(row, 28, fileLine.PaymentEffectiveDate, ReflectionInterface.Key.Enter);

                if (RI.MessageCode == "05097")
                {
                    TS1JError("Does not exist on system", fileLine.BorrowersLastName, row);
                    Hit(ReflectionInterface.Key.F2);
                }

                if (RI.MessageCode == "30008")
                {
                    TS1JError("Not a duplicate item", fileLine.BorrowersLastName, row);
                    Hit(ReflectionInterface.Key.F2);
                }

                if (RI.MessageCode == "04404")
                {
                    TS1JError("All Loans Deconverted", fileLine.BorrowersLastName, row);
                    Hit(ReflectionInterface.Key.F2);
                }
                row++;
            }
        }

        private bool VerifyBatchTotals(string batchNumber, List<FileData> data)
        {
            FastPath("TX3Z/CTS1R");
            PutText(8, 39, batchNumber, ReflectionInterface.Key.Enter);

            if (int.Parse(GetText(10, 18, 8).Replace(",", "")) != int.Parse(GetText(9, 57, 8).Replace(",", "")))
                return false;
            else if (decimal.Parse(GetText(11, 27, 13).Replace(",", "")) != decimal.Parse(GetText(10, 66, 13).Replace(",", "")))
                return false;
            else
            {
                Hit(ReflectionInterface.Key.F10);
                Hit(ReflectionInterface.Key.PrintScreen);
                return true;
            }
        }

        private void TS1JError(string message, string borrowersLastName, int row)
        {
            Hit(ReflectionInterface.Key.F2);
            PutText(22, 17, GetText(row, 2, 2), ReflectionInterface.Key.F4);
            PutText(11, 17, borrowersLastName);
            PutText(19, 2, message, ReflectionInterface.Key.Enter);
            Hit(ReflectionInterface.Key.F12);
            Hit(ReflectionInterface.Key.Enter);
        }
    }
}
