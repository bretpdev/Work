using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public sealed class BorrowerActivityInformation : FileInformation
    {
        public override FileParser.FileSection Section { get; protected set; } = FileParser.FileSection.Section_IV;
        public override List<Error> ExceptionLog { get; protected set; } = new List<Error>();

        private List<string> planTypes { get; set; }
        private ProcessLogRun logRun { get; set; }
        private DataAccess DA { get; set; }

        private readonly List<string> ELPlans = new List<string>() { "CONSOLSTANDARD", "EXTENDEDFIXED", "EXTENDED-GRANDFATHERED", "CONSOL-STANDARD"};

        public List<BorrowerActivityRecord> borrowerActivityRecords { get; private set; } = new List<BorrowerActivityRecord>();

        //public List<BorrowerActivityResult> borrrowerActivityResults { get; private set; }
        private bool foundHeaderRow = false;
        private bool onPageBreak = false;
        private bool foundFirstPageBreak = false;
        private int rowsToSkipAfterFirstHeader = 2;
        private int rowsToSkipAfterPageBreak = 9;
        private int rowPageBreak = 0;
        private int rowHeader = 0;

        public BorrowerActivityInformation(ProcessLogRun logRun)
        {
            this.logRun = logRun;          
        }

        public BorrowerActivityInformation(ProcessLogRun logRun, DataAccess DA)
        {
            this.logRun = logRun;
            this.DA = DA;
            planTypes = DA.GetRepaymentPlanTypes().Select(r => r.RepaymentPlanType).ToList();
        }

        public override bool FoundInformation()
        {
            return foundHeaderRow;
        }

        public override void GetInformation(string line)
        {
            CheckParsingLocation(line);

            if(rowHeader > rowsToSkipAfterFirstHeader)
            {
                if(foundFirstPageBreak && rowPageBreak > rowsToSkipAfterPageBreak)
                {
                    HandleCommentInformation(line);
                }
                else if(!foundFirstPageBreak)
                {
                    HandleCommentInformation(line);
                }
            }
        }

        private void HandleCommentInformation(string line)
        {
            if (line.Replace("-", "").Replace("0", "").Trim() != "")
            {
                //If the activity date is populated, create a new record
                if (StringParsingHelper.SafeSubStringTrimmed(line,8, 8) != "")
                {
                    BorrowerActivityRecord borrowerActivityRecord = new BorrowerActivityRecord();
                    borrowerActivityRecord.ActivityDate = Convert.ToDateTime(StringParsingHelper.SafeSubStringTrimmed(line,8, 8));
                    borrowerActivityRecord.ActivityDescription = StringParsingHelper.SafeSubStringTrimmed(line,19, 112);
                    borrowerActivityRecords.Add(borrowerActivityRecord);
                }
                //otherwise accumulate the comment to the last record
                else if (borrowerActivityRecords.Count > 0)
                {
                    borrowerActivityRecords.LastOrDefault().ActivityDescription += "|" + StringParsingHelper.SafeSubStringTrimmed(line,19, 112);
                }
            }
        }

        private void CheckParsingLocation(string line)
        {
            if(foundFirstPageBreak)
            {
                rowPageBreak++;
            }
            if(foundHeaderRow)
            {
                rowHeader++;
            }

            if(onPageBreak)
            {
                onPageBreak = false;
            }

            if(line.Replace(" ", "").Contains("BORROWERHISTORYANDACTIVITYREPORT"))
            {
                if(foundHeaderRow)
                {
                    rowPageBreak = 0;
                    onPageBreak = true;
                    foundFirstPageBreak = true;
                }
            }

            if(!foundHeaderRow && line.Replace(" ", "").Contains("ACTIVITY-------------------ACTIVITYDESCRIPTION-------------------NOTICE0"))
            {
                foundHeaderRow = true;
            }
        }

        public override bool ValidateInformation(string file, DataAccess DA)
        {
            foreach(BorrowerActivityRecord record in borrowerActivityRecords)
            {
                if(record == null || !record.ActivityDate.HasValue || record.ActivityDescription == null)
                {
                    DA.ThrowDataValidationError("Bad Borrower Activity Records.File: " + file);
                }
            }
            return true;
        }

        public override bool WriteToDatabase(DataAccess DA)
        {
            List<BorrowerActivityResult> result = DA.InsertToBwrAtyHst(borrowerActivityRecords);
            //In writing to the database we need to parse the records into the repayment plan history table
            bool success = AggregateRepaymentPlanChanges(result, DA);
            return result != null && success;
        }

        public bool WriteRepaymentPlanChangesToDatabase(BorrowerActivityResult result, RepaymentPlanChangesRecord record, DataAccess DA, List<DueDatePeriod> dueDatePeriods)
        {
            if (result != null)
            {
                if (!record.EffectiveDate.HasValue)
                {
                    DateTime? firstPayDue = DA.GetBorrowerInformation(result.BorrowerInformationId).FirstOrDefault().FirstPayDue;
                    DateTime nextDueDate;
                    if (dueDatePeriods.Count > 0)
                    {
                        var periods = dueDatePeriods.Where(r => r.BeginDate < result.ActivityDate).OrderBy(r => r.BeginDate);
                        if(periods.Count() > 0)
                        {
                            var period = periods.Last();
                            nextDueDate = new DateTime(result.ActivityDate.Year, result.ActivityDate.Month, period.Day.Value);
                        }
                        else
                        {
                            nextDueDate = new DateTime(result.ActivityDate.Year, result.ActivityDate.Month, dueDatePeriods.Last().Day.Value);
                        }
                    }
                    else
                    {
                        nextDueDate = new DateTime(result.ActivityDate.Year, result.ActivityDate.Month, firstPayDue.Value.Day);
                    }

                    //If the due date for the month is before the activity date, start with the next month
                    if (nextDueDate.Date < result.ActivityDate.Date)
                    {
                        nextDueDate.AddMonths(1);
                    }

                    DateTime activityDatePlus21Days = result.ActivityDate.AddDays(21);
                    if (nextDueDate.Date < activityDatePlus21Days.Date)
                    {
                        nextDueDate = nextDueDate.AddMonths(1);
                    }

                    record.EffectiveDate = nextDueDate;
                }
            }
            bool success = DA.InsertToRepaymentPlanChanges(result, record);
            return success;
        }

        public RepaymentPlanChangesRecord CheckOutstandingPrincipal(RepaymentPlanChangesRecord record, string flatActivityDescription, BorrowerActivityResult result)
        {
            if (flatActivityDescription.Contains("DISCLOSUREREFLECTEDPRINCIPALBALANCEOF"))
            {
                string outstandingPrin = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "DISCLOSUREREFLECTEDPRINCIPALBALANCEOF", new char[] { '.', ' ', ',' });
                if (outstandingPrin != "" && !record.OutstandingPrin.HasValue)
                {
                    record.OutstandingPrin = Convert.ToDecimal(outstandingPrin);
                }
                else if (outstandingPrin != "" && record.OutstandingPrin.HasValue)
                {
                    ExceptionLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Outstanding principal already exists for repayment plan change. Existing prinipal: {record.OutstandingPrin.Value.ToString()} Additional principal: {outstandingPrin};".Trim()));
                }
            }
            return record;
        }

        public RepaymentPlanChangesRecord CheckInterestRate(RepaymentPlanChangesRecord record, string flatActivityDescription, BorrowerActivityResult result)
        {
            if(flatActivityDescription.Contains("INTERESTRATEINEFFECT"))
            {
                string interestRate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "INTERESTRATEINEFFECT,", new char[] { ',', '.', ' ' });
                if (interestRate != "" && !record.InterestRate.HasValue)
                {
                    record.InterestRate = Convert.ToDecimal(interestRate);
                }
                else if (interestRate != "" && record.InterestRate.HasValue)
                {
                    ExceptionLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Interest rate already exists for repayment plan change. Existing rate: {record.InterestRate.Value.ToString()} Additional rate: {interestRate} ;".Trim()));
                }
            }
            else if(flatActivityDescription.Contains("INTERESTRATE"))
            {
                string interestRate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "INTERESTRATE", new char[] { ',', '.', ' ' });
                if (interestRate != "" && !record.InterestRate.HasValue)
                {
                    record.InterestRate = Convert.ToDecimal(interestRate);
                }
                else if (interestRate != "" && record.InterestRate.HasValue)
                {
                    ExceptionLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Interest rate already exists for repayment plan change. Existing rate: { record.InterestRate.Value.ToString()} Additional rate: interestRate;".Trim()));
                }
            }
            return record;
        }

        public decimal? CheckZeroBalanceDelinquencyException(decimal? paymentAmount, BorrowerActivityResult result, string flatActivityDescription)
        {
            //Try to read a payment amount from the current record
            RepaymentPlanChangesRecord record = new RepaymentPlanChangesRecord();
            ExceptionLog.AddRange(ChangeParsingHelper.CheckPmtAmt(record, flatActivityDescription, result));
            ChangeParsingHelper.CheckPaymentTable(record, result, flatActivityDescription);

            if(record.PmtAmt.HasValue)
            {
                return record.PmtAmt;
            }

            if (paymentAmount.HasValue && paymentAmount.Value == 0.00M)
            {
                for(int i = 1; i < 9; i++)
                {
                    if(flatActivityDescription.Contains("PDP" + i.ToString()))
                    {
                        ExceptionLog.Add(new Error(result.BorrowerInformationId, $"First Repayment Plan Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Borrower received delinquency notice PDP {i.ToString()} while having $0.00 payment amount;"));
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Payment amount should be the resulting payment amount from the previous iteration with a value if one exists
        /// </summary>
        public void CheckPaymentAmountGapException(PaymentAmountInconsistencyChecker checker, string flatActivityDescription, string activityDescription, BorrowerActivityResult result, string lastPlanType)
        {
            List<Error> errorLog = checker.CheckForPaymentAmount(flatActivityDescription, activityDescription, result, lastPlanType);

            ExceptionLog.AddRange(errorLog);
        }

        public void GetInterestRateChange(DataAccess DA, BorrowerActivityResult result, string flatActivityDescription)
        {
            RepaymentPlanChangesRecord record = new RepaymentPlanChangesRecord();
            if (flatActivityDescription.Contains("INTERESTRATEINEFFECT"))
            {
                string interestRate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "INTERESTRATEINEFFECT,", new char[] { ',', '.', ' ' });
                if (interestRate != "")
                {
                    record.InterestRate = Convert.ToDecimal(interestRate);
                    //Get the effective date
                    string effectiveDate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "EFFECTIVE", new char[] { '/', ' ' });
                    DateTime? val = effectiveDate.ToDateNullable();
                    if(val.HasValue)
                    {
                        record.EffectiveDate = val;
                    }
                }
            }
            else if (flatActivityDescription.Contains("INTERESTRATE"))
            {
                string interestRate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "INTERESTRATE", new char[] { ',', '.', ' ' });
                if (interestRate != "")
                {
                    record.InterestRate = Convert.ToDecimal(interestRate);
                    //Get the effective date
                    string effectiveDate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "INTERESTBEGINSTOACCRUE", new char[] { '/', ' ' });
                    DateTime? val = effectiveDate.ToDateNullable();
                    if (val.HasValue)
                    {
                        record.EffectiveDate = val;
                    }
                    else
                    {
                        ChangeParsingHelper.CheckPaymentTable(record, result, flatActivityDescription);
                    }
                }
            }
            if(record.InterestRate.HasValue)
            {
                DA.InsertToInterestRateChanges(result.BorrowerActivityId, record.InterestRate.Value, record.EffectiveDate);
            }
        }

        public void GetOutstandingPrincipalChange(DataAccess DA, BorrowerActivityResult result, string flatActivityDescription)
        {
            RepaymentPlanChangesRecord record = new RepaymentPlanChangesRecord();
            if (flatActivityDescription.Contains("REFLECTEDPRINCIPALBALANCEOF"))
            {
                string outstandingPrin = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "REFLECTEDPRINCIPALBALANCEOF", new char[] { '.', ' ', ',' });
                if (outstandingPrin != "" && !record.OutstandingPrin.HasValue)
                {
                    record.OutstandingPrin = Convert.ToDecimal(outstandingPrin);
                    record.EffectiveDate = result.ActivityDate;
                }
            }
            if(record.OutstandingPrin.HasValue)
            {
                DA.InsertToOutstandingPrincipalChanges(result.BorrowerActivityId, record.OutstandingPrin.Value, record.EffectiveDate);
            }
        }

        private void GetEffectiveDateForRepaymentPlan(RepaymentPlanChangesRecord record, BorrowerActivityResult result)
        {
            string flatActivityDescription = StringParsingHelper.GetFlatString(result.ActivityDescription);
            if(flatActivityDescription.Contains("REPAYMENTPLANCHANGEFROM"))
            {
                string effectiveDate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "EFFECTIVEDATEWAS", new char[] { '/' });
                record.EffectiveDate = effectiveDate.ToDateNullable();
            }
        }

        private bool AggregateRepaymentPlanChanges(List<BorrowerActivityResult> results, DataAccess DA)
        {
            //Initialize excpetion helpers
            PaymentAmountInconsistencyChecker checker = new PaymentAmountInconsistencyChecker(DA);

            //Parsers
            DueDateParser dueDateParser = new DueDateParser(DA);
            PaymentAmountChangeParser paymentAmountChangeParser = new PaymentAmountChangeParser();

            //Last populated records for various values
            DateTime? pmtAmtPeriodStart = null;
            //DateTime? achEffectiveDate = null;
            decimal? previousLastPmtAmt = null;
            decimal? lastPmtAmt = null;
            //DateTime? lastBillEffectiveDate = null;
            string lastPlanType = null;
            bool foundFirstRepaymentPlanChange = false;
            DateTime? lastActivityDate = null;
            DateTime? previousLastActivityDate = null;
            DueDatePeriod partialDueDate = new DueDatePeriod();
            List<PaymentAmountPeriod> paymentAmountPeriods = new List<PaymentAmountPeriod>();
            List<DueDatePeriod> dueDatePeriods = new List<DueDatePeriod>();

            //Days to look forward and back when finding a repayment plan change
            int readForward = 21;
            int readBackward = 1;
            results = results.OrderBy(r => r.ActivityDate).ThenBy(r => r.BorrowerActivityId).ToList();
            for(int i = 0; i < results.Count; i++)
            {
                lastActivityDate = results[i].ActivityDate;
                if(!previousLastActivityDate.HasValue)
                {
                    previousLastActivityDate = lastActivityDate;
                }

                if (results[i].ActivityDescription.Replace(" ", "").Replace("|", "").Contains("REPAYMENTPLANCHANGEDFROM") || results[i].ActivityDescription.Replace(" ", "").Replace("|", "").Contains("REPAYMENTPLANCHANGEFROM"))
                {
                    RepaymentPlanChangesRecord record = new RepaymentPlanChangesRecord();
                    List<string> planType = GetRepaymentPlanType(results[i].ActivityDescription, results[i]);
                    GetEffectiveDateForRepaymentPlan(record, results[i]);
                    if (planType == null)
                    {
                        continue;
                    }

                    record.PlanType = planType[1];
                    
                    //If we find an EL plan we need to add an error, because we want to manually review EL for the time being
                    if(ELPlans.Contains(record.PlanType.Replace(" ", "")))
                    {
                        ExceptionLog.Add(new Error(results[i].BorrowerInformationId, $"Borrower Information Id: {results[i].BorrowerInformationId.ToString()} Borrower Activity Id: {results[i].BorrowerActivityId.ToString()}, {record.PlanType} found on account. EL Plan needs manual review;".Trim()));
                    }

                    //add the first repayment plan
                    if (!foundFirstRepaymentPlanChange)
                    {
                        BorrowerRecord borrowerInformation = DA.GetBorrowerInformation().SingleOrDefault();
                        if(borrowerInformation != null)
                        {                          
                            RepaymentPlanChangesRecord startingRecord = new RepaymentPlanChangesRecord();
                            startingRecord.PlanType = planType[0];
                            
                            //If we find an EL plan we need to add an error, because we want to manually review EL for the time being
                            if (ELPlans.Contains(startingRecord.PlanType.Replace(" ", "")))
                            {
                                ExceptionLog.Add(new Error(results[i].BorrowerInformationId, $"Borrower Information Id: {results[i].BorrowerInformationId.ToString()} Borrower Activity Id: {results[i].BorrowerActivityId.ToString()}, {startingRecord.PlanType} found on account. EL Plan needs manual review;".Trim()));
                            }

                            startingRecord.EffectiveDate = borrowerInformation.FirstPayDue;
                            WriteRepaymentPlanChangesToDatabase(results[i], startingRecord, DA, dueDatePeriods);
                            foundFirstRepaymentPlanChange = true;
                        }
                    }

                    //Check that the last plan type matches the starting plan type
                    if(lastPlanType != null)
                    {
                        List<string> ICRPlans = new List<string>() { "INCOME CONTINGENT REPAYMENT 1", "INCOME CONTINGENT REPAYMENT 2", "INCOME CONTINGENT REPAYMENT 3", "INCOME CONTINGENT" };
                        if(lastPlanType != planType[0] && !(planType[0] == "FORCED ICR" && ICRPlans.Contains(lastPlanType)) && results[i].ActivityDate > DA.GetBorrowerInformation(results[i].BorrowerInformationId).FirstOrDefault().FirstPayDue)
                        {
                            ExceptionLog.Add(new Error(results[i].BorrowerInformationId, $"Borrower Information Id: {results[i].BorrowerInformationId.ToString()} Borrower Activity Id: {results[i].BorrowerActivityId.ToString()} Last Plan Type does not match the starting plan type of the new change last plan: {lastPlanType} starting plan type: {planType[0]};".Trim()));
                        }
                    }
                    lastPlanType = planType[1];
                    
                    //read backward
                    int j = i - 1;
                    DateTime current = results[i].ActivityDate;
                    while (j > -1 && record.EffectiveDate == null)
                    {
                        string flatActivityDescription = results[j].ActivityDescription.Replace(" ", "").Replace("|", "");
                        if ((current - results[j].ActivityDate).TotalDays > readBackward || flatActivityDescription.Contains("REPAYMENTPLANCHANGEDFROM"))
                        {
                            break;
                        }
                        //Check for the repayment plan change effective date
                        ChangeParsingHelper.CheckPaymentTable(record, results[j], flatActivityDescription);
                        ExceptionLog.AddRange(ChangeParsingHelper.CheckFirstPayDue(record, flatActivityDescription, results[j]));

                        j--;
                    }
                    j = i + 1;
                    while(j < results.Count && record.EffectiveDate == null)
                    {
                        string flatActivityDescription = results[j].ActivityDescription.Replace(" ", "").Replace("|", "");
                        if ((results[j].ActivityDate - current).TotalDays > readForward || results[j].ActivityDescription.Replace(" ", "").Replace("|", "").Contains("REPAYMENTPLANCHANGEDFROM"))
                        {
                            break;
                        }
                        //Check for the repayment plan change effective date
                        ChangeParsingHelper.CheckPaymentTable(record, results[j], flatActivityDescription);
                        ExceptionLog.AddRange(ChangeParsingHelper.CheckFirstPayDue(record, flatActivityDescription, results[j]));

                        j++;
                    }
                    WriteRepaymentPlanChangesToDatabase(results[i], record, DA, dueDatePeriods);
                }
                //Check for exceptions here
                string activityDesc = results[i].ActivityDescription.Replace(" ", "").Replace("|", "");
                decimal? newLastPayAmt = CheckZeroBalanceDelinquencyException(lastPmtAmt, results[i], activityDesc);
                if(newLastPayAmt.HasValue)
                {
                    lastPmtAmt = newLastPayAmt;
                }
                CheckPaymentAmountGapException(checker, activityDesc, results[i].ActivityDescription, results[i], lastPlanType);
                CheckICRException(lastPlanType, results[i]);
                //Handle Linear history of Interest Rate, Payment Amount, and Oustanding Principal
                GetInterestRateChange(DA, results[i], activityDesc);
                ExceptionLog.AddRange(paymentAmountChangeParser.GetPaymentAmountChange(i, results, DA, lastPlanType, dueDatePeriods, 21, 1));
                GetOutstandingPrincipalChange(DA, results[i], activityDesc);

                if (previousLastPmtAmt != lastPmtAmt)
                {
                    if(pmtAmtPeriodStart.HasValue && i > 0)
                    {
                        DateTime endDate = pmtAmtPeriodStart == paymentAmountChangeParser.lastPaymentAmountChange.EffectiveDate.Value ? pmtAmtPeriodStart.Value : paymentAmountChangeParser.lastPaymentAmountChange.EffectiveDate.Value.AddDays(-1);
                        paymentAmountPeriods.Add(new PaymentAmountPeriod() { BeginDate = pmtAmtPeriodStart, EndDate = endDate, PaymentAmount = previousLastPmtAmt });
                    }
                    pmtAmtPeriodStart = paymentAmountChangeParser.lastPaymentAmountChange.EffectiveDate;
                }

                DueDatePeriod newPeriod = dueDateParser.GetDueDateChange(results[i], partialDueDate);
                if (partialDueDate.BeginDate.HasValue && partialDueDate.Day.HasValue)
                {
                    //Due date change is in effect
                    if (dueDatePeriods.Count > 0)
                    {
                        dueDateParser.GetPeriod(partialDueDate, dueDatePeriods.Last());
                        dueDatePeriods.Add(partialDueDate);
                        partialDueDate = newPeriod == null ? new DueDatePeriod() : newPeriod;
                    }
                    //Use last bill effective date
                    else
                    {
                        dueDatePeriods.Add(partialDueDate);
                        partialDueDate = newPeriod == null ? new DueDatePeriod() : newPeriod;
                    }
                }

                if (i == results.Count - 1 && !lastPmtAmt.HasValue)
                {
                    lastPmtAmt = DA.GetBorrowerInformation(results[i].BorrowerInformationId).FirstOrDefault()?.PaymentAmount;
                }

                if(i == results.Count - 1)
                {
                    //get loan add date for results[i].FirstRepaymentPlanId and use that as end date instead of activityDate
                    DateTime? loanAdd = DA.GetLoanAddDate(results[i].BorrowerInformationId);
                    if (!loanAdd.HasValue || loanAdd.Value < results[i].ActivityDate)
                    {
                        string loanAddDateString = loanAdd.HasValue ? loanAdd.ToString() : "NONE";
                        LogException($"Loan add date is before the end of the activity history or is not available. Loan Add Date: { loanAddDateString }", results[i]);
                        loanAdd = results[i].ActivityDate.AddDays(1);
                    }

                    //If there is no plan type insert a record saying the plan type is the same as the first repay plan
                    if (!foundFirstRepaymentPlanChange)
                    {
                        RepaymentPlanChangesRecord record = new RepaymentPlanChangesRecord();

                        BorrowerRecord firstRepay = DA.GetBorrowerInformation().FirstOrDefault();
                        record.EffectiveDate = firstRepay.FirstPayDue;
                        record.PlanType = firstRepay.RepayPlan;
                        WriteRepaymentPlanChangesToDatabase(null, record, DA, dueDatePeriods);
                    }

                    //Add end date to last due date period
                    if(dueDatePeriods.Count > 0)
                    {
                        dueDatePeriods.Last().EndDate = dueDatePeriods.Last().EndDate == null ? DateTime.MaxValue : dueDatePeriods.Last().EndDate;
                    }

                    //Chec
                    DA.AddMonthsInRepayment(results[i].BorrowerInformationId, loanAdd.Value, dueDatePeriods);
                }

                previousLastActivityDate = lastActivityDate;
                previousLastPmtAmt = lastPmtAmt;

                //add an exception log if there is not payment amount change
                if (i == results.Count - 1 && !paymentAmountChangeParser.firstPaymentAmountChangeFound)
                {
                    LogException("No payment amount change for firstrepayment plan", results[i]);
                }
            }
            return true;
        }

        public void CheckICRException(string currentPlan, BorrowerActivityResult result)
        {
            string flatActivityDescription = GetFlattenedString(result.ActivityDescription);
            string curPlan = currentPlan;

            BorrowerRecord borrowerInfo = null;

            if (curPlan == null || curPlan == "")
            {
                borrowerInfo = DA.GetBorrowerInformation(result.BorrowerInformationId).FirstOrDefault();
                curPlan = borrowerInfo.RepayPlan;
            }

            if (curPlan != "INCOME CONTINGENT")
            {
                if(flatActivityDescription.Contains("NOTICETYPEBILFNEG-AMBILLSENTTOBORROWERBYPAPER") || flatActivityDescription.Contains("ICRDOCUMENTSRECEIVED") || flatActivityDescription.Contains("NOTICETYPEICR2NOTICEOFINTERESTONLYPAYMENTSENTTOICRBORROWER"))
                {
                    if(borrowerInfo == null)
                    {
                        borrowerInfo = DA.GetBorrowerInformation(result.BorrowerInformationId).FirstOrDefault();
                    }
                    //DateTime? firstPayDue = DA.GetBorrowerInformation(result.BorrowerInformationId).FirstOrDefault().FirstPayDue;
                    if (result.ActivityDate >= borrowerInfo.FirstPayDue)
                    {
                        LogException("Indication of ICR plan without corresponding ICR plan found", result);
                    }
                }
            }
        }

        public string GetFlattenedString(string str)
        {
            return str.Replace(" ", "").Replace("|", "");
        }

        public void LogException(string exception, BorrowerActivityResult result)
        {
            ExceptionLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} {exception};".Trim()));
        }

        /// <summary>
        /// first record is the starting plan
        /// second record is the new plan
        /// </summary>
        private List<string> GetRepaymentPlanType(string activityDescription, BorrowerActivityResult result)
        {
            List<string> parsedPlans = new List<string>();
            int splitIndex = activityDescription.IndexOf("TO");
            string firstHalf = activityDescription.Substring(0, splitIndex - 1);
            string secondHalf = activityDescription.Substring(splitIndex, activityDescription.Length - splitIndex);
            string maxLengthPlan = "";
            foreach (string plan in planTypes)
            {
                if(firstHalf.Contains(plan) && plan.Length > maxLengthPlan.Length)
                {
                    maxLengthPlan = plan;
                }
            }

            //Add the longest plan found in the first half(some plans contain substrings of others)
            if(maxLengthPlan != "")
            {
                parsedPlans.Add(maxLengthPlan);
                maxLengthPlan = "";
            }

            foreach (string plan in planTypes)
            {
                if (secondHalf.Contains(plan) && plan.Length > maxLengthPlan.Length)
                {
                    maxLengthPlan = plan;
                }
            }

            //Add the longest plan found in the first half(some plans contain substrings of others)
            if (maxLengthPlan != "")
            {
                parsedPlans.Add(maxLengthPlan);
                maxLengthPlan = "";
            }

            BorrowerRecord firstRepay = DA.GetBorrowerInformation(result.BorrowerInformationId).FirstOrDefault();
            if (parsedPlans.Count != 2 && result.ActivityDate >= firstRepay.FirstPayDue)
            {
                logRun.AddNotification("Unable To Determine Plan Types from repayment plan change activity: " + activityDescription, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                ExceptionLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Unable To Determine Plan Types from repayment plan change activity: {activityDescription};".Trim()));
                return null;
            }
            else if(parsedPlans.Count != 2)
            {
                return new List<string>() { firstRepay.RepayPlan, firstRepay.RepayPlan };
            }

            return parsedPlans;
        }
    }
}
