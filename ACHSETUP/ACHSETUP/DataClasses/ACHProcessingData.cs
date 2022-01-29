using Uheaa.Common.Scripts;

namespace ACHSETUP
{
    public class ACHProcessingData
    {

        /// <summary>
        /// Tracks whether or not a check by phone was created
        /// </summary>
        public bool CheckByPhoneCreated { get; set; }
        /// <summary>
        /// Borrower's demos gathered from TX1J
        /// </summary>
        public SystemBorrowerDemographics BorrowerDemos { get; set; }

        
        public ACHProcessingData()
            : base()
        {
            CheckByPhoneCreated = false;
        }

    }
}