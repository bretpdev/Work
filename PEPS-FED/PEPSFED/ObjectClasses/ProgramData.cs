using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEPSFED
{
    public class ProgramData : ObjectBase
    {
        public override long RecordId{ get; set; }
        public override string RecordType { get; set; }
        public override string OpeId { get; set; }
        public string ChangeIndicator { get; set; }
        public string FpellStartDt { get; set; }
        public string FpellEndDt { get; set; }
        public string FpellApprovInd { get; set; }
        public string FfelStafStartDt { get; set; }
        public string FfelStafEndDt { get; set; }
        public string FfelStafApprovInd { get; set; }
        public string FfelStafUnsubStartDt { get; set; }
        public string FfelStafUnsubEndDt { get; set; }
        public string FfelStafUnsubApprovInd { get; set; }
        public string FfelPlusStartDt { get; set; }
        public string FfelPlusEndDt { get; set; }
        public string FfelPlusApprovInd { get; set; }
        public string FfelSlsStartDt { get; set; }
        public string FfelSlsEndDt { get; set; }
        public string FfelSlsApprovInd { get; set; }
        public string FdslpStafStartDt { get; set; }
        public string FdslpStafEndDt { get; set; }
        public string FdslpStafApprovInd { get; set; }
        public string FdslpStafUnsubStartDt { get; set; }
        public string FdslpStafUnsubEndDt { get; set; }
        public string FdslpStafUnsubApprovInd { get; set; }
        public string FdslpPlusStartDt { get; set; }
        public string FdslpPlusEndDt { get; set; }
        public string FdslpPlusApprovInd { get; set; }
        public string FperkinsStartDt { get; set; }
        public string FperkinsEndDt { get; set; }
        public string FperkinsApprovInd { get; set; }
        public string FseogStartDt { get; set; }
        public string FseogEndDt { get; set; }
        public string FseogApprovInd { get; set; }
        public string FwsPrivSecEmplStartDt { get; set; }
        public string FwsPrivSecEmpEndDt { get; set; }
        public string FwsPrivSecEmplApprovInd { get; set; }
        public string FwsJobLocDevStartDt { get; set; }
        public string FwsJobLocDevEndDt { get; set; }
        public string FwsJobLocDevApprovInd { get; set; }
        public string FwsComServStartDt { get; set; }
        public string FwsComServEndDt { get; set; }
        public string FwsComServApprovInd { get; set; }
        public string Filler { get; set; }
        public DateTime? FfelStaffordStartDate { get; set; }
        public DateTime? FfelUnsubsidizedStaffordStartDate { get; set; }
        public DateTime? FfelPlusStartDate { get; set; }
        public DateTime? FfelSlsStartDate { get; set; }
        public DateTime? DlStaffordStartDate { get; set; }
        public DateTime? DlUnsibsidizedStaffordStartDate { get; set; }
        public DateTime? DlPlusStartDate { get; set; }
        public DateTime? FederalPerkinsStartDate { get; set; }

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

        public void FormatProperties()
        {
            FfelStaffordStartDate = ParseDate(FfelStafStartDt.Trim(), this);
            FfelStafApprovInd = FfelStafApprovInd.Trim();
            FfelUnsubsidizedStaffordStartDate = ParseDate(FfelStafUnsubStartDt, this);
            FfelStafUnsubApprovInd = FfelStafUnsubApprovInd.Trim();
            FfelPlusStartDate = ParseDate(FfelPlusStartDt, this);
            FfelPlusApprovInd = FfelPlusApprovInd.Trim();
            FfelSlsStartDate = ParseDate(FfelSlsStartDt, this);
            FfelSlsApprovInd = FfelSlsApprovInd.Trim();
            DlStaffordStartDate = ParseDate(FdslpStafStartDt, this);
            FdslpStafApprovInd = FdslpStafApprovInd.Trim();
            DlUnsibsidizedStaffordStartDate = ParseDate(FdslpStafUnsubStartDt, this);
            FdslpStafUnsubApprovInd = FdslpStafUnsubApprovInd.Trim();
            DlPlusStartDate = ParseDate(FdslpPlusStartDt, this);
            FdslpPlusApprovInd = FdslpPlusApprovInd.Trim();
            FederalPerkinsStartDate = ParseDate(FperkinsStartDt, this);
            FperkinsApprovInd = FperkinsApprovInd.Trim();
        }
    }
}
