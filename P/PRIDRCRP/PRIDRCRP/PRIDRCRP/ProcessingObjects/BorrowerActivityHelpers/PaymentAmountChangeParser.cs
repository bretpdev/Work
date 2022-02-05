using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    public class PaymentAmountChangeParser
    {
        public RepaymentPlanChangesRecord lastPaymentAmountChange { get; set; }
        public bool firstPaymentAmountChangeFound = false;

        public List<Error> GetPaymentAmountChange(int result, List<BorrowerActivityResult> results, DataAccess DA, string lastPlanType, List<DueDatePeriod> dueDatePeriods, int daysForward = 2, int daysBackward = 1)
        {
            List<Error> errorLog = new List<Error>();
            RepaymentPlanChangesRecord record = new RepaymentPlanChangesRecord();
            //results = results.OrderBy(r => r.ActivityDate).ThenBy(r => r.BorrowerActivityId).ToList();
            string flatActivityDescription = results[result].ActivityDescription.Replace(" ", "").Replace("|", "");
            if (flatActivityDescription.Contains("PAYMENTAMOUNTCHANGEDTO"))
            {
                errorLog.AddRange(ChangeParsingHelper.CheckPmtAmt(record, flatActivityDescription, results[result]));
                record.EffectiveDate = results[result].ActivityDate;
                DateTime? firstPayDue = DA.GetBorrowerInformation(results[result].BorrowerInformationId).FirstOrDefault().FirstPayDue;
                if (record.EffectiveDate == null && results[result].ActivityDate <= firstPayDue)
                {
                    record.EffectiveDate = firstPayDue;
                }
                if ((!record.PmtAmt.HasValue || !record.EffectiveDate.HasValue) && results[result].ActivityDate > DA.GetBorrowerInformation(results[result].BorrowerInformationId).FirstOrDefault().FirstPayDue)
                {
                    errorLog.Add(new Error(results[result].BorrowerInformationId, $"Borrower Information Id: {results[result].BorrowerInformationId.ToString()} Borrower Activity Id: {results[result].BorrowerActivityId.ToString()} Unable to get payment amount and corresponding payment effective date for Borrower Activity Id: {results[result].BorrowerActivityId} Activity Descripition: {results[result].ActivityDescription};"));
                }
                else if (record.PmtAmt.HasValue && record.EffectiveDate.HasValue)
                {
                    lastPaymentAmountChange = record;
                    if(!firstPaymentAmountChangeFound && record.EffectiveDate.Value > firstPayDue)
                    {
                        errorLog.Add(new Error(results[result].BorrowerInformationId, $"Borrower Information Id: {results[result].BorrowerInformationId.ToString()} Borrower Activity Id: {results[result].BorrowerActivityId.ToString()} Found first payment amount change dated after first pay due date.;"));
                    }
                    DA.InsertToPaymentAmountChanges(results[result].BorrowerActivityId, record.PmtAmt.Value, record.EffectiveDate);
                    firstPaymentAmountChangeFound = true;
                }
            }
            else if (flatActivityDescription.Contains("PAYMENTAMOUNTCHANGEDFROM") || flatActivityDescription.Contains("PAYMENTSTEMPORARILYCHANGEDTO") || flatActivityDescription.Contains("#PMTSPMTAMTBEGINNING") || flatActivityDescription.Contains("NEWPAYMENTAMOUNTIS"))
            {
                //Check if the record is a payment table and parse the info from it
                ChangeParsingHelper.CheckPaymentTable(record, results[result], flatActivityDescription);
                //Check if there is a record containing payment amount information
                errorLog.AddRange(ChangeParsingHelper.CheckPmtAmt(record, flatActivityDescription, results[result]));
                errorLog.AddRange(ChangeParsingHelper.CheckFirstPayDue(record, flatActivityDescription, results[result], true));

                //read backward
                int j = result - 1;
                DateTime current = results[result].ActivityDate;
                while (j > -1 && record.EffectiveDate == null)
                {
                    flatActivityDescription = results[j].ActivityDescription.Replace(" ", "").Replace("|", "");
                    if ((current - results[j].ActivityDate).TotalDays > daysBackward || (record.EffectiveDate.HasValue && record.PmtAmt.HasValue))
                    {
                        break;
                    }
                    //Check if the record is a payment table and parse the info from it
                    ChangeParsingHelper.CheckPaymentTable(record, results[j], flatActivityDescription);
                    //Check if there is a record containing payment amount information
                    errorLog.AddRange(ChangeParsingHelper.CheckPmtAmt(record, flatActivityDescription, results[j]));
                    errorLog.AddRange(ChangeParsingHelper.CheckFirstPayDue(record, flatActivityDescription, results[j], true));
                    j--;
                }
                j = result + 1;
                while (j < results.Count && record.EffectiveDate == null)
                {
                    flatActivityDescription = results[j].ActivityDescription.Replace(" ", "").Replace("|", "");
                    if ((results[j].ActivityDate - current).TotalDays > daysForward || (record.EffectiveDate.HasValue && record.PmtAmt.HasValue))
                    {
                        break;
                    }
                    //Check if the record is a payment table and parse the info from it
                    ChangeParsingHelper.CheckPaymentTable(record, results[j], flatActivityDescription);
                    //Check if there is a record containing payment amount information
                    errorLog.AddRange(ChangeParsingHelper.CheckPmtAmt(record, flatActivityDescription, results[j]));
                    errorLog.AddRange(ChangeParsingHelper.CheckFirstPayDue(record, flatActivityDescription, results[j], true));
                    j++;
                }
                //record.EffectiveDate = record.EffectiveDate.HasValue ? record.EffectiveDate.Value : results[result].ActivityDate;
                DateTime? firstPayDue = DA.GetBorrowerInformation(results[result].BorrowerInformationId).FirstOrDefault().FirstPayDue;
                if (!record.EffectiveDate.HasValue)
                {
                    //DateTime? firstPayDue = DA.GetBorrowerInformation(results[result].BorrowerInformationId).FirstOrDefault().FirstPayDue;
                    DateTime nextDueDate;
                    if (dueDatePeriods.Count > 0)
                    {
                        var periods = dueDatePeriods.Where(r => r.BeginDate < results[result].ActivityDate).OrderBy(r => r.BeginDate);
                        if (periods.Count() > 0)
                        {
                            var period = periods.Last();
                            nextDueDate = new DateTime(results[result].ActivityDate.Year, results[result].ActivityDate.Month, period.Day.Value);
                        }
                        else
                        {
                            nextDueDate = new DateTime(results[result].ActivityDate.Year, results[result].ActivityDate.Month, dueDatePeriods.Last().Day.Value);
                        }
                    }
                    else
                    {
                        nextDueDate = new DateTime(results[result].ActivityDate.Year, results[result].ActivityDate.Month, firstPayDue.Value.Day);
                    }

                    //If the due date for the month is before the activity date, start with the next month
                    if (nextDueDate.Date < results[result].ActivityDate.Date)
                    {
                        nextDueDate.AddMonths(1);
                    }

                    DateTime activityDatePlus21Days = results[result].ActivityDate.AddDays(21);
                    if (nextDueDate.Date < activityDatePlus21Days.Date)
                    {
                        nextDueDate = nextDueDate.AddMonths(1);
                    }

                    record.EffectiveDate = nextDueDate;
                }
                if (record.PmtAmt.HasValue && record.EffectiveDate.HasValue)
                {
                    lastPaymentAmountChange = record;
                    if (!firstPaymentAmountChangeFound && record.EffectiveDate.Value > firstPayDue)
                    {
                        errorLog.Add(new Error(results[result].BorrowerInformationId, $"Borrower Information Id: {results[result].BorrowerInformationId.ToString()} Borrower Activity Id: {results[result].BorrowerActivityId.ToString()} Found first payment amount change dated after first pay due date.;"));
                    }
                    DA.InsertToPaymentAmountChanges(results[result].BorrowerActivityId, record.PmtAmt.Value, record.EffectiveDate);
                    firstPaymentAmountChangeFound = true;
                }
            }

            string lastPlanTypeString = lastPlanType == null ? "" : lastPlanType;
            if (DA.GetDateInDefFor(results[result].BorrowerInformationId, record.EffectiveDate.HasValue ? record.EffectiveDate.Value : results[result].ActivityDate) ||
                lastPlanTypeString.Replace(" ", "") == "FORCEDICR" ||
                lastPlanTypeString.Replace(" ", "") == "INCOMEBASED" || 
                lastPlanTypeString.Replace(" ", "") == "INCOMECONTINGENT" || 
                lastPlanTypeString.Replace(" ", "") == "STANDARD")
            {
                return new List<Error>();
            }
            else
            {
                return errorLog;
            }
        }
    }
}
