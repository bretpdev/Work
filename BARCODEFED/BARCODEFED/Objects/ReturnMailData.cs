using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARCODEFED
{
    class ReturnMailData
    {
        public DateTime CreateDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Letter { get; set; }
        public string AccountIdentifier { get; set; }
        public string SelectedAccountIdentifier { get; set; }

    }
}
