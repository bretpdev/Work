using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace INCIDENTRP
{
	partial class DataAccess
	{
		[UsesSproc(IncidentReportingUheaa, "spDeleteAgencyDataInvolved")]
		public void DeleteAgencyDataInvolved(long ticketNumber)
		{
			LDA.Execute("spDeleteAgencyDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteAgencyEmployeeHrDataInvolved")]
		public void DeleteAgencyEmployeeHrDataInvolved(long ticketNumber)
		{
			LDA.Execute("spDeleteAgencyEmployeeHrDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteBorrowerDataInvolved")]
		public void DeleteBorrowerDataInvolved(long ticketNumber)
		{
			LDA.Execute("spDeleteBorrowerDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteThirdPartyDataInvolved")]
		public void DeleteThirdPartyDataInvolved(long ticketNumber)
		{
			LDA.Execute("spDeleteThirdPartyDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spGetAgencyDataInvolved")]
		public AgencyDataInvolved LoadAgencyDataInvolved(long ticketNumber)
		{
			return LDA.ExecuteList<AgencyDataInvolved>("spGetAgencyDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetAgencyEmployeeHrDataInvolved")]
		public AgencyEmployeeHrDataInvolved LoadAgencyEmployeeHrDataInvolved(long ticketNumber)
		{
			return LDA.ExecuteList<AgencyEmployeeHrDataInvolved>("spGetAgencyEmployeeHrDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetBorrowerDataInvolved")]
		public BorrowerDataInvolved LoadBorrowerDataInvolved(long ticketNumber)
		{
			return LDA.ExecuteList<BorrowerDataInvolved>("spGetBorrowerDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThirdPartyDataInvolved")]
		public ThirdPartyDataInvolved LoadThirdPartyDataInvolved(long ticketNumber)
		{
			return LDA.ExecuteList<ThirdPartyDataInvolved>("spGetThirdPartyDataInvolved", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spSetAgencyDataInvolved")]
		public void SaveAgencyDataInvolved(AgencyDataInvolved agency, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("AccountingOrAdministrativeRecordsWereReleased", agency.AccountingOrAdministrativeRecordsWereReleased));
			parms.Add(new SqlParameter("ClosedSchoolRecordsWereReleased", agency.ClosedSchoolRecordsWereReleased));
			parms.Add(new SqlParameter("ConfidentialCaseFilesWereReleased", agency.ConfidentialCaseFilesWereReleased));
			parms.Add(new SqlParameter("ContractInformationWasReleased", agency.ContractInformationWasReleased));
			parms.Add(new SqlParameter("OperationsReportsWereReleased", agency.OperationsReportsWereReleased));
			parms.Add(new SqlParameter("ProposalAndLoanPurchaseRequestsWereReleased", agency.ProposalAndLoanPurchaseRequestsWereReleased));
			parms.Add(new SqlParameter("UespParticipantRecordsWereReleased", agency.UespParticipantRecordsWereReleased));
			parms.Add(new SqlParameter("OtherInformationWasReleased", agency.OtherInformationWasReleased));
			parms.Add(new SqlParameter("OtherInformation", agency.OtherInformation));
			LDA.Execute("spSetAgencyDataInvolved", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetAgencyEmployeeHrDataInvolved")]
		public void SaveAgencyEmployeeHrDataInvolved(AgencyEmployeeHrDataInvolved employee, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Name", employee.Name));
			parms.Add(new SqlParameter("State", employee.State));
			parms.Add(new SqlParameter("NotifierKnowsEmployee", employee.NotifierKnowsEmployee));
			parms.Add(new SqlParameter("NotifierRelationshipToEmployee", employee.NotifierRelationshipToEmployee));
			parms.Add(new SqlParameter("DateOfBirthWasReleased", employee.DateOfBirthWasReleased));
			parms.Add(new SqlParameter("EmployeeIdNumberWasReleased", employee.EmployeeIdNumberWasReleased));
			parms.Add(new SqlParameter("HomeAddressWasReleased", employee.HomeAddressWasReleased));
			parms.Add(new SqlParameter("HealthInformationWasReleased", employee.HealthInformationWasReleased));
			parms.Add(new SqlParameter("PerformanceInformationWasReleased", employee.PerformanceInformationWasReleased));
			parms.Add(new SqlParameter("PersonnelFilesWereReleased", employee.PersonnelFilesWereReleased));
			parms.Add(new SqlParameter("UnauthorizedReferenceWasReleased", employee.UnauthorizedReferenceWasReleased));
			LDA.Execute("spSetAgencyEmployeeHrDataInvolved", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetBorrowerDataInvolved")]
		public void SaveBorrowerDataInvolved(BorrowerDataInvolved borrower, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Name", borrower.Name));
			parms.Add(new SqlParameter("AccountNumber", borrower.AccountNumber));
			parms.Add(new SqlParameter("State", borrower.State));
			parms.Add(new SqlParameter("DataRegion", borrower.DataRegion));
			parms.Add(new SqlParameter("BorrowerInformationIsVerified", borrower.BorrowerInformationIsVerified));
			parms.Add(new SqlParameter("NotifierKnowsPiiOwner", borrower.NotifierKnowsPiiOwner));
			parms.Add(new SqlParameter("NotifierRelationshipToPiiOwner", borrower.NotifierRelationshipToPiiOwner));
			parms.Add(new SqlParameter("AddressWasReleased", borrower.AddressWasReleased));
			parms.Add(new SqlParameter("BankAccountNumbersWereReleased", borrower.BankAccountNumbersWereReleased));
			parms.Add(new SqlParameter("CreditReportOrScoreWasReleased", borrower.CreditReportOrScoreWasReleased));
			parms.Add(new SqlParameter("DateOfBirthWasReleased", borrower.DateOfBirthWasReleased));
			parms.Add(new SqlParameter("EmployeeIdNumberWasReleased", borrower.EmployeeIdNumberWasReleased));
			parms.Add(new SqlParameter("EmployerIdNumberWasReleased", borrower.EmployerIdNumberWasReleased));
			parms.Add(new SqlParameter("LoanAmountsOrBalancesWereReleased", borrower.LoanAmountsOrBalancesWereReleased));
			parms.Add(new SqlParameter("LoanApplicationsWereReleased", borrower.LoanApplicationsWereReleased));
			parms.Add(new SqlParameter("LoanIdsOrNumbersWereReleased", borrower.LoanIdsOrNumbersWereReleased));
			parms.Add(new SqlParameter("LoanPaymentHistoriesWereReleased", borrower.LoanPaymentHistoriesWereReleased));
			parms.Add(new SqlParameter("MedicalOrConditionalDisabilityWasReleased", borrower.MedicalOrConditionalDisabilityWasReleased));
			parms.Add(new SqlParameter("PayoffAmountsWereReleased", borrower.PayoffAmountsWereReleased));
			parms.Add(new SqlParameter("PhoneNumberWasReleased", borrower.PhoneNumberWasReleased));
			parms.Add(new SqlParameter("PromissoryNotesWereReleased", borrower.PromissoryNotesWereReleased));
			parms.Add(new SqlParameter("SocialSecurityNumbersWereReleased", borrower.SocialSecurityNumbersWereReleased));
			LDA.Execute("spSetBorrowerDataInvolved", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThirdPartyDataInvolved")]
		public void SaveThirdPartyDataInvolved(ThirdPartyDataInvolved party, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Name", party.Name));
			parms.Add(new SqlParameter("AccountNumber", party.AccountNumber));
			parms.Add(new SqlParameter("State", party.State));
			parms.Add(new SqlParameter("DataRegion", party.DataRegion));
			parms.Add(new SqlParameter("NotifierKnowsPiiOwner", party.NotifierKnowsPiiOwner));
			parms.Add(new SqlParameter("NotifierRelationshipToPiiOwner", party.NotifierRelationshipToPiiOwner));
			parms.Add(new SqlParameter("SocialSecurityNumbersWereReleased", party.SocialSecurityNumbersWereReleased));
			parms.Add(new SqlParameter("LoanIdsOrNumbersWereReleased", party.LoanIdsOrNumbersWereReleased));
			parms.Add(new SqlParameter("LoanAmountsOrBalancesWereReleased", party.LoanAmountsOrBalancesWereReleased));
			parms.Add(new SqlParameter("LoanPaymentHistoriesWereReleased", party.LoanPaymentHistoriesWereReleased));
			parms.Add(new SqlParameter("PayoffAmountsWereReleased", party.PayoffAmountsWereReleased));
			parms.Add(new SqlParameter("BankAccountNumbersWereReleased", party.BankAccountNumbersWereReleased));
			parms.Add(new SqlParameter("DateOfBirthWasReleased", party.DateOfBirthWasReleased));
			parms.Add(new SqlParameter("MedicalOrConditionalDisabilityWasReleased", party.MedicalOrConditionalDisabilityWasReleased));
			LDA.Execute("spSetThirdPartyDataInvolved", IncidentReportingUheaa, parms.ToArray());
		}
	}
}