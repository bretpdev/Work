using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTHIST
{
    public class ExcelRow
    {
        public string EffectiveDate { get; set; } //Effective Date, EffDate
        public string PaymentType { get; set; } //Payment Type, PmtType
        public decimal OrigianlPrincipal { get; set; } //Beginning Principal
        public decimal PaymentAmount { get; set; } //Payment Amount, PmtAmt
        public decimal Principal { get; set; } //Principal Paid, PrincCol
        public decimal Interest { get; set; } //Interest Paid, IntCol
        public decimal Legal { get; set; } //Legal Costs Paid
        public decimal Other { get; set; } //Other Costs Paid
        public decimal CollectionCosts { get; set; } //Collection Costs Paid, CC
        public decimal Overpayment { get; set; } //Overpayment or Reversal Amount, OV
        public string ReversalType { get; set; } //Reversal Type, RevType
    }
}
