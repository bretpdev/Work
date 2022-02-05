using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OLPAYREVR
{
    public class FileLoader
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        public string FileName { get; private set; } = null;
        public List<string> FileLoadingErrors { get; set; } = new List<string>(); //TODO: Waiting on BA to see if we actually want to write this out to a file or just look at PL

        public FileLoader(ProcessLogRun logRun, DataAccess da)
        {
            LogRun = logRun;
            DA = da;
        }

        public bool ReadAndLoadFile()
        {
            List<Payment> data = GetDataFromFile();
            if (data != null && data.Count > 0)
            {
                if (IsValidInput(data))
                {
                    bool loadResult = true;
                    DateTime? createdAt = DateTime.Now;
                    foreach (Payment p in data)
                    {
                        if (!DA.AddRecord(p, createdAt))
                        {
                            loadResult &= false;
                            LogRun.AddNotification($"Error trying to add record {p} to database", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        }
                    }

                    if (!loadResult)
                        Dialog.Warning.Ok($"There was an error when uploading the file to the database. Please consult System Support for PL # {LogRun.ProcessLogId}");

                    return loadResult;
                }
            }
            return false;
        }

        private List<Payment> GetDataFromFile()
        {
            FileName = GetUserFileSelection();
            if (FileName == null)
                return null;

            try
            {
                var connString = $"Provider=Microsoft.ACE.OLEDB.12.0; data source={FileName}; Extended Properties='Excel 12.0;'";
                using (var adapter = new OleDbDataAdapter("SELECT [SSN],[PAYMENT AMOUNT],[PAYMENT DATE],[PAYMENT TYPE] FROM [Sheet1$]", connString))
                {
                    var ds = new DataSet();

                    adapter.Fill(ds, "Payment");

                    var data = ds.Tables["Payment"].AsEnumerable();


                    string textInputError = CheckTextTypeInput(data); // Check if user converted columns to "Text" data type
                    string originalTypesInputError = CheckOriginalTypesInput(data); // Check if user used original data types from provided example file
                    bool userInputIsCorrectType = textInputError == null || originalTypesInputError == null;
                    
                    if (!userInputIsCorrectType)
                    {
                        string error = $"Error reading input file {FileName}. Columns need to either be set to Text data type in Excel or the following columns need to be set to the following data type:\n \n SSN: Special \n Payment Amount: Currency \n Payment Date: Special \n Payment Type: General";
                        LogRun.AddNotification($"{textInputError} \n {originalTypesInputError} \n {error}", NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
                        Dialog.Error.Ok(error);
                        return null;
                    }

                    EnumerableRowCollection<Payment> query = null;
                    if (textInputError == null) // User had columns set to text in Excel doc, hence no error
                    {   
                        query = data.Where(r => r.ItemArray[0].GetType().Name != "DBNull").Select(p => 
                            new Payment
                            {
                                Ssn = p.Field<string>("SSN").Trim(),
                                PaymentAmount = p.Field<string>("PAYMENT AMOUNT").ToDouble(),
                                PaymentEffectiveDate = ConvertToDate(p.Field<string>("PAYMENT DATE").Trim()),
                                PaymentType = p.Field<string>("PAYMENT TYPE").Trim()
                            }
                        );
                    }
                    else // User had columns set to original types in Excel doc
                    {
                        query = data.Where(r => r.ItemArray[0].GetType().Name != "DBNull").Select(p =>
                            new Payment
                            {
                                Ssn = ReconcileSsnField(p.Field<double?>("SSN").ToString()),
                                PaymentAmount = (double?)p.Field<decimal?>("PAYMENT AMOUNT"),
                                PaymentEffectiveDate = ConvertToDate(p.Field<double?>("PAYMENT DATE").ToString().Trim()),
                                PaymentType = p.Field<string>("PAYMENT TYPE").Trim()
                            }
                        );
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "No value given for one or more required parameters.")
                {
                    string error = $"Error reading input file {FileName}. One or more columns were either missing or had a typo in their header field. Please fix this and re-run the script.";
                    LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    Dialog.Error.Ok(error);
                    return null;
                }
                LogRun.AddNotification($"Error reading input file {FileName}. File was empty or did not contain correctly formatted data.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Dialog.Error.Ok($"Error reading input file {FileName}. File was empty or did not contain correctly formatted data.");
                return null;
            }
        }

        private string ReconcileSsnField(string s)
        {
            int diff = 9 - s.Length;
            for (int i = 0; i < diff; i++)
                s = $"0{s}";
            return s;
        }

        private string CheckOriginalTypesInput(EnumerableRowCollection<DataRow> data)
        {
            int rowNum = 0;
            bool userInputIsOriginalTypes = true;
            foreach (var row in data)
            {
                rowNum++;
                if (row.ItemArray[0].GetType().Name != "Double" && row.ItemArray[0].GetType().Name != "DBNull")
                    userInputIsOriginalTypes = false;

                if (row.ItemArray[1].GetType().Name != "Decimal" && row.ItemArray[1].GetType().Name != "DBNull")
                    userInputIsOriginalTypes = false;

                if (row.ItemArray[2].GetType().Name != "Double" && row.ItemArray[2].GetType().Name != "DBNull")
                    userInputIsOriginalTypes = false;

                if (row.ItemArray[3].GetType().Name != "String" && row.ItemArray[3].GetType().Name != "DBNull")
                    userInputIsOriginalTypes = false;

                if (row.ItemArray.Length > 4)
                    userInputIsOriginalTypes = false;

                if (!userInputIsOriginalTypes)
                    return $"Data type mismatch for original types occurs on line {++rowNum}";
            }
            return null;
        }

        private string CheckTextTypeInput(EnumerableRowCollection<DataRow> data)
        {
            int rowNum = 0;
            foreach (var row in data)
            {
                rowNum++;
                foreach (var cell in row.ItemArray)
                {
                    if (cell.GetType().Name != "String" && cell.GetType().Name != "DBNull")
                        return $"Data type mismatch on line {++rowNum}";
                }
            }
            return null;
        }

        private string GetUserFileSelection()
        {
            string fileName = null;
            while (fileName == null)
            {
                using (OpenFileDialog filePicker = new OpenFileDialog())
                {
                    if (filePicker.ShowDialog() != DialogResult.OK)
                        return null;

                    fileName = filePicker.FileName;
                }
            }
            return fileName;
        }

        private DateTime? ConvertToDate(string text)
        {
            if (text.Length == 7)
                text = $"0{text}"; //Handles Excel chopping off leading zero
            return text.ToDateNullable();
        }

        private bool NoValidInputFileExists(List<string> files)
        {
            if (files.Count() > 1)
            {
                string message = "More than one input file was found in the folder. Extra file must be removed before re-running. Ending script run.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message);
                return true;
            }
            if (files.Count() == 0)
            {
                string message = "No input file was found. Please add the file and then re-run the script. Ending script run.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message);
                return true;
            }
            return false;
        }

        private bool IsValidInput(List<Payment> data)
        {
            bool dataIsValid = true;
            foreach (Payment p in data)
            {
                string invalidData = InvalidData(p);
                if (invalidData != "")
                {
                    dataIsValid = false;
                    string error = $"Data validation error for input file {FileName}. For payment record {p} the following issue(s) were found: {invalidData}";
                    FileLoadingErrors.Add(error);
                    LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }

            if (!dataIsValid)
                Dialog.Error.Ok($"One or more records in the spreadsheet had invalid data. Please consult System Support, as the errors were written out to PL # {LogRun.ProcessLogId}");

            return dataIsValid;
        }

        private string InvalidData(Payment payment)
        {
            string invalidDataComment = "";
            if (string.IsNullOrWhiteSpace(payment.Ssn) || payment.Ssn.Length != 9)
                invalidDataComment += "The SSN was invalid. It must be nine numerical characters. ";

            if (!payment.PaymentAmount.HasValue)
                invalidDataComment += "The PAYMENT AMOUNT was invalid. ";

            if (!payment.PaymentEffectiveDate.HasValue)
                invalidDataComment += "The PAYMENT DATE was invalid. ";

            if (string.IsNullOrWhiteSpace(payment.PaymentType) || payment.PaymentType.Length != 2)
                invalidDataComment += "The PAYMENT TYPE was invalid. ";

            return invalidDataComment;
        }
    }
}
