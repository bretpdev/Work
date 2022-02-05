using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace IPACPMTFED
{
    public class IpacPaymentPostingFed : FedScript
    {
        public IpacPaymentPostingFed(ReflectionInterface ri)
            : base(ri, "IPACPMTFED")
        {

        }

        public override void Main()
        {
            Application.EnableVisualStyles();
            ScheduleData data = ShowEntryForm();
            
            CreateScheduleInformation(data);
            string fileToProcess = GetAndValidateFiles();

            List<FileData> dataFromFile = ReadCsvFile(data.ScheduleAmount, fileToProcess);
            CreatePostingBatch(dataFromFile, data);

            File.Delete(fileToProcess);
            ProcessingComplete();
        }

        /// <summary>
        /// Displays Processing Complete and ends the script.
        /// </summary>
        private void ProcessingComplete()
        {
            MessageBox.Show("Processing Complete.");
            EndDllScript();
        }

        /// <summary>
        /// Gets the file to process and checks to ensure only 1 file exists.
        /// </summary>
        /// <returns>FIle to process.</returns>
        private string GetAndValidateFiles()
        {
            string[] fileToProcess = Directory.GetFiles(EnterpriseFileSystem.TempFolder, "IPAC Payments*");

            if (fileToProcess.Count() > 1)
            {
                MessageBox.Show("Multiple IPAC Payments.csv files exist.  Please resolve and try again.");
                EndDllScript();
            }
            else if (fileToProcess.Count() < 1)
            {
                MessageBox.Show("IPAC Payments.csv file was not found.  Please resolve and try again.");
                EndDllScript();
            }

            return fileToProcess.Single();
        }

        /// <summary>
        /// Reads the file and loads it into an object.
        /// </summary>
        /// <param name="scheduleAmount">Schedule amount.</param>
        /// <param name="fileToProcess">File to read.</param>
        /// <returns></returns>
        private List<FileData> ReadCsvFile(string scheduleAmount, string fileToProcess)
        {
            using (Processing pro = new Processing(fileToProcess))
            {
                pro.Process();

                if (pro.Data.Count < 1)
                {
                    MessageBox.Show("The IPAC Payments.csv file on your T drive was empty.  Please investigate and try again.");
                    EndDllScript();
                }

                decimal total = pro.Data.Sum(p => p.PaymentAmount);

                if (total != decimal.Parse(scheduleAmount))
                {
                    MessageBox.Show("The sum of all groups does not match the schedule amount entered.  Please investigate and try again.");
                    EndDllScript();
                }

                return pro.Data;
            }
        }

        /// <summary>
        /// Shows the a data entry form the user will have to fill out.
        /// </summary>
        /// <returns>ScheduleData object.</returns>
        private ScheduleData ShowEntryForm()
        {
            ScheduleData data = new ScheduleData();

            FormBuilder form = new FormBuilder("Payment Posting");
            form.InputWidth = 300;
            form.AcceptButtonText = "OK";
            using (var frm = FormBuilder.Generate(data, form))
            {
                frm.ShowIcon = false;

                if (frm.ShowDialog() == DialogResult.Cancel)
                    EndDllScript();

                return data;
            }
        }

        /// <summary>
        /// Creates the schedule on TL2M.
        /// </summary>
        /// <param name="data">ScheduleData object</param>
        private void CreateScheduleInformation(ScheduleData data)
        {
            if (!CheckScheduleExist(data))
            {
                FastPath("TX3Z/ATL2M");
                EnterDataOnTl2m(data, string.Empty);
                if (!CheckForText(23, 2, "01004") && RI.MessageCode != "06600")
                {
                    MessageBox.Show("An error has occurred.  Please fix the error and press insert to continue the script.");
                    PauseForInsert();
                }
            }
        }

        /// <summary>
        /// Enters data on TL2M
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        private void EnterDataOnTl2m(ScheduleData data, string message)
        {
            PutText(9, 11, data.ScheduleNumber, true);
            PutText(9, 35, "02", true);
            PutText(9, 59, data.FormattedScheduleDate, true);
            PutText(10, 11, data.ScheduleAmount, true);
            PutText(15, 15, "IPC", true);
            PutText(16, 15, data.SendingServicer, true);
            PutText(16, 55, "I", true);
            PutText(17, 15, data.InvoiceNumber, true);
            if (!message.IsNullOrEmpty())
                PutText(20, 2, message, true);

            Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Checks that the data from the file matches the data entered by the user.
        /// </summary>
        /// <param name="data">ScheduleData object.</param>
        /// <returns>List of strings with errors.</returns>
        private List<string> CheckErrors(ScheduleData data)
        {
            List<string> errors = new List<string>();

            if (decimal.Parse(GetText(10, 11, 15).Replace(",", "")) != decimal.Parse(data.ScheduleAmount.Replace(",", "")))
                errors.Add("The schedule amount entered does not match the schedule amount in the schedule number.");
            if (!CheckForText(15, 15, "IPC"))
                errors.Add(@"Inconsistent data found, the SOURCE should be ""IPC"".");
            if (!data.SendingServicer.ToUpper().Contains(GetText(16, 15, 3)))
                errors.Add("The sending servicer entered does not match the sending servicer in the schedule number.");
            if (!data.InvoiceNumber.Contains(GetText(17, 15, 20)))
                errors.Add("The Invoice # entered does not match the Invoice # in the schedule number.");

            return errors;
        }

        /// <summary>
        /// Checks TL2M to see if a schedule already exists.
        /// </summary>
        /// <param name="data">ScheduleData object.</param>
        /// <returns>True if a schedules exists.</returns>
        private bool CheckScheduleExist(ScheduleData data)
        {
            FastPath("TX3Z/ITL2M");
            PutText(7, 36, data.ScheduleNumber, ReflectionInterface.Key.Enter, true);

            if (!CheckForText(1, 71, "TLX2O"))
                return false;

            bool firstTry = false;
            while (true)
            {
                if (!CheckForText(9, 35, "02"))
                    return false;

                List<string> errors = CheckErrors(data);

                if (errors.Count == 0)
                    return true;
                else
                {
                    if (!firstTry)
                    {
                        MessageBox.Show("Please review the following errors: \n" + string.Join("\n", errors.Select(e => " - " + e).ToArray()),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                        PauseForInsert();
                        firstTry = true;
                    }
                    else
                    {
                        //The user still did not enter the correct information so the script will now do it.
                        PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                        EnterDataOnTl2m(data, "correcting schedule information");

                        if (CheckForText(23, 2, "01005"))
                            return true;
                        else
                        {
                            MessageBox.Show("An Error has occurred.  Please correct the error and press Insert to continue the script.");
                            PauseForInsert();
                            return true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a posting batch for payments.
        /// </summary>
        /// <param name="data">Data from file.</param>
        /// <param name="batchInfo">ScheduleData object</param>
        private void CreatePostingBatch(List<FileData> data, ScheduleData batchInfo)
        {
            //Compass only allows 9999 payments in a batch.
            if (data.Count > 9999)
            {
                int numberOfBatches = data.Count / 9999;
                if (data.Count % 9999 != 0)
                    numberOfBatches++;

                for (int i = 0; i < numberOfBatches; i++)
                    CreateTheBatch(data.Skip(i * 9999).Take(9999).ToList(), batchInfo, false);
            }
            else
            {
                CreateTheBatch(data, batchInfo, true);
            }

        }

        /// <summary>
        /// Creates the schedule batch on TS1G.
        /// </summary>
        /// <param name="data">Data from file.</param>
        /// <param name="batchInfo">ScheduleData object</param>
        /// <param name="singleBatch">Indicator if we need to create multiple batches</param>
        private void CreateTheBatch(List<FileData> data, ScheduleData batchInfo, bool singleBatch)
        {
            FastPath("TX3Z/ATS1G");

            if (CheckForText(23, 2, "01473"))
                PutText(10, 32, " ", ReflectionInterface.Key.Enter, true);

            PutText(6, 51, "2");
            PutText(10, 28, data.Count.ToString());
            PutText(11, 28, data.Sum(p => p.PaymentAmount).ToString());
            PutText(12, 28, "10");
            PutText(13, 28, DateTime.Now.ToString("MMddyy"));
            PutText(15, 28, UserId);
            PutText(20, 12, batchInfo.ScheduleNumber);
            PutText(20, 35, "02");
            PutText(21, 15, batchInfo.FormattedScheduleDate.Remove(4, 2), ReflectionInterface.Key.Enter);

            if (RI.MessageCode != "01004" && RI.MessageCode != "06600")
            {
                MessageBox.Show("An error has occurred.  Please fix the error and press insert to continue the script.");
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

        /// <summary>
        /// Enter Individual payments for a batch. 
        /// </summary>
        /// <param name="data">Data from the file.</param>
        /// <param name="batchNumber">Batch number.</param>
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

                PutText(row, 5, fileLine.Ssn);
                PutText(row, 17, fileLine.PaymentAmount.ToString());
                PutText(row, 28, fileLine.PaymentEffectiveDate, ReflectionInterface.Key.Enter);

                ValidateMessageCode(fileLine, row);

                row++;
            }
        }

        /// <summary>
        /// Validates the payment was added.
        /// </summary>
        /// <param name="fileLine">file data.</param>
        /// <param name="row">Current row in the session.</param>
        private void ValidateMessageCode(FileData fileLine, int row)
        {
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
                TS1JError("All loans deconverted", fileLine.BorrowersLastName, row);
                Hit(ReflectionInterface.Key.F2);
            }
        }

        /// <summary>
        /// Verifies that the batches are in sync.
        /// </summary>
        /// <param name="batchNumber">Batch Number</param>
        /// <param name="data">Data from the file.</param>
        /// <returns>True if the are in sync.</returns>
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

        /// <summary>
        /// Corrects errors on TS1J.
        /// </summary>
        /// <param name="message">Error correction message.</param>
        /// <param name="borrowersLastName">Borrowers last name.</param>
        /// <param name="row">Current row in the session.</param>
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
