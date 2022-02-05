namespace BILLINGFED
{
    public class CommentsAndArcs
    {
        public string Comment { get; set; }
        public string Arc { get; set; }
        public string DelinquentArc1 { get; set; }
        public string DelinquentArc2 { get; set; }
        public string EndorserSsn { get; set; }

        /// <summary>
        /// Determines which ARC and comment is needed for the given borrower
        /// </summary>
        public CommentsAndArcs(int reportNumber, string acctNum, int daysDeliq, string endAcctNum)
        {
            Comment = "";
            Arc = "";
            DelinquentArc1 = "";
            DelinquentArc2 = "";
            EndorserSsn = null;

            switch (reportNumber)
            {
                case 2:
                    Comment = "Billing Statement sent to Borrower";
                    Arc = "BILLS";
                    break;
                case 3:
                    Comment = "AutoPay Notice sent to Borrower";
                    Arc = "BILLS";
                    break;
                case 4:
                    Comment = "Delinquent Billing Statement sent to Borrower";
                    Arc = "BILLS";
                    DelinquentArc1 = GetDeliquentArc1(daysDeliq);
                    break;
                case 5:
                    Comment = "Reduced Payment Forbearance Billing Statement sent to Borrower";
                    Arc = "BILLS";
                    break;
                case 9:
                    Comment = "Bill not sent, sent by previous servicer";
                    Arc = "BILLS";
                    break;
                case 10:
                    Comment = $"Billing Statement sent to Endorser for Borrower {acctNum}";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    break;
                case 11:
                    Comment = $"AutoPay Notice sent to Endorser for Borrower {acctNum}";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    break;
                case 12:
                    Comment = $"Delinquent Billing Statement sent to Endorser for Borrower {acctNum}";
                    Arc = "BILLE";
                    DelinquentArc2 = GetDeliquentArc2(daysDeliq);
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    break;
                case 13:
                    Comment = $"Reduced Payment Forbearance Billing Statement sent to Endorser for Borrower {acctNum}";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    break;
                case 15:
                    Comment = "Bill not sent, sent by previous servicer";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    break;
                case 16:
                    Comment = "Interest Statement sent to Borrower";
                    Arc = "BILLS";
                    break;
                case 17:
                    Comment = "Interest Statement sent to Endorser";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    break;
                case 18:
                    Comment = "Interest Notice sent to Borrower";
                    Arc = "BILLS";
                    break;
                case 19:
                    Comment = "Interest Notice sent to Endorser";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    break;
                case 20:
                    Comment = "Ebill sent to Borrower";
                    Arc = "BILLS";
                    return;
                case 21:
                    Comment = "Ebill sent to Endorser";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    return;
                case 22:
                    Comment = "Paid ahead sent to Borrower";
                    Arc = "BILLS";
                    return;
                case 23:
                    Comment = "Paid ahead sent to Endorser";
                    Arc = "BILLE";
                    EndorserSsn = BillingStatementsFed.DA.GetSsnFromAcctNum(endAcctNum);
                    return;
            }
        }

        /// <summary>
        /// Determines which Arc to use from the number of days delinquent
        /// </summary>
        /// <param name="daysDeliq">Number of days delinquent</param>
        /// <returns>ARC number as string</returns>
        private string GetDeliquentArc1(int daysDeliq)
        {
            if (daysDeliq < 16) { return ""; }
            else if (daysDeliq <= 30) { return "DL200"; }
            else if (daysDeliq <= 60) { return "DL400"; }
            else if (daysDeliq <= 90) { return "DL800"; }
            else if (daysDeliq <= 120) { return "DL910"; }
            else if (daysDeliq <= 150) { return "DL140"; }
            else if (daysDeliq <= 180) { return "DL170"; }
            else if (daysDeliq <= 210) { return "DL202"; }
            else { return "DL230"; }
        }

        /// <summary>
        /// Determines which Arc to use from the number of days delinquent for Endorser
        /// </summary>
        /// <param name="daysDeliq">Number of days delinquent</param>
        /// <returns>ARC number as string</returns>
        private string GetDeliquentArc2(int daysDeliq)
        {
            if (daysDeliq < 16) { return ""; }
            else if (daysDeliq <= 30) { return "DL201"; }
            else if (daysDeliq <= 60) { return "DL401"; }
            else if (daysDeliq <= 90) { return "DL801"; }
            else if (daysDeliq <= 120) { return "DL911"; }
            else if (daysDeliq <= 150) { return "DL141"; }
            else if (daysDeliq <= 180) { return "DL171"; }
            else if (daysDeliq <= 210) { return "DL203"; }
            else { return "DL231"; }
        }
    }
}