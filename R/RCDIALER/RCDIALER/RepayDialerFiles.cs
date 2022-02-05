using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLoggerRC;
using static System.Console;

namespace RCDIALER
{
    public class RepayDialerFiles
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public string FileName { get; set; }
        public int Count { get; set; } = 0;

        public RepayDialerFiles(ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(LogRun.LDA);
        }

        public void Process(string overrideSproc = null)
        {
            List<Sprocs> sprocs = DA.GetSprocs();
            if (overrideSproc.IsPopulated())
                DA.LoadCalls(overrideSproc);
            else
            {
                foreach (Sprocs sproc in sprocs)
                    DA.LoadCalls(sproc.SprocName);
            }
            List<DialerData> data = DA.GetDialerData();
            if (data == null || data.Count == 0)
            {
                string message = "There are no records in the Voyager database to create the RC_OB_AutoBlast file.";
                LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Warning);
                WriteLine(message);
                return;
            }

            try
            {
                if (CreateFile(data))
                {
                    WriteLine("Setting ProcessedAt for each record.");
                    //Set all the records ProcessedAt field
                    Parallel.ForEach(data, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, d => DA.SetProcessedAt(d.OutboundCallsId));
                    WriteLine($"Processing complete. New file is in {FileName}");
                }
            }
            catch (Exception ex)
            {
                string message = $"There was an error creating the file: {FileName}. EX: {ex.Message}. Please review the Process Log error to determine the cause.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                WriteLine(message);
            }
        }

        private bool CreateFile(List<DialerData> data, string file = null)
        {
            try
            {
                FileName = file ?? $"{EnterpriseFileSystem.FtpFolder}{DateTime.Now.ToShortDateString().Replace("/", "")}.RC_OB_AutoBlast.csv";
                WriteLine($"Createing file {FileName} with {data.Count} records.");
                using StreamWriter sw = new StreamWriter(FileName);
                //Header
                sw.WriteLine("RC_id|Servicer_id|firstname|lastname|Cust_Address_Line1|Cust_Address_Line2|City|state|zip_code|email_address|home_Phone|" +
                    "Work_Phone|cell_phone|monthly_repayment_amount|school_code|School_Name|days_delinquent|delinquency_bucket");
                foreach (DialerData d in data)
                {
                    sw.WriteLine($"{d.RCID}|{d.ServicerId}|{d.FirstName}|{d.LastName}|{d.Address1}|{d.Address2}|{d.City}|{d.State}|" +
                        $"{d.Zip}|{d.Email}|{d.HomePhone}|{d.WorkPhone}|{d.CellPhone}|{d.MonthlyRepaymentAmount.FormatAmount()}|{d.SchoolCode}|" +
                        $"{d.SchoolName}|{d.DaysDelinquent}|{d.DelinquentBucket}");
                }
                return true;
            }
            catch (IOException ex)
            {
                if (Count == 5) //If it keeps creating an IO exception, throw a new exception to get out of the recursive loop
                    throw new Exception(ex.Message);
                else
                {
                    string additionalFile = $"{EnterpriseFileSystem.FtpFolder}{DateTime.Now.ToShortDateString().Replace("/", "")}.RC_OB_AutoBlast_{++Count}.csv";
                    WriteLine($"The {FileName} is currently open. A new file will be created named {additionalFile}.");
                    return CreateFile(data, additionalFile); //Using recursion to create a new file if there is an IO exception
                }
            }
        }
    }

    public static class Decimals
    {
        public static string FormatAmount(this string field)
        {
            if (field.ToInt() > 0)
                return field.Insert(field.Length - 2, ".").ToDecimal().ToString();
            else if (field.Contains("."))
                return field;
            else
                return "0.00";
        }
    }
}