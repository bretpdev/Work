using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace IMGHISTFED
{
	class Deferment : RFile
	{
		public string SSN { get; set; }
		public string Name { get; set; }
		public string LoanProgram { get; set; }
        public string DisbursementDate { get; set; }
		public string Type { get; set; }
		public string Begin { get; set; }
		public string End { get; set; }

        protected override void AfterParse(List<string> fields)
        {
            DateTime? date = DisbursementDate.ToDateNullable();
            if (date.HasValue)
                DisbursementDate = date.Value.ToShortDateString();
            else
                DisbursementDate = null;
        }
	}
}
