using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace CPINTRTLPD
{
    class CsvRow
    {
        [CsvLineNumber]
        public int LineNumber { get; set; }
        [CsvLineContent]
        public string LineContent { get; set; }
        public string PC_STA_LPD06 { get; set; }
        public string IC_LON_PGM { get; set; }
        public string PF_RGL_CAT { get; set; }
        public string IF_GTR { get; set; }
        public string IF_OWN { get; set; }
        public string IF_BND_ISS { get; set; }
        public string PC_ITR_TYP { get; set; }
        public DateTime PD_EFF_SR_LPD06 { get; set; }
        public DateTime PD_EFF_END_LPD06 { get; set; }

        public override string ToString()
        {
            return "Line " + LineNumber + ": " + LineContent;
        }
    }
}
