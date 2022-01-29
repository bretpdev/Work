using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace PHONE4RMVL
{
    public class Borrowers
    {
        [DbName ("MPerson")]
        public string borrower { get; set; }

        [DbName("MPersonID")]
        public string Df_Prs_Id { get; set; }

        [DbName("BorrSsn")]
        public string borrSsn { get; set; }

        [DbName("IsCoborrow")]
        public int IsCoborow { get; set; }

        //[DbName("ORIGALLOW")]
        //public string origAllow { get; set; }

        [DbName("DOM_AC")]
        public string domArea { get; set; }

        [DbName("DOM_EXCH")]
        public string domExch { get; set; }

        [DbName("DOM_LCL")]
        public string domLocal { get; set; }

        [DbName("FGN_Intl")]
        public string fgnIntl { get; set; }

        [DbName("FGN_CNY")]
        public string forArea { get; set; }

        [DbName("FGN_CTY")]
        public string forExch { get; set; }

        [DbName("FGN_LCL")]
        public string forLcl { get; set; }

        [DbName("EXT")]
        public string domExt { get; set; }

        [DbName("SOURCE")]
        public string phSource { get; set; }

        [DbName("MBL")]
        public string MBL { get; set; }

        [DbName("CONSENT")]
        public string consent { get; set; }

        public string forExt { get; set; }
        public bool domestic;
        public bool foreign;

        public void determineStatus()
        {
            if ((forArea.Length > 0  && forArea != "___"  && forArea != "   ") &&
                 (forExch.Length > 0 && forExch != "_____" && forExch != "     "))

            {
                foreign = true;
            }
             else
            {
                foreign = false;
            }

            if ((domArea.Length > 0 && domArea != "___" && domArea != "   ") &&
                (domExch.Length > 0 && domExch != "_____" && domExch != "     "))

            {
                domestic = true;
            }
            else
            {
                domestic = false;
            }
        }

        public void phoneScrape(ReflectionInterface ri)
        {

            domArea = ri.GetText(17, 14, 3).Replace("_", "");
            domExch = ri.GetText(17, 23, 3).Replace("_", "");
            domLocal = ri.GetText(17, 31, 4).Replace("_", "");
            domExt = ri.GetText(17, 40, 4).Replace("_", "");
            forArea = ri.GetText(18, 14, 4).Replace("_", "");
            forExch = ri.GetText(18, 24, 5).Replace("_", "");
            forLcl = ri.GetText(18, 35, 13).Replace("_", "");
            forExt = ri.GetText(18, 53, 4).Replace("_", "");
            

        }
    }
}
