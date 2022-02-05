using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHONE4RMVL
{
    public class ScreenInfo
    {
        public string PhoneType { get; set; }
        public string MBL { get; set; }
        public string Consent { get; set; }
        public string MobSta { get; set; }
        public string LastVer { get; set; }
        public string SrcCd { get; set; }
        public string ValidPhone { get; set; }

        public string domArea { get; set; }
        public string domExch { get; set; }
        public string domLocl { get; set; }
        public string domExtn { get; set; }

        public string intlArea { get; set; }
        public string intlExch { get; set; }
        public string intlLocl { get; set; }
        public string intlExtn { get; set; }

        public void update(Borrowers bor, OriginalMobile orig)
        {
            if (bor.domestic)
            {
                domArea = bor.domArea = orig.domNpa;
                domExch = bor.domExch = orig.domNxx;
                domLocl = bor.domLocal = orig.domLcl;
                domExtn = bor.domExt = orig.domExt;
                intlArea = "";
                intlExch = "";
                intlLocl = "";
                intlExtn = "";
            }
            else
            {
                intlArea = bor.forArea = orig.forNpa;
                intlExch = bor.forExch = orig.forNxx;
                intlLocl = bor.forLcl = orig.forLcl;
                intlExtn = bor.fgnIntl = orig.forExt;
                domArea = "";
                domExch = "";
                domLocl = "";
                domExtn = "";
            }

            Consent = bor.consent;
            MBL = bor.MBL;
            SrcCd = bor.phSource ;
        }

        public void replace(OriginalMobile om, bool intl)
        {
            if (!intl)
            {
                domArea = om.domNpa;
                domExch = om.domNxx;
                domLocl = om.domLcl;
                domExtn = om.domExt;
            }
            else
            {
                intlArea = om.forNpa;
                intlExch = om.forNxx;
                intlLocl = om.forLcl;
                intlExtn = om.forExt;
            }

        }
    }
}
