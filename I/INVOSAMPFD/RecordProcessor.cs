using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using System.Diagnostics;
using Uheaa.Common.ProcessLogger;

namespace INVOSAMPFD
{
    class RecordProcessor
    {
        private ReflectionInterface ri;
        private CsvRecord[] records;
        private string ssn;
        private ProcessLogData pld;
        public RecordProcessor(ReflectionInterface ri, string ssn, CsvRecord[] borrowerRecords, ProcessLogData pld)
        {
            this.ri = ri;
            this.records = borrowerRecords;
            this.ssn = ssn;
            this.pld = pld;
        }
        public Report Process()
        {
            Report report = new Report(ssn);
            AddUserInfoTable(report);

            //gather all methods starting with B##.
            var bMethods = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(o => IsStep(o.Name));
            foreach (var methodInfo in bMethods.OrderBy(o => o.Name)) //process them in alphabetical order, B01, B02, B03...
            {
                //get the actual method
                var step = (Step)Delegate.CreateDelegate(typeof(Step), this, methodInfo.Name);
                //get the section header
                var description = methodInfo.GetCustomAttribute<DescriptionAttribute>();
                //find any records that match this step
                string bCode = methodInfo.Name.Substring(0, 3); //B01, B02, etc.
                bCode = bCode.Substring(1); //remove the B, new spec change has us working with only the numeric portion of the codes
                var codeOverrideAttr = methodInfo.GetCustomAttribute<CodeOverrideAttribute>();
                if (codeOverrideAttr != null)
                    bCode = codeOverrideAttr.Code; //this method has a custom code
                var record = records.FirstOrDefault(o => o.PerformanceCategoryBilled == o.PerformanceCategoryLoan && o.PerformanceCategoryLoan == bCode);
                if (record != null)
                {
                    //create a section for this step
                    var section = report.AddSections(description.Description);
                    //execute the step
                    try
                    {
                        if (!step(record, section))
                            report.HasErrors = true;
                    }
                    catch (Exception ex)
                    {
                        report.HasErrors = true;
                        section.AddError(string.Format("ERROR\r\nLine {0}: {1}\r\n{2}", record.LineNumber, record.LineContent, ex.Message));
                        ProcessLogger.AddNotification(pld.ProcessLogId, "Error in method " + methodInfo.Name, NotificationType.ErrorReport, NotificationSeverityType.Warning, Assembly.GetExecutingAssembly(), ex);
                    }
                }
            }
            if (report.Sections.Count == 1) //only the list of SSNs are present, no methods were executed for this borrower
            {
                var section = report.AddSections("ERROR");
                string message = string.Format("ERROR\r\nNo records for this borrower had matching Performance Category Billed and Performance Category Loan.  Expected at least one matching record.");
                section.AddError(message);
                ProcessLogger.AddNotification(pld.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning, Assembly.GetExecutingAssembly());
            }
            return report;
        }

        /// <summary>
        /// Each borrower section of the report starts with a table of their rows imported from the file.
        /// </summary>
        private void AddUserInfoTable(Report report)
        {
            var userInfoSection = report.AddSections(null);
            var userInfoTable = userInfoSection.CreateTable("Borrower Information");
            userInfoTable.AddRow("Borrower SSN", "Performance Category Billed", "Performance Category Loan", "Loan Sequence", "Principal Balance", "Interest Balance", "Total Principal and Interest", "Max Days Delinquent", "PIF Date");
            foreach (var record in records)
            {
                userInfoTable.AddRow(record.BorrowerSsn, record.PerformanceCategoryBilled, record.PerformanceCategoryLoan, record.LoanSequence.ToString(), record.PrincipalBalance, record.InterestBalance, record.TotalPrincipalAndInterest, record.MaxDaysDelinquent, record.PifDate);
            }
        }

        #region Common Step Methods
        private bool Tsx29(CsvRecord record, ReportSection section, string description, string validErrorCode = null)
        {
            ri.FastPath("TX3Z/ITS26" + record.BorrowerSsn);
            if (Error(record, section, validErrorCode: validErrorCode))
                return false;
            if (ri.ScreenCode == "TSX28") //manually select loan sequence
            {
                PageHelper.Iterate(ri, (row, settings) =>
                {
                    var sequence = ri.GetText(row, 14, 4).ToIntNullable() ?? -1;
                    int selectionNumber = ri.GetText(row, 1, 3).ToInt();
                    if (sequence == record.LoanSequence)
                    {
                        ri.PutText(21, 12, selectionNumber.ToString(), Key.Enter);
                        settings.ContinueIterating = false;
                    }
                });
                if (Error(record, section, validErrorCode: validErrorCode))
                    return false;
            }
            section.AddScreenshot(ri.ReflectionSession);
            section.AddDescription(description);
            TableGenerator tg = new TableGenerator();
            tg.GenerateSeparationSourceTable(section);
            tg.GenerateSeparationReasonTable(section);

            return true;
        }
        private bool Tsx29_Tsx2B(CsvRecord record, ReportSection section, string tsx29Description, string tsx2bDescription, string validErrorCode = null)
        {
            if (!Tsx29(record, section, tsx29Description, validErrorCode: validErrorCode))
                return false;

            ri.Hit(Key.Enter);
            ri.Hit(Key.Enter);
            if (Error(record, section, validErrorCode: validErrorCode))
                return false;

            section.AddScreenshot(ri.ReflectionSession);
            section.AddDescription(tsx2bDescription);

            return true;
        }

        private bool Tsx29_Tsx31(CsvRecord record, ReportSection section, string tsx29Description, string tsx31Description)
        {
            if (!Tsx29(record, section, tsx29Description))
                return false;

            ri.Hit(Key.F2);
            ri.Hit(Key.F7);
            if (Error(record, section))
                return false;
            section.AddScreenshot(ri.ReflectionSession);
            section.AddDescription(tsx31Description);

            return true;
        }
        #endregion
        #region Steps
        private delegate bool Step(CsvRecord record, ReportSection section);
        [Description("01 (In School)")]
        private bool B01InSchool(CsvRecord record, ReportSection section)
        {
            string description = "[This screen shot displays the borrower’s current status as well as the enrollment status effective date, separation date, separation reason, separation source, and grace end date if applicable/known.]";
            if (!Tsx29(record, section, description))
                return false;
            return true;
        }
        [Description("02 (In Grace)")]
        private bool B02InGrace(CsvRecord record, ReportSection section)
        {
            string description = "[This screen shot displays the borrower’s current status as well as the enrollment status effective date, separation date, separation reason, separation source, and grace end date.]";
            if (!Tsx29(record, section, description))
                return false;
            return true;
        }
        [Description("03 (In Repayment)")]
        private bool B03InRepayment(CsvRecord record, ReportSection section)
        {
            string firstDescription = "[This screen shot displays the borrower’s current status as well as the enrollment status effective date, separation date, separation reason, separation source, and grace end date.]";
            string secondDescription = "[This screen shot displays the borrower’s current number of days delinquent (if applicable) as well as the recent billing information and last financial activity.]";
            if (!Tsx29_Tsx2B(record, section, firstDescription, secondDescription))
                return false;

            return true;
        }
        [Description("04 (Service Member)")]
        private bool B04ServiceMember(CsvRecord record, ReportSection section)
        {
            ri.FastPath("TX3Z/ITD2A" + record.BorrowerSsn);
            if (Error(record, section))
                return false;
            ri.PutText(11, 65, "ASCRA", Key.Enter);
            if (Error(record, section))
                return false;

            if (ri.ScreenCode == "TDX2C")
                ri.PutText(7, 2, "X", Key.Enter);

            section.AddScreenshot(ri.ReflectionSession);
            string description = "This screen shot displays a note placed on a borrower’s account when they are found eligible under SCRA. The begin date and end date for the status should be included in the comment text. If the end date of the status is not known for a borrower the comment may reflect and end date of 99/99/9999.]";
            section.AddDescription(description);

            return true;
        }
        [Description("05 (In Deferment)")]
        private bool B05InDeferment(CsvRecord record, ReportSection section)
        {
            string firstDescription = "[This screen shot displays the borrower’s current status as well as the enrollment status effective date, separation date, separation reason, separation source, and grace end date.]";
            string secondDescription = "[This screen shot displays the borrower’s Deferment/Forbearance history. Displayed is the Deferment/Forbearance type, begin date, end date, and certification dates.]";
            if (!Tsx29_Tsx31(record, section, firstDescription, secondDescription))
                return false;

            return true;
        }
        [Description("06 (In Forbearance)")]
        private bool B06InForbearance(CsvRecord record, ReportSection section)
        {
            string firstDescription = "[This screen shot displays the borrower’s current status as well as the enrollment status effective date, separation date, separation reason, separation source, and grace end date.]";
            string secondDescription = "[This screen shot displays the borrower’s Deferment/Forbearance history. Displayed is the Deferment/Forbearance type, begin date, end date, and certification dates.]";
            if (!Tsx29_Tsx31(record, section, firstDescription, secondDescription))
                return false;

            return true;
        }
        [Description("07 (Delinquent 6-30 Days)")]
        private bool B07Delinquent6To30(CsvRecord record, ReportSection section)
        {
            return Delinquent(record, section);
        }
        [Description("08 (Delinquent 31-90 Days)")]
        private bool B08Delinquent31To90(CsvRecord record, ReportSection section)
        {
            return Delinquent(record, section);
        }
        [Description("09 (Delinquent 91-150 Days)")]
        private bool B09Delinquent91To150(CsvRecord record, ReportSection section)
        {
            return Delinquent(record, section);
        }
        [Description("10 (Delinquent 151-270 Days)")]
        private bool B10Delinquent151To270(CsvRecord record, ReportSection section)
        {
            return Delinquent(record, section);
        }
        [Description("11 (Delinquent 271-360 Days)")]
        private bool B11Delinquent271To360(CsvRecord record, ReportSection section)
        {
            return Delinquent(record, section, validErrorCode: "03363");
        }
        [Description("12 (Delinquent >360 Days)")]
        private bool B12DelinquentGreaterThan360(CsvRecord record, ReportSection section)
        {
            return SecondaryDelinquent(record, section);
        }
        [Description("PIF"), CodeOverride("PIF")]
        private bool B13Pif(CsvRecord record, ReportSection section)
        {
            return SecondaryDelinquent(record, section);
        }
        [Description("TRN"), CodeOverride("TRN")]
        private bool B14Trn(CsvRecord record, ReportSection section)
        {
            return SecondaryDelinquent(record, section);
        }

        private bool SecondaryDelinquent(CsvRecord record, ReportSection section)
        {
            string firstDescription = "[This screen shot displays the borrower’s current status as well as the enrollment status effective date, separation date, separation reason, separation source, and grace end date. *Note: The Litigation status is only used for the special scenario of borrowers who cannot be transferred to DMCS but have reached the delinquency stage normally associated with a DMCS transfer.]";
            return Delinquent(record, section, firstDescription, validErrorCode: "03363");
        }
        private bool Delinquent(CsvRecord record, ReportSection section, string firstDescription = null, string validErrorCode = null)
        {
            firstDescription = firstDescription ?? "[This screen shot displays the borrower’s current status as well as the enrollment status effective date, separation date, separation reason, separation source, and grace end date.]";
            string secondDescription = "[This screen shot displays the borrower’s current number of days delinquent (if applicable) as well as the recent billing information and last financial activity.]";
            if (!Tsx29_Tsx2B(record, section, firstDescription, secondDescription, validErrorCode: validErrorCode))
                return false;
            return true;
        }
        #endregion

        /// <summary>
        /// Evaluates the call stack to see if we're currently in a method starting with B##.  If so, return that B## code.
        /// </summary>
        /// <returns>A code like B## or null if inapplicable.</returns>
        private string GetCurrentStepCode()
        {
            StackTrace st = new StackTrace();
            foreach (var frame in st.GetFrames())
            {
                string methodName = frame.GetMethod().Name;
                if (IsStep(methodName))
                    return methodName.Substring(0, 3);
            }
            return null;
        }

        /// <summary>
        /// Returns true if the given method name starts with a B and two digits.
        /// </summary>
        private bool IsStep(string methodName)
        {
            return methodName[0] == 'B' && char.IsDigit(methodName[1]) && char.IsDigit(methodName[2]);
        }
        /// <summary>
        /// If RI.MessageCode is populated, log an error and return true.  Otherwise, return false;
        /// </summary>
        private bool Error(CsvRecord record, ReportSection section, string validErrorCode = null)
        {
            if (string.IsNullOrEmpty(ri.Message) || ri.MessageCode == validErrorCode)
                return false;
            string message = string.Format("ERROR\r\nLine {0}: {1}\r\n{2}", record.LineNumber, record.LineContent, ri.Message);
            section.AddScreenshot(ri.ReflectionSession);
            section.AddError(message);
            string plMessage = string.Format("{0} - Borrower {1}, {2}: Received error {3}", GetCurrentStepCode(), record.BorrowerSsn, ri.ScreenCode, ri.Message);
            ProcessLogger.AddNotification(pld.ProcessLogId, plMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning, Assembly.GetExecutingAssembly(), null);
            return true;
        }

        private class CodeOverrideAttribute : Attribute
        {
            public CodeOverrideAttribute(string code)
            {
                Code = code;
            }
            public string Code { get; set; }
        }
    }
}
