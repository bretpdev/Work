using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I1I2SCHLTR
{
    public class SchoolData
    {
        public string School { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolAddress2 { get; set; }
        public string SchoolAddress3 { get; set; }
        public string SchoolCity { get; set; }
        public string SchoolState { get; set; }
        public string SchoolZip { get; set; }
        public string Lender { get; set; }
        public string CostCenterCode { get; set; }

        public List<PrintData> Borrowers;

        public SchoolData()
        {
            Borrowers = new List<PrintData>();
        }
    }
}
