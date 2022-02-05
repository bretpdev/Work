using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLINFO
{
    public class EnrollmentData
    {
        //public enum EnrollmentSource
        //{
        //    NSLDS, //I
        //    NCH, //M
        //    School //D
        //}

        //Populated by EnrollmentInformation form
        public string AccountIdentifier { get; set; }
        public string SchoolCode { get; set; }
        public string Source { get; set; }
        public string SourceText { get; set; }
        public bool EVRHistoryContainsEnrollmentInformation { get; set; }
        //Populated by parsing EnrollmentInformation form data
        //public string Ssn { get; set; }
        //public string AccountNumber { get; set; }
    }
}
