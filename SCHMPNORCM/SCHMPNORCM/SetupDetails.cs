using System;

namespace SCHMPNORCM
{
	class SetupDetails
	{
		public enum LoanProgram
		{
			Stafford,
			Plus
		}

		public enum Process
		{
			UpdateProductionRegion,
			UpdateTestRegion,
			SaveInformation,
			DeleteTextFile,
			GenerateEmailLoanDelivery
		}

		public enum SetupType
		{
			SerialMpn,
			Commonline,
			Clearinghouse,
			Nslds
		}

		public string SchoolCode { get; set; }
		public DateTime EffectiveDate { get; set; }
		public SetupType SchoolSetTo { get; set; }
		public LoanProgram Program { get; set; }
		public bool CommonlineApplication { get; set; }
		public bool CommonlineChange { get; set; }
		public bool CommonlineDisbursementRoster { get; set; }
		public bool ModificationResponse { get; set; }
		public bool HoldAllDisbursements { get; set; }
		public bool ServiceBureauParticipant { get; set; }
		public bool Elmres { get; set; }
		public Process ProcessingOption { get; set; }
	}//class
}//namespace
