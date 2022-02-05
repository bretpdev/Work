using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHONE4RMVL
{
    public class OriginalMobile
    {
        public string MBL { get; set; }
        public string Consent { get; set; }
        public string SourceCode { get; set; }
        public string lastVer { get; set; }
        public string PhoneType { get; set; }
        public string domNpa { get; set; }
        public string domNxx { get; set; }
        public string domLcl { get; set; }
        public string domExt { get; set; }
        public string forNpa { get; set; }
        public string forNxx { get; set; }
        public string forLcl { get; set; }
        public string forExt { get; set; }
        public string ValidPhone { get; set; }

        public bool foreign;
        public bool domestic;
        public void determineStatus()
        {
            if ((forNpa.Length > 0 && forNpa != "___" && forNpa != "   ") &&
                 (forNpa.Length > 0 && forNxx != "_____" && forLcl != "     "))

            {
                foreign = true;
            }
            else
            {
                foreign = false;
            }

            if ((domNpa.Length > 0 && domNpa != "___" && domNpa != "   ") &&
                (domNpa.Length > 0 && domNxx != "_____" && domLcl != "     "))

            {
                domestic = true;
            }
            else
            {
                domestic = false;
            }
        }
    }
}
