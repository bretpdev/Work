using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace VERFORBFED
{
    public class ForbearanceDetails
    {
        private string Ssn { get; set; }
        private ReflectionInterface RI { get; set; }
        private DataAccess DA { get; set; }

        public DateTime? StartDate { get; set; }
        public bool HadDefForb { get; set; }
        public decimal DaysOfForbearanceUsed { get; internal set; }
        public List<CollectionSuspenseForbearance> CollectionForbDetails { get; internal set; }
        public bool BorrowerIsDeliquent { get; set; }
        private string numberOfMonthsRequested;
        public string NumberOfMonthsRequested
        {
            get
            {
                if (string.IsNullOrEmpty(numberOfMonthsRequested))
                    return "0";
                else
                    return numberOfMonthsRequested;
            }
            set
            {
                numberOfMonthsRequested = value;
            }
        }

        public DateTime EndDate { get; set; }
        public string ForbearanceReason { get; set; }
        public bool InvalidEndDate { get; set; }
        private DateTime today;
        public ForbearanceDetails(string ssn, string accountNumber, DataAccess da, ReflectionInterface ri, DateTime today)
        {
            Ssn = ssn;
            DA = da;
            RI = ri;
            CollectionForbDetails = DA.GetCollectionSuspenseForbInfo();
            DaysOfForbearanceUsed = DA.GetNumberOfForbDaysUsed();
            this.today = today;
        }

        /// <summary>
        /// Set the forbearance start date.
        /// </summary>
        /// <param name="defermentEndDate">possible deferment end date</param>
        /// <param name="forbearanceEndDate">possible forbearance end date</param>
        public void SetForbearanceStartDate(DateTime? defermentEndDate, DateTime? forbearanceEndDate, bool is120)
        {
            if (is120)
            {
                StartDate = today.AddDays(-119);
                return;
            }
            DateTime? ddo = DA.GetDateDelinquencyOccurred();

            //If the borrower is Delinquent set the start date as the date the Delinquency occurred. 
            if (ddo.HasValue)
                StartDate = ddo;
            else if (defermentEndDate.HasValue)//if the borrower is in a deferment set the start date to the day after the deferment ends
            {
                HadDefForb = true;
                StartDate = defermentEndDate.Value.AddDays(1);
            }
            else if (forbearanceEndDate.HasValue)//if the borrower is in a forbearance set the start date to the day after the forbearance end
            {
                HadDefForb = true;
                if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone && forbearanceEndDate.Value.Month % 3 == 0 && forbearanceEndDate.Value.Day >= 30)
                {
                    InvalidEndDate = true;
                    return;
                }
                StartDate = forbearanceEndDate.Value.AddDays(1);
            }
            else//if none of the conditions are met set start date as the current date.
                StartDate = today;
        }

        /// <summary>
        /// Calculates the forbearance end date based upon the start date  and the number of months requested.
        /// </summary>
        /// <param name="nextPayDue">Next bill due date, this will be used if the borrower only wants to clear delinquency</param>
        public void CalculateForbearanceEndDate(DateTime? nextPayDue, bool is120)
        {
            if (!nextPayDue.HasValue)
            {
                DateTime? ddo = DA.GetDateDelinquencyOccurred();
                if (ddo.HasValue)
                {
                    nextPayDue = new DateTime(today.Year, today.Month, ddo.Value.Day);
                }
            }
            if (is120)
            {
                EndDate = today;
                return;
            }
            //just clearing delinquency no future time needed.
            if (HadDefForb || nextPayDue < DateTime.Now.AddDays(-30))
                EndDate = StartDate.Value.AddMonths(int.Parse(NumberOfMonthsRequested));
            else if (NumberOfMonthsRequested == "0")
                EndDate = nextPayDue.HasValue ? nextPayDue.Value : today.AddMonths(1);
            else
                EndDate = nextPayDue.HasValue ? nextPayDue.Value.AddMonths(int.Parse(NumberOfMonthsRequested)) : StartDate.Value.AddMonths(int.Parse(NumberOfMonthsRequested));

        }
    }
}
