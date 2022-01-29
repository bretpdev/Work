
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DocumentProcessing
{
    public class EcorrData
    {
        public enum CorrespondenceFormat
        {
            Standard = 1,
            LargePrint = 2,
            Braille = 3,
            AudioCD = 4
        }

        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string EmailAddress { get; set; }
        public bool ValidEmail { get; set; }
        public bool LetterIndicator { get; set; }
        public bool EbillIndicator { get; set; }
        public bool TaxIndicator { get; set; }
        public CorrespondenceFormat Format { get; set; }
    }
}