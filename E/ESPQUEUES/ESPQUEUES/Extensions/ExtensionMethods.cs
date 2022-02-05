using System;
using System.Collections.Generic;
using System.Linq;

namespace ESPQUEUES
{
	static class ExtensionMethods
	{
		private static readonly string[] CONSOL_TYPES = { "SUBCNS", "UNCNS", "SUBSPC", "UNSPC" };

		public static IEnumerable<Loan> ConsolLoans(this IEnumerable<Loan> loans)
		{
			return loans.Where(p => CONSOL_TYPES.Contains(p.Program)).ToList();
		}

		public static bool ContainsUncommentedLoan(this IEnumerable<Loan> loans, string program, DateTime firstDisbursementDate)
		{
			return (loans.Any(p => p.Program == program && p.FirstDisbursementDate == firstDisbursementDate && p.CommentIndicator == Loan.CommentStatus.Needed));
		}

		public static List<int> SequenceNumbersMarkedForComments(this IEnumerable<Loan> loans)
		{
			return loans.LoansMarkedForComments().Select(p => p.Sequence).ToList();
		}

		public static IEnumerable<Loan> LoansMarkedForComments(this IEnumerable<Loan> loans)
		{
			return loans.Where(p => p.CommentIndicator == Loan.CommentStatus.Needed).ToList();
		}

		public static IEnumerable<Loan> LoansMissingEnrollmentInfo(this IEnumerable<Loan> loans)
		{
			return loans.Where(p => string.IsNullOrEmpty(p.OneLinkSchoolCode) || string.IsNullOrEmpty(p.EnrollmentStatus) || p.EnrollmentStatusEffectiveDate == Loan.NoDate || p.ExpectedGraduationDate == Loan.NoDate || p.CertificationDate == Loan.NoDate).ToList();
		}

		public static IEnumerable<Loan> LoansNotMissingEnrollmentInfo(this IEnumerable<Loan> loans)
		{
			return loans.Where(p => !string.IsNullOrEmpty(p.OneLinkSchoolCode) && !string.IsNullOrEmpty(p.EnrollmentStatus) && p.EnrollmentStatusEffectiveDate != Loan.NoDate && p.ExpectedGraduationDate != Loan.NoDate && p.CertificationDate != Loan.NoDate).ToList();
		}

		public static IEnumerable<Loan> NonConsolLoans(this IEnumerable<Loan> loans)
		{
			return loans.Where(p => !CONSOL_TYPES.Contains(p.Program)).ToList();
		}

		public static IEnumerable<Loan> NonSeparatedWglLoans(this IEnumerable<Loan> loans, string separationReason)
		{
			return loans.Where(p => new string[] { "W", "G", "L" }.Contains(p.EnrollmentStatus) && (p.EnrollmentStatusEffectiveDate != p.SeparationDate || p.GetAbbreviatedSeparationReason(separationReason) != p.EnrollmentStatus)).ToList();
		}
	}
}
