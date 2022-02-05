using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace IMGHISTFED
{
    class EndorserDemographic : RFile
    {
        public int BorrowerNumber { get; set; }
        public string SSN { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Value2 { get; set; }
        public string Validity { get; set; }
        public string BorrowerSSN { get; set; }
        public string BorrowerName { get; set; }
    }
}
