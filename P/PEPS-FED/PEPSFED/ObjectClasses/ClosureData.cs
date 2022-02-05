using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEPSFED
{
    public class ClosureData : ObjectBase
    {
        public override long RecordId { get; set; }
        public override string RecordType { get; set; }
        public override string OpeId { get; set; }
        public string ChangeIndicator { get; set; }
        public string ClosureDtCurrent { get; set; }
        public string ClosureDtPrevious { get; set; }
        public string HistoryCd { get; set; }
        public string UnauthorizedLocationInd { get; set; }
        public string TuitionRecoveryFund { get; set; }
        public string PerkinsInd { get; set; }
        public string KnownAmount { get; set; }
        public string StateBondInd { get; set; }
        public string SchoolBondAmount { get; set; }
        public string RecordHolderDescription { get; set; }
        public string VerifiedBy { get; set; }
        public string CreatedOnDt { get; set; }
        public string ModifiedDt { get; set; }
        public string Filler { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var info in GetType().GetProperties())
            {
                var value = info.GetValue(this, null);
                if (value == null)
                    value = "";
                sb.AppendLine(info.Name + ": " + value.ToString().Trim());
            }
            return sb.ToString();
        }
    }
}
