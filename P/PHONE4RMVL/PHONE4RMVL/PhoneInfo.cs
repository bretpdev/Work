using System.Threading.Tasks;

namespace PHONE4RMVL
{
    public class PhoneInfo
    {
        public string DomAC { get; set; }
        public string DomExch { get; set; }
        public string DomLcl { get; set; }
        public string DomExt { get; set; }
        public string DomValid { get; set; }

       


        public string IntlCntry { get; set; }
        public string IntlCty { get; set; }
        public string IntlLcl { get; set; }
        public string IntlExt { get; set; }

        public string ForCny { get; set; }
        public string ForCity { get; set; }
        public string ForLcl { get; set; }

        public int determineVacancy()
        {
            if (IntlCntry == "" && IntlCty == "" && IntlLcl == null)
                return 1;

            if (DomAC == "" && DomExch == "" && DomExt == "")
                return 2;

            return 0;
        }

        public void Init()
        {
            DomAC = "";
            DomExch = "";
            DomLcl = "";
            DomExt = "";
            DomValid = "";
            DomValid = "";
            IntlCntry = "";
            IntlCty = "";
            IntlExt = "";
        }

    }
}
