using System.Collections.Generic;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;

namespace CSLSLTRFED
{
    public class EndorserData
    {
        public string Ssn { get; set; }
        public List<int> SequenceNumbers { get; set; }
        public SystemBorrowerDemographics EndorserDemo { get; set; }
        public EcorrData EndorserEcorr { get; set; }

        public EndorserData()
        {
            SequenceNumbers = new List<int>();
            EndorserEcorr = new EcorrData();
        }
    }

    public class Endorsers
    {
        public string LF_EDS { get; set; }
        public int LN_SEQ { get; set; }
    }
}