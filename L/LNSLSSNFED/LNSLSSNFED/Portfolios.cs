using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNSLSSNFED
{
    public class Portfolios
    {
        public Dictionary<string, string> DLO;

        public Dictionary<string, string> LNC;

        public Portfolios()
        {
            DLO = new Dictionary<string, string>
            {
                {"DLSTFD","DLSTFD"},{"DLUNST","DLUNST"},{"DLPLGB","DLPLGB"},{"DLPLUS","DLPLUS"}
            };

            LNC = new Dictionary<string, string>
            {
                {"DLPCNS","DLPCNS"},{"DLSCNS","DLSCNS"},{"DLUSPL","DLUSPL"},{"DLSSPL","DLSSPL"},{"DLUCNS","DLUCNS"}
            };
        }
    }
}
