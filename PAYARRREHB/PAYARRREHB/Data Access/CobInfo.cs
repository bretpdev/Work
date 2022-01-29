using Uheaa.Common.DataAccess;

namespace PAYARRREHB
{
    public class CobInfo
    {
        [DbName("DF_PRS_ID_BR")]
        public string BorSsn { get; set; }

        [DbName("DF_PRS_ID_EDS")]
        public string CoBorSsn { get; set; }

        [DbName("ISCOBOW")]
        public int isCob { get; set; }
    }
}
