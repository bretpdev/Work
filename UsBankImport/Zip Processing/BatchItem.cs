using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsBankImport
{
    public class BatchItem
    {
        public string Group { get; set; }
        public string BatchItemNumber { get; set; }
        public string CheckImage { get; set; }
        public string InvoiceImage { get; set; }
        public string GroupName { get; set; }

        readonly string[] applicableGroups = new string[] 
        { 
            "2",   //Check only
            "4",   //Web Decisioning
            "14",  //Single Matching Scanline amount
            "15",  //Single Partial
            "18",  //Multiple Matching Scanline Amount
            "19"   //Multiple Partial
        };

        public bool ValidGroup()
        {
            return applicableGroups.Contains(Group);
        }
    }
}
