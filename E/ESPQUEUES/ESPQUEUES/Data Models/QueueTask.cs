using System;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace ESPQUEUES
{
    public class QueueTask
    {
        public enum ProcessingMethod
        {
            CreateDefermentReviewTasks,
            ReviewAccount,
            CreateSchoolCodeReviewTasks,
            CreateMilitaryStatusReviewTasks,
            CreateMissingDataReviewTasks,
            ReviewDateDiscrepancy,
            ReviewDefermentEligibility,
            PromptForManualUpdate,
            PlusReview,
            CreateGracePeriodReviewTasks,
            AddToErrors,
            ManualReview,
            FailedToParseTaskInfo
        }
        public int ProcessingQueueId { get; set; }
        public string BorrowerSsn { get; set; }
        public string AccountNumber { get; set; }
        public string Queue { get; set; }
        public string SubQueue { get; set; }
        public string TaskControlNumber { get; set; }
        public string RequestArc { get; set; }
        public DateTime? RequestArcCreatedAt { get; set; }
        public bool HasOtherGuarantor { get; set; }
        public int? ArcAddProcessingId { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public DateTime? ReassignedAt { get; set; }
        public int? ProcessingStepId { get; set; }
        public DateTime LenderNotifiedDate { get; set; }
        public string Message { get; set; }
        public string StudentSsn { get; set; }
        public string SchoolCode { get; set; }
        public string SeparationReason { get; set; }
        public DateTime SeparationDate { get; set; }
        public DateTime CertifiedDate { get; set; }
        public DateTime EnrollmentStatusDate { get; set; }
        public string SeparationSource { get; set; }
        public ProcessingMethod Status { get; set; }

        public override string ToString()
        {
            string format = "{0}, ST SSN {1}, SCHL {2}, SEP REA {3}, SEP DT {4:MM/dd/yyyy}, CERT DT {5:MM/dd/yyyy}, ENRL STAT EFF {6:MM/dd/yyyy}, SEP SOURCE {7}, NOTIF DT {8:MM/dd/yyyy}";
            object[] args = { Message, StudentSsn, SchoolCode, SeparationReason, SeparationDate, CertifiedDate, EnrollmentStatusDate, SeparationSource, LenderNotifiedDate };
            return string.Format(format, args);
        }

        public void Populate(ReflectionInterface ri)
        {
            try
            {
                BorrowerSsn = ri.GetText(8, 6, 9);
                LenderNotifiedDate = DateTime.Parse(ri.GetText(8, 47, 10));
                Message = ri.GetText(9, 2, 78);
                StudentSsn = ri.GetText(10, 2, 9);
                SchoolCode = ri.GetText(10, 20, 8);
                SeparationReason = ri.GetText(10, 28, 2);
                SeparationDate = ri.GetText(10, 34, 2).IsNullOrEmpty() ? new DateTime() : DateTime.Parse((ri.GetText(10, 34, 2) + ri.GetText(10, 36, 2) + ri.GetText(10, 30, 4)).ToDateFormat());
                CertifiedDate = ri.GetText(10, 42, 2).IsNullOrEmpty() ? new DateTime() : DateTime.Parse((ri.GetText(10, 42, 2) + ri.GetText(10, 44, 2) + ri.GetText(10, 38, 4)).ToDateFormat());
                EnrollmentStatusDate = DateTime.Parse((ri.GetText(10, 50, 2) + ri.GetText(10, 52, 2) + ri.GetText(10, 46, 4)).ToDateFormat());
                SeparationSource = ri.GetText(10, 54, 2);
                Status = FindProcess(Message);
            }
            catch (Exception)
            {
                Status = ProcessingMethod.FailedToParseTaskInfo;
            }
        }

        private static ProcessingMethod FindProcess(string message)
        {
            switch (message.ToLower())
            {
                case "unknown deferment type exists, please correct the account":
                case "align repayment forb overlaps forb/defer":
                case "comaker exists - review defer eligibility":
                case "cert date unknown - review for possible defer change":
                case "school forb or defer exists > incoming sep - older cert dt":
                case "incoming date may qualify for bridge deferment":
                case "admin forb to cover dlnqcy-other defer/forb exists for period":
                case "cert dte equal-conflict between incoming enrol and existing dfr":
                case "defer school not equal incoming school/review defer":
                case "schl defr exists with different status than incoming record":
                case "different schools - school deferment exists with greater end date":
                case "other deferment type exists during incoming enrollment - review":
                case "school forb exists (no schl code) - review for possible enrllmnt chng":
                case "repurchased or rehabilitated loan-manual review required":
                case "loan not eligible for incoming status - review defer eligibility":
                case "equal or newer cert exists for incoming schl - review defer dates":
                case "defer change needed - review existing align repay forb":
                case "overlap with another deferment, pdg, or forbearance - update manually":
                case "forb exists with unknown cert dt or a cert greater than incoming record":
                case "defer change/delete - review other existing defer":
                case "no sep date on incoming transmission - review deferment info":
                case "sep date change - defer/forb may need modified":
                case "verify the validity of align repayment forb":
                case "incoming esed/crt dtd are prior to cnsldn disb dtd":
                case "deferment exist - review loan":
                    return ProcessingMethod.CreateDefermentReviewTasks;
                case "incoming school code begins with 8 or blank - no current schl defer":
                case "cert date unknown - review for sep date change":
                case "possible update required-pull clearinghouse for additional info":
                case "possible update required-pull nslds for additional info":
                case "cert date equal - conflict between incoming enrol and existing sep":
                case "unknown cert date - review if cert date should be updated":
                case "school transfer new cert l/t current cert date plus tolerance":
                case "data missing on incoming tape to adjust loan":
                case "different schools - possible sep date change":
                case "incoming sep date is more than 10 years is future":
                case "same cert and sep dtd incoming reason different - please review":
                    return ProcessingMethod.ReviewAccount;
                case "ineligible or invalid school code":
                case "bad data on incoming file - incoming esed/begin is > incoming sep dte":
                    return ProcessingMethod.CreateSchoolCodeReviewTasks;
                case "borrower in military - review incoming data for possible change":
                    return ProcessingMethod.CreateMilitaryStatusReviewTasks;
                case "no incoming esed - review required":
                    return ProcessingMethod.CreateMissingDataReviewTasks;
                case "no repay schedule exists - review same month same year":
                    return ProcessingMethod.ReviewDateDiscrepancy;
                case "student not equal borrower - review deferment eligibility":
                case "other plusrev loans exist for this borrower - review for defer":
                case "other plusrev loans exist for this borrower - review all loans for defer":
                    return ProcessingMethod.PlusReview;
                case "borrower on leave - review for additional deferment eligibility":
                    return ProcessingMethod.ReviewDefermentEligibility;
                case "bad data on incoming file - incoming sep dte is l/t incoming esed/begin":
                    return ProcessingMethod.PromptForManualUpdate;
                case "tl4 loan with a # mos disclosed in grace equal to zero":
                    return ProcessingMethod.CreateGracePeriodReviewTasks;
                case "manual review required - please review":
                    return ProcessingMethod.ManualReview;
                default:
                    return ProcessingMethod.AddToErrors;
            }
        }
    }
}
