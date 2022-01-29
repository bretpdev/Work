using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payments
{
    class DayOfTheMonth
    {

        public int DayNumber { get; set; }

        public DayOfTheMonth(int tdayOfMonth)
        {
            DayNumber = tdayOfMonth;
        }

        public List<PossiblePaymentDate> CalculatePossibleMonths()
        {
            List<PossiblePaymentDate> months = new List<PossiblePaymentDate>();
            //calculate this month, next month, and the month after next
            string thisMonth = string.Format("{0}/{1}/{2}", DateTime.Today.Month.ToString(), DayNumber.ToString(), DateTime.Today.Year.ToString());
            string nextMonth = string.Format("{0}/{1}/{2}", DateTime.Today.AddMonths(1).Month.ToString(), DayNumber.ToString(), DateTime.Today.AddMonths(1).Year.ToString());
            string nextNextMonth = string.Format("{0}/{1}/{2}",DateTime.Today.AddMonths(2).Month.ToString(), DayNumber.ToString(), DateTime.Today.AddMonths(2).Year.ToString());
            
            //add months to list if due date would be in the future but not by more than 60 days
            if (DateTime.Parse(thisMonth) > DateTime.Today) 
            {
                months.Add(new PossiblePaymentDate(DateTime.Parse(thisMonth).ToString("MMM"), thisMonth));
            }
            if (DateTime.Parse(nextMonth) < DateTime.Today.AddMonths(2))
            {
                months.Add(new PossiblePaymentDate(DateTime.Parse(nextMonth).ToString("MMM"),nextMonth));
            }
            if (DateTime.Parse(nextNextMonth) < DateTime.Today.AddMonths(2))
            {
                months.Add(new PossiblePaymentDate(DateTime.Parse(nextNextMonth).ToString("MMM"),nextNextMonth));   
            }
            return months;
        }

        public override string ToString()
        {
            return DayNumber.ToString();
        }
    }
}
