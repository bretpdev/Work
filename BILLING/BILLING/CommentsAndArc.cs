using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace BILLING
{
    class CommentsAndArc
    {
        public bool IsEndorser { get; set; }
        public bool EndorserSameAddress { get; set; }
        public string EndorserSsn { get; set; }
        public int? DaysDelinquent { get; set; }
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }

        public CommentsAndArc(Borrower bor, List<string> fields, ProcessLogRun plr, DataAccess da)
        {
            DA = da;
            PLR = plr;
            IsEndorser = fields[395] == "1" ? true : false;
            EndorserSameAddress = fields[396] == "1" ? true : false;
            EndorserSsn = fields[394];
            DaysDelinquent = fields[385].ToIntNullable();
        }

        /// <summary>
        /// Gets the comment for the borrower depending on the condition of their loan
        /// </summary>
        /// <param name="bor">The borrower being processed</param>
        public string GetComment(Borrower bor, bool endorser, bool sameAddress)
        {
            string comment = "";
            if (bor.ReportNumber == 2) comment = "Installment Bill Sent to Borrower";
            else if (bor.ReportNumber == 4) comment = "Interest Statement Sent to Borrower";
            else if (bor.ReportNumber == 5) comment = "Auto-pay Notice Bill Sent to Borrower";
            else if (bor.ReportNumber == 10) comment = "TILP Installment Bill Sent to Borrower";
            else if (bor.ReportNumber == 11) comment = "TILP Auto-pay Notice Bill Sent to Borrower";
            else if (bor.ReportNumber.IsIn(12, 13, 15, 16, 17, 18, 19, 20, 23, 24))
            {
                comment = GetDaysDelinquent(bor) + " Days Delinquent Bill Sent to Borrower";
                if (bor.ReportNumber.IsIn(23, 24))
                    comment += " on autopay";
            }
            else if (bor.ReportNumber == 21) comment = "Reduced Payment Forbearance Letter Sent to Borrower";
            else if (bor.ReportNumber == 22) comment = "Installment bill sent to autopay Borrower";
            else if (bor.ReportNumber == 25) comment = "Complete Installment Bill Sent to Borrower";
            else if (bor.ReportNumber == 26) comment = "Complete Reduced Payment Forbearance Bill Sent to Borrower";
            else comment = string.Empty;

            if (!endorser && sameAddress)
                comment += ". Endorser at same address.";

            return comment;
        }

        /// <summary>
        /// Gets the number of days delinquent for the file
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        /// <returns>The number of days delinquent in a string format</returns>
        private string GetDaysDelinquent(Borrower bor)
        {
            if (bor.ReportNumber.IsIn(12, 23))
                return "20";
            else if (bor.ReportNumber.IsIn(13, 24))
                return "40";
            else if (bor.ReportNumber == 15)
                return "80";
            else if (bor.ReportNumber == 16)
                return "110";
            else if (bor.ReportNumber == 17)
                return "140";
            else if (bor.ReportNumber == 18)
                return "170";
            else if (bor.ReportNumber == 19)
                return "200";
            else if (bor.ReportNumber == 20)
                return "230";
            return "";
        }

        /// <summary>
        /// Sets the ARC according to the status of the borrower account
        /// </summary>
        /// <param name="bor">Borrower object</param>
        /// <param name="fields">Line Data split into a list of strings</param>
        public string GetArc(Borrower bor, bool endorser, bool sameAddress)
        {
            if (bor.ReportNumber.IsIn(12, 13, 15, 16, 17, 18, 19, 20, 23, 24))
            {
                if (endorser || sameAddress)
                    return GetEndorserDelinquentArc(bor);
                else
                {
                    if (bor.ReportNumber == 12) return "DL200";
                    else if (bor.ReportNumber == 13) return "DL400";
                    else if (bor.ReportNumber == 15) return "DL800";
                    else if (bor.ReportNumber == 16) return "DL910";
                    else if (bor.ReportNumber == 17) return "DL140";
                    else if (bor.ReportNumber == 18) return "DL170";
                    else if (bor.ReportNumber == 19) return "DL202";
                    else if (bor.ReportNumber == 20) return "DL230";
                    else if (bor.ReportNumber == 23) return "DL200";
                    else if (bor.ReportNumber == 24) return "DL400";
                    else return string.Empty;
                }
            }
            if (bor.ReportNumber.IsIn(25, 26) && (DaysDelinquent != null && DaysDelinquent.Value > 0))
                return "COPBL";
            return string.Empty;
        }

        /// <summary>
        /// Sets the Endorser delinquency ARC
        /// </summary>
        /// <param name="bor">Borrower object</param>
        private string GetEndorserDelinquentArc(Borrower bor)
        {
            if (bor.ReportNumber == 12) return "DL201";
            else if (bor.ReportNumber == 13) return "DL401";
            else if (bor.ReportNumber == 15) return "DL801";
            else if (bor.ReportNumber == 16) return "DL911";
            else if (bor.ReportNumber == 17) return "DL141";
            else if (bor.ReportNumber == 18) return "DL171";
            else if (bor.ReportNumber == 19) return "DL203";
            else if (bor.ReportNumber == 20) return "DL231";
            else if (bor.ReportNumber == 23) return "DL201";
            else if (bor.ReportNumber == 24) return "DL401";
            else return string.Empty;
        }

    }
}