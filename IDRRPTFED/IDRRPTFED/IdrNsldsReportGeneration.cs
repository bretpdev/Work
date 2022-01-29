using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace IDRRPTFED
{
    public class IdrNsldsReportGeneration
    {
        public DataAccess DA { get; private set; }
        public IdrNsldsReportGeneration(DataAccess da)
        {
            DA = da;
        }

        public int Run()
        {
            RunHistory selectedRun = null;
            using (SelectMonth selector = new SelectMonth())
            {
                if (Arguments.ShowPrompts)
                {
                    if (selector.ShowDialog() == DialogResult.Cancel)
                        return Program.FAILURE;
                }

                selectedRun = selector.SelectedRun;
            }

            try
            {
                GenerateTheFile(selectedRun.StartDate, selectedRun.EndDate);

                //Update the database with the new end date
                DataAccess.SetRunDate(DateTime.Now, selectedRun.StartDate, selectedRun.EndDate, selectedRun.DateID);
            }
            catch (Exception ex)
            {
                string comment = string.Format($"There was an error running the script with a date range selection of {selectedRun.StartDate} to {selectedRun.EndDate}. Please check Process Logger for further details.");
                NotifyUserAndLogCriticalError(ex, comment);
                return Program.FAILURE;
            }

            return Program.SUCCESS;
        }

        /// <summary>
        /// This method is only for testing purposes so that specific application IDs can be reported rather than a date range.  Uses the commandline arguments to specify which applications to report.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public int Run(List<string> arguments)
        {
            List<int> appIds = new List<int>();
            try
            {
                for (int i = 0; i < arguments.Count(); i++)
                    appIds.Add(int.Parse(arguments[i]));

                GenerateTheFile(null, null, appIds);
            }
            catch (Exception ex)
            {
                string comment = string.Format($"There was an error running the script with the application IDs {appIds} passed through the command-line-arguments. Please check Process Logger.");
                NotifyUserAndLogCriticalError(ex, comment);
                return Program.FAILURE;
            }

            return Program.SUCCESS;
        }

        /// <summary>
        /// Instead of reporting all applications in a date range, this is a secondary option to only report specific applications found in a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public int Run(string fileName)
        {
            try
            {
                List<int> appIds = new List<int>();
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        appIds.Add(int.Parse(line));
                    }
                }

                GenerateTheFile(null, null, appIds);
            }
            catch (Exception ex)
            {
                string comment = string.Format($"There was an error running the script using the input file {fileName}. Common errors include: (a) An incorrect file path and (b) The application IDs are not delimited by line breaks in the file. Please check Process Logger for further details.");
                NotifyUserAndLogCriticalError(ex, comment);
                return Program.FAILURE;
            }

            return Program.SUCCESS;
        }

        /// <summary>
        /// Pulls all of the data from the database for the selected run and creates the NSLDS file. Has an optional parameter of appIds so that specific applications can be reported.
        /// </summary>
        /// <param name="start">Start period</param>
        /// <param name="end">End period</param>
        private void GenerateTheFile(DateTime? start, DateTime? end, List<int> appIds = null)
        {
            string file = string.Format("{0}NSLDS_FILE{1}", EnterpriseFileSystem.TempFolder, DateTime.Now.ToString("MMddyyy_hhmmss"));

            List<int> nullAwardIds = new List<int>();

            var grabber = new RecordDataGrabber();

            //If specific applications IDs are not passed in, grab all records from selected date range
            if (appIds == null)
                grabber.PopulateByDate(DA, start.Value, end.Value);
            else //If specific application IDs are passed in, add each of them to reporting lists
                grabber.PopulateByAppIds(DA, appIds);

            nullAwardIds.AddRange(grabber.NullAwardIds);

            CheckNullAwardIds(nullAwardIds);
            WriteRecords(file, grabber.AbData, grabber.BdData, grabber.BeData, grabber.BfData);
            MoveTheFile(file);
        }

        /// <summary>
        /// Move the file from the Users T drive to the Archive location.
        /// </summary>
        /// <param name="file">File to move</param>
        private void MoveTheFile(string file)
        {
            string fileForArchive = EnterpriseFileSystem.GetPath("NSLDS_IDR_ARCHIVE");

            try
            {
                File.Move(file, file.Replace(EnterpriseFileSystem.TempFolder, fileForArchive));
            }
            catch (Exception ex)
            {
                string comment = string.Format($"There was an error saving the file {fileForArchive}.  Please ensure this file is saved to archive.");
                string specificEmailComment = "Error saving Archive File";
                NotifyUserAndLogCriticalError(ex, comment, specificEmailComment);
            }
        }

        /// <summary>
        /// Check to see if there are any null award ids.  Will prompt the user if there are.
        /// </summary>
        /// <param name="nullAwardIds">List of null award ids</param>
        private void CheckNullAwardIds(List<int> nullAwardIds)
        {
            if (nullAwardIds.Count != 0)
            {
                string errorMessage = "The following application ids have a null award id: ";
                foreach (int item in nullAwardIds.Distinct())
                {
                    errorMessage += "\n" + item.ToString() + " ";
                    ProcessLogger.AddNotification(Program.LogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmailHelper.SendMail(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", Program.ScriptId + "@utahsbr.edu", "NULL Award IDs Found", errorMessage, string.Empty, string.Empty, EmailHelper.EmailImportance.High, true);
            }
        }

        /// <summary>
        /// Writes all records to the file.
        /// </summary>
        /// <param name="file">File to write to</param>
        /// <param name="abData">AB record data</param>
        /// <param name="bdData">BD record data</param>
        /// <param name="beData">BE record data</param>
        /// <param name="bfData">Bf record data</param>
        private void WriteRecords(string file, List<AbRecordData> abData, List<BdRecordData> bdData, List<BeRecordData> beData, List<BfRecordData> bfData)
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                WriteABRecords(abData, sw);
                WriteBDRecords(bdData, sw);
                WriteBERecords(beData, sw);
                WriteBFRecords(bfData, sw);
            }//end using
        }

        /// <summary>
        /// Writes BF records
        /// </summary>
        /// <param name="bfData"></param>
        /// <param name="sw"></param>
        private void WriteBFRecords(List<BfRecordData> bfData, StreamWriter sw)
        {
            foreach (BfRecordData item in bfData.Where(p => p.BFDisclosureDate != null))
            {
                sw.WriteLine(string.Format("BF{0}{1}{2}{3}{4}{5}",
                    item.AwardId,
                    FormatDate(item.BFDisclosureDate),
                    FormatAppId(item.ApplicationId, item.EApplicationId),
                    " ".PadRight(193),
                    item.LoanSeq,
                    " ".PadRight(11)).ToUpper());
            }
        }

        /// <summary>
        /// Writes BE records
        /// </summary>
        /// <param name="beData"></param>
        /// <param name="sw"></param>
        private void WriteBERecords(List<BeRecordData> beData, StreamWriter sw)
        {
            foreach (BeRecordData item in beData)
            {
                sw.WriteLine(string.Format("BE{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                        item.AwardId,
                        FormatAppId(item.ApplicationId, item.EApplicationId),
                        item.BERepaymentTypeProgram == null ? "  " : item.BERepaymentTypeProgram,
                        FormatBool(item.BERequestedByBorrower, "N"),
                        item.BERepaymentPlanTypeStatus == null ? "  " : item.BERepaymentPlanTypeStatus,
                        FormatDate(item.BERepaymentPlanTypeStausDate),
                        item.BEFamilySize.ToString().Length == 1 ? "0" + item.BEFamilySize.ToString() : item.BEFamilySize.ToString(),
                        FormatMoney(item.TotalIncome),
                        " ".PadRight(180),
                        item.LoanSeq,
                    " ".PadRight(11)).ToUpper());

                ReportNonProcessedPlans(item, sw);
            }
        }

        /// <summary>
        /// Writes BD records
        /// </summary>
        /// <param name="bdData"></param>
        /// <param name="sw"></param>
        private void WriteBDRecords(List<BdRecordData> bdData, StreamWriter sw)
        {
            foreach (BdRecordData item in bdData)
            {
                sw.WriteLine(string.Format("BD{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}{33}{34}{35}{36}{37}{38}{39}{40}",
                    item.AwardId,
                    FormatAppId(item.ApplicationId, item.EApplicationId),
                    FormatDate(item.BDApplicationReceivedDate),
                    item.BDRepaymentPlanRequestCode.IsNullOrEmpty() ? " " : item.BDRepaymentPlanRequestCode,
                    FormatBool(item.BDLoansAtOtherServicer, " "),
                    item.BDSpouseId == null ? "N" : "Y",
                    FormatBool(item.BDJointRepaymentPlan, " "),
                    "  ",
                    item.BDTaxYear == null ? "    " : item.BDTaxYear.ToString(),
                    item.BDFilingStatusCode == null ? " " : item.BDFilingStatusCode == 6 ? " " : item.BDFilingStatusCode.ToString(),
                    FormatMoney(item.BDAgi),
                    FormatBool(item.BDAgiReflectsCurrentIncome, " "),
                    " ",
                    " ".PadRight(6),
                    FormatBool(item.BDSupportingDocRequired, "N"),
                    "        ",
                    FormatBool(item.BorrowerSelectedLowestPlan, "N"),
                    FormatBool(item.TaxesFiledFlag, "N"),
                    FormatBool(item.TaxableIncomeIndicator, "N"),
                    FormatMoney(item.BDTaxableIncome),
                    FormatBool(item.BDAppSupportingDocRequired, " "),
                    FormatDate(item.BDAppSupportingDocRecDate),
                    item.CurrentDefForbOpt ?? " ",
                    item.EApplicationId.Replace(" ", "").IsNullOrEmpty() ? " " : item.PublicServiceIndicator == null ? " " : item.PublicServiceIndicator == "1" ? "Y" : "N",  // If paper, leave blank; otherwise Y/N
                    FormatMoney(item.ReducedPaymentForb),
                    item.NumberOfChildren.HasValue ? item.NumberOfChildren.Value.ToString().PadLeft(2, '0') : "00",
                    item.NumberOfDependents.HasValue ? item.NumberOfDependents.Value.ToString().PadLeft(2, '0') : "00",
                    ((item.MaritalStatus ?? 1) == 1 ? "S" : "M"),
                    FormatBool(item.SeperatedFromSpouse, " "),
                    FormatBool(item.AccessToSpouseIncome, " "),
                    FormatBool(item.SpousesTaxesFiled, " "),
                    (item.SpouseTaxYear ?? "").PadLeft(4, ' '),
                    item.SpouseFilingStatusId == null ? " " : item.SpouseFilingStatusId == 6 ? " " : item.SpouseFilingStatusId.ToString(),
                    FormatMoney(item.SpouseAgi),
                    FormatBool(item.SpouseAgiReflectsIncome, " "),
                    FormatBool(item.SpouseSupDocsReq, " "),
                    FormatDate(item.SpouseSupDocRecDate),
                    FormatMoney(item.SpouseIncome),
                    " ".PadRight(98),
                    item.LoanSeq,
                    " ".PadRight(11)).ToUpper());
            }
        }

        /// <summary>
        /// Writes AB records
        /// </summary>
        /// <param name="abData"></param>
        /// <param name="sw"></param>
        private static void WriteABRecords(List<AbRecordData> abData, StreamWriter sw)
        {
            foreach (AbRecordData item in abData.Where(p => !p.ABSpouseSsn.IsNullOrEmpty()))
            {
                sw.WriteLine(string.Format("AB{0}{1}{2}{3:yyyyMMdd}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                    item.AwardId,
                    item.ABPersonRole,
                    item.ABSpouseSsn,
                    item.ABSpouseDob,
                    item.ABSpouseFirstName,
                    item.ABSpouseLastName,
                    item.ABSpouseMiddleName.IsNullOrEmpty() ? " ".PadRight(35) : item.ABSpouseMiddleName,
                    (bool)item.ABPersonsSsnIndicator ? "P" : "R",
                    item.ABDriversLicense.IsNullOrEmpty() ? " ".PadRight(30) : item.ABDriversLicense,
                    item.ABDriversLicenseSt.IsNullOrEmpty() ? "  " : item.ABDriversLicenseSt,
                    " ".PadRight(55),
                    item.LoanSeq,
                    " ".PadRight(11)).ToUpper());
            }
        }

        /// <summary>
        /// Determines requested plans. Passes plan objects to helper methods to identify what should be written in the report file.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sw"></param>
        /// <param name="lowestPlanSelected"></param>
        private void ReportNonProcessedPlans(BeRecordData item, StreamWriter sw)
        {
            RequestedPlans requestedPlans = DataAccess.GetRequestedPlans(item.ApplicationId);

            if (requestedPlans != null)
                requestedPlans.PopulatePlans();

            item.BERepaymentTypeProgram = item.BERepaymentTypeProgram ?? ""; //For rare instances where BE table has null value

            if (requestedPlans.RequestedRepaymentPlans.IsNullOrEmpty())
            {
                ReportDeniedPlansWhenNoneRequested(item, sw, requestedPlans);
                return;
            }

            ReportDeniedRequestedPlans(item, sw, requestedPlans);
        }

        /// <summary>
        /// Cycles through requested plans that weren't processed and sends them to a helper method to write the lines in the report.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sw"></param>
        /// <param name="requestedPlans"></param>
        private void ReportDeniedRequestedPlans(BeRecordData item, StreamWriter sw, RequestedPlans requestedPlans)
        {
            foreach (IdrPlan plan in requestedPlans.PlanStatuses.Where(p => p.ProcessedByAgent == false && p.RequestedByBorrower == true))
            {
                item.BERequestedByBorrower = plan.RequestedByBorrower;
                WriteDeniedLine(sw, item, plan.IdrPlanReportingCode);
            }
        }

        /// <summary>
        /// Reports BE data for applications where the borrower did not request a plan.  
        /// In the event of a borrower actually making no plan request, an agent defaults to LP.
        /// Hence, this method is purely future-proofing in case that processing standard changes. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sw"></param>
        /// <param name="requestedPlans"></param>
        private void ReportDeniedPlansWhenNoneRequested(BeRecordData item, StreamWriter sw, RequestedPlans requestedPlans)
        {
            foreach (IdrPlan plan in requestedPlans.PlanStatuses.Where(p => p.ProcessedByAgent == false))
            {
                item.BERequestedByBorrower = false;
                WriteDeniedLine(sw, item, plan.IdrPlanReportingCode);
            }
        }

        /// <summary>
        /// Writes denied line
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        /// <param name="deniedPlan"></param>
        private void WriteDeniedLine(StreamWriter sw, BeRecordData item, string deniedPlan)
        {
            sw.WriteLine("BE{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                    item.AwardId,
                                    FormatAppId(item.ApplicationId, item.EApplicationId),
                                    deniedPlan,
                                    FormatBool(item.BERequestedByBorrower, "N"),
                                    "D1",
                                    FormatDate(item.BERepaymentPlanTypeStausDate),
                                    item.BEFamilySize.ToString().Length == 1 ? "0" + item.BEFamilySize.ToString() : item.BEFamilySize.ToString(),
                                    FormatMoney(item.TotalIncome),
                                    " ".PadRight(180),
                                    item.LoanSeq,
                                " ".PadRight(11));
        }

        /// <summary>
        /// Displays error message, Process Logs error, and sends an email.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="comment"></param>
        /// <param name="specificEmailComment"></param>
        private static void NotifyUserAndLogCriticalError(Exception ex, string comment, string specificEmailComment = null)
        {
            string emailComment = specificEmailComment ?? "Error reporting applications";
            MessageBox.Show(comment);
            ProcessLogger.AddNotification(Program.LogData.ProcessLogId, comment, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
            EmailHelper.SendMail(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", Program.ScriptId + "@utahsbr.edu", emailComment, comment, string.Empty, string.Empty, EmailHelper.EmailImportance.High, false);
        }

        private string FormatBool(bool? item, string nullValue)
        {
            if (item == null)
                return nullValue;
            else
                return (bool)item ? "Y" : "N";
        }

        private string FormatDate(DateTime? date, string pad = "        ")
        {
            if (date == null)
                return pad;
            else
                return ((DateTime)date).ToString("yyyyMMdd");
        }

        private string FormatAppId(int appid, string eApp)
        {
            if (!eApp.Replace(" ", "").IsNullOrEmpty())
                return RemoveLeadingZeros(eApp).PadRight(10, ' ').ToUpper();

            List<string> alpha = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int index = 0;
            int val = appid;
            int position = 0;
            for (; true; index++)
            {
                val = val - 10000;
                if (val < 9999)
                    break;
                if (index == 26)
                {
                    index = -1;
                    position += 1;
                    continue;
                }
                if (position > 4)
                {
                    Dialog.Error.Ok("This app has used the maximum number of application id combinations");
                    Environment.Exit(1);
                }
            }

            string alphaSeq = alpha[index];
            string appIdData = "P502M" + alphaSeq;

            if (index == 0)
                val = appid;

            if (val == 0)
                val += 1;

            return appIdData + val.ToString().PadLeft(4, '0').ToUpper();
        }

        private string RemoveLeadingZeros(string id)
        {
            return id.TrimLeft("0");
        }

        private string FormatMoney(int? amount)
        {
            if (amount == null)
                return "000000";

            string amountNoCents = amount.ToString().PadLeft(6, '0');

            return amountNoCents;
        }
    }
}