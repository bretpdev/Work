using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace PEPSFED
{
    public class DetailData :ObjectBase
    {
        public override long RecordId { get; set; }
        public override string RecordType { get; set; }
        public override string OpeId { get; set; }
        public string ChangeIndicator { get; set; }
        public string SchoolName { get; set; }
        public string LocName { get; set; }
        public string Line1Adr { get; set; }
        public string Line2Adr { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string ForeignProvinceName { get; set; }
        public string EligStatusInd { get; set; }
        public string CertTypeCd { get; set; }
        public string ApprovInd { get; set; }
        public string ActnCd { get; set; }
        public string ActnReasonCd { get; set; }
        public string ActnDt { get; set; }
        public string PpaSentDt { get; set; }
        public string PpaExecutionDt { get; set; }
        public string PpaExpirationDt { get; set; }
        public string PgmLength { get; set; }
        public string SchType { get; set; }
        public string AcadCal { get; set; }
        public string EthnicCd { get; set; }
        public string Surety { get; set; }
        public string RegionCd { get; set; }
        public string CongDist { get; set; }
        public string SicCd { get; set; }
        public string FaadsCd { get; set; }
        public string CloseDt { get; set; }
        public string InitApprDt { get; set; }
        public string DisapprovalDt { get; set; }
        public string LocationReason { get; set; }
        public string SystemFundedOfficeInd { get; set; }
        public string BranchInd { get; set; }
        public string CaseTeamCd { get; set; }
        public string ReinstateDt { get; set; }
        public string WebPage { get; set; }
        public string Filler { get; set; }
        public DateTime? EligibilityActionDate { get; set; }
        public DateTime? CloseDate { get; set; }

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
            SchoolName = RemoveSpecialCharacters(SchoolName.Trim());
            Line1Adr = Line1Adr.Trim();
            Line2Adr = Line2Adr.Trim();
            City = City.Trim();
            State = State.Trim();
            Country = Country.Trim();
            Zip = Zip.Trim();
            if (Zip.Length == 9 && Zip.SafeSubString(5, 4) == "0000")
                Zip = Zip.SafeSubString(0, 5);
            ForeignProvinceName = ForeignProvinceName.Trim();
            ActnCd = ActnCd.Trim();
            ActnReasonCd = ActnReasonCd.Trim();
            string eligibilityActionDate = ActnDt.Trim();
            EligibilityActionDate = ParseDate(eligibilityActionDate, this);
            PgmLength = PgmLength.Trim();
            SchType = SchType.Trim();
            string closeDate = CloseDt.Trim();
            CloseDate = ParseDate(closeDate, this);
        }
    }
}
