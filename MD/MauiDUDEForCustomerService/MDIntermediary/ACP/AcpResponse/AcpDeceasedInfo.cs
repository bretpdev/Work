using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class AcpDeceasedInfo
    {
        public string ThirdPartyKnowsBorrower { get; set; }
        public string SpeakingTo { get; set; }
        public string LetterOfPermissionOnFile { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string DeathOccurredInformation { get; set; }
        public string AbleToSendOriginalDeathCertificate { get; set; }
        public string ClosestLivingRelativeInformation { get; set; }
        public string NameOfFuneralHome { get; set; }

        public string GenerateComment()
        {
            return string.Format("{0} CALLED INDICATES BORROWER DECEASED - BORROWER DIED ON {1} IN {2} - {3} - ADVISED TO CONTACT {4} - {5}",
                SpeakingTo, DateOfDeath.ToShortDateString(), DeathOccurredInformation,
                AbleToSendOriginalDeathCertificate == "Y" ? "WILL SEND DEATH CERTIFICATE" : "WILL NOT SEND DEATH CERTIFICATE",
                ClosestLivingRelativeInformation, NameOfFuneralHome);
        }
    }
}
