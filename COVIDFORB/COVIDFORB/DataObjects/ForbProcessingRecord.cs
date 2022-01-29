using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COVIDFORB
{
    public class ForbProcessingRecord
    {
        public long ForbearanceProcessingId { get; set; }
        public string AccountNumber { get; set; }
        public string ForbCode { get; set; }
        public DateTime DateRequested { get; set; }
        public string ForbearanceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateCertified { get; set; }
        public string CoMakerEligibility { get; set; }
        public string AuthorizedToExceedMax { get; set; }
        public string ForbToClearDelq { get; set; }
        public string CapitalizeInterest { get; set; }
        public string PaymentAmount { get; set; }
        public string SignatureOfBorrower { get; set; }
        public bool SelectAllLoans { get; set; }
        public long BusinessUnitId { get; set; }
        public string SubType { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolEnrollment { get; set; }
        public string ReservistNationalGuard { get; set; }
        public string DodForm { get; set; }
        public string SignatureOfOfficial { get; set; }
        public string PhysiciansCertification { get; set; }
        public string MedicalInternship { get; set; }
        public string StateLicensingCertificationProvided { get; set; }
    }
}
