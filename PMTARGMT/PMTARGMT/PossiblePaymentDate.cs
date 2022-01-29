using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payments
{
    public class PossiblePaymentDate
    {

        public string Month { get; set; }
        public DateTime ActualDate { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tMonth">The month (will appear as text in combo box).</param>
        /// <param name="tActualDate">The actual date that the month was pulled from.</param>
        public PossiblePaymentDate(string tMonth, string tActualDate)
        {
            Month = tMonth;
            ActualDate = DateTime.Parse(tActualDate);
        }

        /// <summary>
        /// Overridded ToString function for combobox text. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Month;
        }

        /// <summary>
        /// The calculated due date (1st, 7th, 15th or 22nd)
        /// </summary>
        /// <returns></returns>
        public string CalculatedDueDay()
        {
            if (ActualDate.Day == 1)
            {
                return "1st";
            }
            else if (ActualDate.Day == 7)
            {
                return "7th";
            }
            else if (ActualDate.Day == 15)
            {
                return "15th";
            }
            else
            {
                return "22nd";
            }
        }

    }
}
