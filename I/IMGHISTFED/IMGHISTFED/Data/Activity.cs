using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace IMGHISTFED
{
	class Activity : RFile
	{
        public int BorrowerCounter { get; set; }
		public string Name { get; set; }
		public string SSN { get; set; }
		public string Arc { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public string Recipient { get; set; }
		public string Description { get; set; }
		public string Comment { get; set; }
		public string UserId { get; set; }
	}
}