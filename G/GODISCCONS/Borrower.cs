using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using Key = Q.ReflectionInterface.Key;
using Microsoft.VisualBasic;

namespace GODISCCONS
{
    class Borrower
    {
        //system gathered values
        public string SSN { get; set; }
        public string TargetAppID { get; set; }
        public string EndorserID { get; set; }
        public SystemBorrowerDemographics Demos { get; set; }
        public SystemBorrowerDemographics EndorserDemos { get; set; }
        public string BorrowerKeyLine { get; set; }
        public string EndorserKeyLine { get; set; }
        public string LoanTermBegin { get; set; }
        public string LoanTermEnd { get; set; }
        public string GradDate { get; set; }
        public string CLUID { get; set; }
        public double GuarAmount { get; set; }
        public double IntRate { get; set; }
        public double WsjRate { get; set; }
        public string SchoolName { get; set; }
        //calculated values
        public double TotalInterestAtCurrentRate { get; set; }
        public double TotalInterestAt18Percent { get; set; }
        public double InSchoolInterestAtCurrentRate { get; set; }
        public double InSchoolInterestAt18Percent { get; set; }
        public double APR { get; set; }
        public double EstimatedPaymentAtCurrentRate { get; set; }
        public double EstimatedPaymentAt18Percent { get; set; }
        public string RepaymentStart { get; set; }
        public string LastRepaymentDate { get; set; }
        public string LastRepaymentDateAt18Percent { get; set; }
        public string SecondToLastRepaymentDate { get; set; }
        public string SecondToLastRepaymentDateAt18Percent { get; set; }
        public double TotalCostAtCurrentRate { get; set; }
        public double TotalCostAt18Percent { get; set; }
        public int NumberOfPaymentsAtCurrentRate { get; set; }
        public int NumberOfPaymentsAt18Percent { get; set; }
        public string CancellationDate { get; set; }



        public Borrower(string ssn)
        {
            SystemBorrowerDemographics Demos = new SystemBorrowerDemographics();
            SystemBorrowerDemographics EndorserDemos = new SystemBorrowerDemographics();
            SSN = ssn;
            TargetAppID = string.Empty;
            EndorserID = string.Empty;
            LoanTermBegin = string.Empty;
            LoanTermEnd = string.Empty;
            GradDate = string.Empty;
            CLUID = string.Empty;
            GuarAmount = 0;
            IntRate = 0;
            WsjRate = 0;
            SchoolName = string.Empty;
            BorrowerKeyLine = string.Empty;
            EndorserKeyLine = string.Empty;
        }


        public void PerformCalculations()
        {
            //general values
            BorrowerKeyLine = Q.Common.ACSKeyLine(SSN,Common.ACSKeyLinePersonType.Borrower, Common.ACSKeyLineAddressType.Legal);
            if (EndorserID != string.Empty){EndorserKeyLine = Q.Common.ACSKeyLine(EndorserID, Common.ACSKeyLinePersonType.Borrower, Common.ACSKeyLineAddressType.Legal);}
            double UsableIntRate = IntRate / 100; //interest rate used in calculations below must be divided by 100 to be in the right format
            APR = IntRate;

            //interest until repayment begins
            RepaymentStart = DateTime.Parse(GradDate).AddDays(150).ToString("MM/dd/yyyy");
            TimeSpan DaysInSchool = DateTime.Parse(GradDate).Subtract(DateTime.Parse(LoanTermBegin));
            InSchoolInterestAtCurrentRate = GuarAmount * (UsableIntRate / 365) * DaysInSchool.Days;
            InSchoolInterestAt18Percent = GuarAmount * (.18 / 365) * DaysInSchool.Days;

            //current rate
            EstimatedPaymentAtCurrentRate = Math.Round((GuarAmount * UsableIntRate / 12) / (1 - Math.Pow(1 + UsableIntRate / 12, -360)), 2);
            if (EstimatedPaymentAtCurrentRate >= 100)
            {
                NumberOfPaymentsAtCurrentRate = 360;
            }
            else
            {
                EstimatedPaymentAtCurrentRate = 100;
                NumberOfPaymentsAtCurrentRate = int.Parse(Math.Round(Financial.NPer((UsableIntRate / 12), -100, GuarAmount, 0, DueDate.EndOfPeriod), 0).ToString());
            }
            TotalInterestAtCurrentRate = (EstimatedPaymentAtCurrentRate * NumberOfPaymentsAtCurrentRate) - GuarAmount;
            TotalCostAtCurrentRate = EstimatedPaymentAtCurrentRate * NumberOfPaymentsAtCurrentRate;
            LastRepaymentDate = DateTime.Parse(RepaymentStart).AddMonths(NumberOfPaymentsAtCurrentRate).ToString("MM/dd/yyyy");
            SecondToLastRepaymentDate = DateTime.Parse(LastRepaymentDate).AddDays(-30).ToString("MM/dd/yyyy");
            
            //18 percent
            EstimatedPaymentAt18Percent = Math.Round((GuarAmount * .18 / 12) / (1 - Math.Pow(1 + .18 / 12, -360)), 2);
            if (EstimatedPaymentAt18Percent >= 100)
            {
                NumberOfPaymentsAt18Percent = 360;
            }
            else
            {
                EstimatedPaymentAt18Percent = 100;
                NumberOfPaymentsAt18Percent = int.Parse(Math.Round(Financial.NPer((.18 / 12), -100, GuarAmount, 0, DueDate.EndOfPeriod), 0).ToString()); ;
            }
            TotalInterestAt18Percent = (EstimatedPaymentAt18Percent * NumberOfPaymentsAt18Percent) - GuarAmount;
            TotalCostAt18Percent = EstimatedPaymentAt18Percent * NumberOfPaymentsAt18Percent;
            LastRepaymentDateAt18Percent = DateTime.Parse(RepaymentStart).AddMonths(NumberOfPaymentsAt18Percent).ToString("MM/dd/yyyy");
            SecondToLastRepaymentDateAt18Percent = DateTime.Parse(LastRepaymentDateAt18Percent).AddDays(-30).ToString("MM/dd/yyyy");
            
            //cancellation date
            CancellationDate = DateTime.Today.AddDays(5).ToString("MM/dd/yyyy");
            //add one day at a time until the date is on a business day
            //while (DateTime.Parse(CancellationDate).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(CancellationDate).DayOfWeek == DayOfWeek.Sunday)
            //{
            //    CancellationDate = DateTime.Parse(CancellationDate).AddDays(1).ToString("MM/dd/yyyy");
            //}
        }
    }
}
