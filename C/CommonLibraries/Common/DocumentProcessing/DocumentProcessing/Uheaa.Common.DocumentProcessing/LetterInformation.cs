
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DocumentProcessing
{
    public class LetterInformation
    {
        public string CostCenter { get; set; }
        public bool Duplex { get; set; }
        public Decimal Pages { get; set; }
        public bool SpecialHandling { get; set; }
        public string LetterId { get; set; }
        public string Instructions { get; set; }
    }
}

