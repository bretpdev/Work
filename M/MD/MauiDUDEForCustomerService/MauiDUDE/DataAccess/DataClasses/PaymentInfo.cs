using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class PaymentInfo
    {
        public decimal CurrentAmountDue { get; set; }
        public decimal AmountPastDue { get; set; }
        public decimal TotalAmountDue { get; set; }
        public decimal TotalAmountPlusLateFees { get; set; }

        public PaymentInfo(decimal currentAmountDue, decimal amountPastDue, decimal totalAmountDue, decimal totalAmountPlusLateFees)
        {
            CurrentAmountDue = currentAmountDue;
            AmountPastDue = amountPastDue;
            TotalAmountDue = totalAmountDue;
            TotalAmountPlusLateFees = totalAmountPlusLateFees;
        }
    }
}
