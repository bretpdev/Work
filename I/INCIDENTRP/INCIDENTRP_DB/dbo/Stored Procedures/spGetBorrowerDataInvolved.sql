-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_BorrowerDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetBorrowerDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		[Name],
		AccountNumber,
		[State],
		DataRegion,
		BorrowerInformationIsVerified,
		NotifierKnowsPiiOwner,
		NotifierRelationshipToPiiOwner,
		AddressWasReleased,
		BankAccountNumbersWereReleased,
		CreditReportOrScoreWasReleased,
		DateOfBirthWasReleased,
		EmployeeIdNumberWasReleased,
		EmployerIdNumberWasReleased,
		LoanAmountsOrBalancesWereReleased,
		LoanApplicationsWereReleased,
		LoanIdsOrNumbersWereReleased,
		LoanPaymentHistoriesWereReleased,
		MedicalOrConditionalDisabilityWasReleased,
		PayoffAmountsWereReleased,
		PhoneNumberWasReleased,
		PromissoryNotesWereReleased,
		SocialSecurityNumbersWereReleased
	FROM DAT_BorrowerDataInvolved
	WHERE TicketNumber = @TicketNumber
END