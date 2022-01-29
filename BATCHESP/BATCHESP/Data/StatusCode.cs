using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BATCHESP
{
    /// <summary>
    /// Codes used for Enrollment Statuses and School Separation Reasons.
    /// </summary>
    class StatusCode
    {
        //ENUM wasnt used, as comparisons against the values makes more sense as string.
        public const string Enrolled = "00";
        public const string Graduated = "01";
        public const string Withdrew = "02";
        public const string Transfer = "03";
        public const string Dismissed = "04";
        public const string OnLeave = "05";
        public const string Deceased = "06";
        public const string NeverEnrolled = "07";
        public const string LessThanHalfTime = "08";
        public const string DesertStorm = "09";
        public const string HalfTime = "10";
        public const string FullTime = "11";
        public const string SchoolClosure = "12";
        public const string IneligibleBorrower = "13";
        public const string ExpectedGradeLetter = "14";
        public const string ActiveDuty = "15";
        public const string LastDateOfAttendance = "16";
        public const string Egd = "17";
        public const string NoRecordFound = "18";
        public const string ThreeQuartersTime = "19";
    }
}
