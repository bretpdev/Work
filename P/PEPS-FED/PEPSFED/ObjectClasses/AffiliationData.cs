using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEPSFED
{
    public class AffiliationData : ObjectBase
    {
        public override long RecordId { get; set; }
        public override string RecordType { get; set; } = "";
        public override string OpeId { get; set; } = "";
        public string ChangeIndicator { get; set; } = "";
        public string PreviousOpeId { get; set; } = "";
        public string CoaActnCd { get; set; } = "";
        public string CoaEfftDt { get; set; } = "";
        public string DefaultCoaCd { get; set; } = "";
        public string Filler { get; set; } = "";

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
