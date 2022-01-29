using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace PRIDRCRP.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestBorrowerInformationGetInformation()
        {
            BorrowerInformation info = new BorrowerInformation(null);

            string ssnLine = "ACCOUNT NO. 111111111-1";
            info.GetInformation(ssnLine);

            string interestLine = "INTEREST RATE 10.01%";
            info.GetInformation(interestLine);

            string firstPayLine = "FIRST PAY DUE 10/08/2018";
            info.GetInformation(firstPayLine);

            string payAmtLine = "PAYMENT AMOUNT $3.50";
            info.GetInformation(payAmtLine);

            string repayLine = "REPAY PLAN NO-DEBT";
            info.GetInformation(repayLine);

            Assert.IsTrue(info.borrowerRecord.Ssn == "111111111");
            Assert.IsTrue(info.borrowerRecord.InterestRate.Value == 10.01M);
            Assert.IsTrue(info.borrowerRecord.FirstPayDue.Value.ToShortDateString() == "10/8/2018");
            Assert.IsTrue(info.borrowerRecord.PaymentAmount.Value == 3.50M);
            Assert.IsTrue(info.borrowerRecord.RepayPlan == "NO-DEBT");
        }

        [TestMethod]
        public void TestBorrowerActivityInformationGetInformation()
        {
            //Test a borrower activity record spanning pages
            BorrowerActivityInformation info = new BorrowerActivityInformation(null);

            string headerRow = "ACTIVITY-------------------ACTIVITYDESCRIPTION-------------------NOTICE0";
            info.GetInformation(headerRow);

            string skipRow = "0";
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);

            string activityPart1 = "        10/08/18    This is the first part of the message";
            info.GetInformation(activityPart1);

            string pageBreak = "BORROWER HISTORY AND ACTIVITY REPORT";
            info.GetInformation(pageBreak);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);

            string activityPart2 = "                   This is the second part of the message";
            info.GetInformation(activityPart2);

            Assert.IsTrue(info.borrowerActivityRecords[0].ActivityDate.Value.ToShortDateString() == "10/8/2018");
            Assert.IsTrue(info.borrowerActivityRecords[0].ActivityDescription == "This is the first part of the message|This is the second part of the message");
        }

        [TestMethod]
        public void TestDisbrusementInformationGetInformation()
        {
            DisbursementInformation info = new DisbursementInformation(null);
            string firstHeader = "DISB INT LOAN DISB LOAN DISB AMT CAPITALIZED REFUND/BORR PAID PRIN BAL PAID LC/NSF0";
            info.GetInformation(firstHeader);

            string secondHeader = "DATE RATE TYPE # I.D. INTEREST CANCEL PRINCIPAL OUTSTANDING INTEREST PAID0";
            info.GetInformation(secondHeader);

            string skipRow = "0";
            info.GetInformation(skipRow);

            string disbRecordRow = "  10/08/18 3.160%   PLUS   01 00VALUETEST    $5,001.01        $0.02        $0.03   $4,001.02    $1,001.03    $1,501.04              0";
            info.GetInformation(disbRecordRow);

            Assert.IsTrue(info.disbursementRecords[0].DisbursementDate.Value.ToShortDateString() == "10/8/2018");
            Assert.IsTrue(info.disbursementRecords[0].InterestRate.Value == 3.160M);
            Assert.IsTrue(info.disbursementRecords[0].LoanType == "PLUS");
            Assert.IsTrue(info.disbursementRecords[0].DisbursementNumber == "01");
            Assert.IsTrue(info.disbursementRecords[0].LoanId == "00VALUETEST");
            Assert.IsTrue(info.disbursementRecords[0].DisbursementAmount.Value == 5001.01M);
            Assert.IsTrue(info.disbursementRecords[0].CapitalizedInterest.Value == 0.02M);
            Assert.IsTrue(info.disbursementRecords[0].RefundCancel.Value == 0.03M);
            Assert.IsTrue(info.disbursementRecords[0].BorrowerPaidPrincipal.Value == 4001.02M);
            Assert.IsTrue(info.disbursementRecords[0].PrincipalOutstanding.Value == 1001.03M);
            Assert.IsTrue(info.disbursementRecords[0].InterestPaid.Value == 1501.04M);
        }

        [TestMethod]
        public void TestPaymentHistoryInformationGetInformation()
        {
            PaymentHistoryInformation info = new PaymentHistoryInformation(null);

            string headerRow = "DESCRIPTION ACT DATE EFF DATE TOT PAID INT PAID PRIN PAID LC/NSF PD ACCRD INT LC/NSF DUE PRIN BAL";
            info.GetInformation(headerRow);

            string skipRow = "0";
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);
            info.GetInformation(skipRow);

            string transactionRow = "          PAYMENT           10/08/18  10/08/18      100.01       40.02       70.03                                       10,000.04  0";
            info.GetInformation(transactionRow);

            Assert.IsTrue(info.paymentRecords[0].Description == "PAYMENT");
            Assert.IsTrue(info.paymentRecords[0].ActionDate.Value.ToShortDateString() == "10/8/2018");
            Assert.IsTrue(info.paymentRecords[0].EffectiveDate.Value.ToShortDateString() == "10/8/2018");
            Assert.IsTrue(info.paymentRecords[0].TotalPaid.Value == 100.01M);
            Assert.IsTrue(info.paymentRecords[0].InterestPaid.Value == 40.02M);
            Assert.IsTrue(info.paymentRecords[0].PrincipalPaid.Value == 70.03M);
            Assert.IsTrue(!info.paymentRecords[0].LcNsfDue.HasValue);
            Assert.IsTrue(!info.paymentRecords[0].LcNsfPaid.HasValue);
            Assert.IsTrue(!info.paymentRecords[0].AccruedInterest.HasValue);
            Assert.IsTrue(info.paymentRecords[0].PrincipalBalance.Value == 10000.04M);
        }

        [TestMethod]
        public void TestPdfParserConvertPostNegativeToDecimal()
        {
            MonetaryPdfParser parser = new MonetaryPdfParser(null, null);

            decimal neg = parser.ConvertPostNegativeToDecimal("10.01-");
            decimal pos = parser.ConvertPostNegativeToDecimal("20.02");

            Assert.IsTrue(neg == -10.01M);
            Assert.IsTrue(pos == 20.02M);
        }
    }
}
