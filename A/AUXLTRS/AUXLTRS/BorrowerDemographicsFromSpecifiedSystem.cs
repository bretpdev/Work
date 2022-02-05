using Uheaa.Common.Scripts;

namespace AUXLTRS
{
    public class BorrowerDemographicsFromSpecifiedSystem
    {
        //This script uses SystemBorrowerDemographics objects, but needs to know
        //from which system they were populated.
        public enum AesSystem
        {
            Compass,
            OneLink
        }
        public AesSystem System { get; set; }
        public SystemBorrowerDemographics Demographics { get; set; }
    }
}