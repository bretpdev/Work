using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    /// <summary>
    /// Stores the Income Source and whether the income is Taxable
    /// </summary>
    public class AdoiIncomeInput
    {
        public IncomeSources? IncomeSource { get; set; }
        public bool? TaxableIncome { get; set; }
        public bool? AgiReflectsCurrentIncome { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public bool SupportingDocsRequired { get; set; }
        public IEnumerable<PayStubs> Paystubs { get; set; }
    }
}
