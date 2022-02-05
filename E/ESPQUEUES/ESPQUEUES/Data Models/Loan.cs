using System;

namespace ESPQUEUES
{
	class Loan
	{
		public enum CommentStatus
		{
			None,
			Added,
			Needed
		}

		public static readonly DateTime NoDate = DateTime.MinValue;

		public DateTime CertificationDate { get; set; }
		public CommentStatus CommentIndicator { get; set; }
		public string CommonlineId { get; set; }
		public DateTime CompassCertifiedDate { get; set; }
		public DateTime CompassNotifiedDate { get; set; }
		public string CompassSchoolCode { get; set; }
		public string EnrollmentStatus { get; set; }
		public DateTime EnrollmentStatusEffectiveDate { get; set; }
		public DateTime ExpectedGraduationDate { get; set; }
		public DateTime FirstDisbursementDate { get; set; }
		public DateTime? GuarantyDate { get; set; }
		public bool HasCurrentDefermentOrForbearance { get; set; }
		public DateTime LenderNotifiedDate { get; set; }
		public DateTime NewSeparationDate
		{
			get
			{
				switch (EnrollmentStatus)
				{
					case "F":
					case "H":
					case "A":
						return ExpectedGraduationDate;
					case "W":
					case "G":
					case "L":
						return EnrollmentStatusEffectiveDate;
					default:
						return NoDate;
				}
			}
		}
		public string NewSeparationReason
		{
			get
			{
				switch (EnrollmentStatus)
				{
					case "F":
						return "11";
					case "H":
						return "10";
					case "A":
						return "05";
					case "W":
						return "02";
					case "G":
						return "01";
					case "L":
						return "08";
					default:
						return "";
				}
			}
		}
		public string OneLinkSchoolCode { get; set; }
		public string Program { get; set; }
		public DateTime? RepaymentStartDate { get; set; }
		public DateTime SeparationDate { get; set; }
		public string SeparationReason { get; set; }
		public int Sequence { get; set; }
		public bool WasDeferredOrForbeared { get; set; }

		public Loan()
		{
			CertificationDate = NoDate;
			CommentIndicator = CommentStatus.None;
			CommonlineId = "";
			CompassCertifiedDate = NoDate;
			CompassNotifiedDate = NoDate;
			CompassSchoolCode = "";
			EnrollmentStatus = "";
			EnrollmentStatusEffectiveDate = NoDate;
			ExpectedGraduationDate = NoDate;
			FirstDisbursementDate = NoDate;
			GuarantyDate = NoDate;
			HasCurrentDefermentOrForbearance = false;
			LenderNotifiedDate = NoDate;
			OneLinkSchoolCode = "";
			Program = "";
			RepaymentStartDate = NoDate;
			SeparationDate = NoDate;
			SeparationReason = "";
			Sequence = 0;
			WasDeferredOrForbeared = false;
		}

		public bool EnrollmentEquals(Loan other)
		{
			if (EnrollmentStatus != other.EnrollmentStatus) { return false; }
			if (EnrollmentStatusEffectiveDate != other.EnrollmentStatusEffectiveDate) { return false; }
			if (SeparationReason != other.SeparationReason) { return false; }
			if (SeparationDate != other.SeparationDate) { return false; }
			if (ExpectedGraduationDate != other.ExpectedGraduationDate) { return false; }
			if (LenderNotifiedDate != other.LenderNotifiedDate) { return false; }
			if (CompassNotifiedDate != other.CompassNotifiedDate) { return false; }
			if (OneLinkSchoolCode != other.OneLinkSchoolCode) { return false; }
			if (CompassSchoolCode != other.CompassSchoolCode) { return false; }
			if (CertificationDate != other.CertificationDate) { return false; }
			if (CompassCertifiedDate != other.CompassCertifiedDate) { return false; }
			return true;
		}

		public string GetAbbreviatedSeparationReason(string separationReasonFromQueueTask)
		{
			switch (SeparationReason)
			{
				case "EN":
					if (separationReasonFromQueueTask == "11")
						return "F";
					else if (separationReasonFromQueueTask == "10")
						return "H";
					else
						return "";
				case "FT":
				case "HT":
				case "LT":
				case "GR":
				case "WI":
					return SeparationReason.Substring(0, 1);
				case "ON":
					return "A";
				case "NV":
					return "X";
				default:
					return "";
			}
		}
	}
}
