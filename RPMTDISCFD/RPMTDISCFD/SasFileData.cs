using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common.DocumentProcessing;

namespace RPMTDISCFD
{
	public class SasFileData
	{
		public string AccountNumber { get; set; }
		public List<int> LoanSeqs { get; set; }
		public string Header { get; set; }
		public string LineData { get; set; }
		public bool DoEcorr { get; set; }
		public EcorrData EcorrInfo { get; set; }
	}
}
