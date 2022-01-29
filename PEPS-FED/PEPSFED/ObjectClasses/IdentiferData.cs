using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEPSFED
{
    public class IdentiferData : ObjectBase
    {
        public override long RecordId { get; set; }
        public override string RecordType { get; set; }
        public override string OpeId { get; set; }
        public string ChangeIndicator { get; set; }
        public string TinCurrent { get; set; }
        public string TinPrevious { get; set; }
        public string CmoGdunsNbrCurrent { get; set; }
        public string CmoGdunsNbrPrevious { get; set; }
        public string PellGdunsNbrCurrent { get; set; }
        public string PellGdunsNbrPrevious { get; set; }
        public string Filler1 { get; set; }
        public string FdslpGdunsNbrCurrent { get; set; }
        public string FdslpGdunsNbrPrevious { get; set; }
        public string Filler2 { get; set; }
        public string CampusBasedGdunsNbrCurrent { get; set; }
        public string CampusBasedGdunsNbrPrevious { get; set; }
        public string PellIdCurrent { get; set; }
        public string PellIdPrevious { get; set; }
        public string FfelIdCurrent { get; set; }
        public string FfelIdPrevious { get; set; }
        public string FdslpIdCurrent { get; set; }
        public string FdslpIdPrevious { get; set; }
        public string CampusBasedIdCurrent { get; set; }
        public string CampusBasedIdPrevious { get; set; }
        public string FedSchlCdCurrent { get; set; }
        public string FedSchlCdPrevious { get; set; }
        public string FdslpSeqNbrCurrent { get; set; }
        public string FdslpSeqNbrPrevious { get; set; }
        public string Filler { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var info in GetType().GetProperties())
            {
                var value = info.GetValue(this, null);
                if (value == null)
                    value = "";
                sb.Append(info.Name + ": " + value.ToString().Trim() + " ");
            }
            return sb.ToString();
        }
    }
}
