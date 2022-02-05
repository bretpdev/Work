-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_BorrowerDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetBorrowerDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Name VARCHAR(100) = NULL,
	@AccountNumber VARCHAR(10) = NULL,
	@State CHAR(2) = NULL,
	@DataRegion VARCHAR(15) = '',
	@BorrowerInformationIsVerified BIT,
	@NotifierKnowsPiiOwner BIT,
	@NotifierRelationshipToPiiOwner VARCHAR(50) = NULL,
	@AddressWasReleased BIT,
	@BankAccountNumbersWereReleased BIT,
	@CreditReportOrScoreWasReleased BIT,
	@DateOfBirthWasReleased BIT,
	@EmployeeIdNumberWasReleased BIT,
	@EmployerIdNumberWasReleased BIT,
	@LoanAmountsOrBalancesWereReleased BIT,
	@LoanApplicationsWereReleased BIT,
	@LoanIdsOrNumbersWereReleased BIT,
	@LoanPaymentHistoriesWereReleased BIT,
	@MedicalOrConditionalDisabilityWasReleased BIT,
	@PayoffAmountsWereReleased BIT,
	@PhoneNumberWasReleased BIT,
	@PromissoryNotesWereReleased BIT,
	@SocialSecurityNumbersWereReleased BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_BorrowerDataInvolved WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_BorrowerDataInvolved (
				TicketNumber,
				TicketType,
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
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@Name,
				@AccountNumber,
				@State,
				@DataRegion,
				@BorrowerInformationIsVerified,
				@NotifierKnowsPiiOwner,
				@NotifierRelationshipToPiiOwner,
				@AddressWasReleased,
				@BankAccountNumbersWereReleased,
				@CreditReportOrScoreWasReleased,
				@DateOfBirthWasReleased,
				@EmployeeIdNumberWasReleased,
				@EmployerIdNumberWasReleased,
				@LoanAmountsOrBalancesWereReleased,
				@LoanApplicationsWereReleased,
				@LoanIdsOrNumbersWereReleased,
				@LoanPaymentHistoriesWereReleased,
				@MedicalOrConditionalDisabilityWasReleased,
				@PayoffAmountsWereReleased,
				@PhoneNumberWasReleased,
				@PromissoryNotesWereReleased,
				@SocialSecurityNumbersWereReleased
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_BorrowerDataInvolved
			SET [Name] = @Name,
				AccountNumber = @AccountNumber,
				[State] = @State,
				DataRegion = @DataRegion,
				BorrowerInformationIsVerified = @BorrowerInformationIsVerified,
				NotifierKnowsPiiOwner = @NotifierKnowsPiiOwner,
				NotifierRelationshipToPiiOwner = @NotifierRelationshipToPiiOwner,
				AddressWasReleased = @AddressWasReleased,
				BankAccountNumbersWereReleased = @BankAccountNumbersWereReleased,
				CreditReportOrScoreWasReleased = @CreditReportOrScoreWasReleased,
				DateOfBirthWasReleased = @DateOfBirthWasReleased,
				EmployeeIdNumberWasReleased = @EmployeeIdNumberWasReleased,
				EmployerIdNumberWasReleased = @EmployerIdNumberWasReleased,
				LoanAmountsOrBalancesWereReleased = @LoanAmountsOrBalancesWereReleased,
				LoanApplicationsWereReleased = @LoanApplicationsWereReleased,
				LoanIdsOrNumbersWereReleased = @LoanIdsOrNumbersWereReleased,
				LoanPaymentHistoriesWereReleased = @LoanPaymentHistoriesWereReleased,
				MedicalOrConditionalDisabilityWasReleased = @MedicalOrConditionalDisabilityWasReleased,
				PayoffAmountsWereReleased = @PayoffAmountsWereReleased,
				PhoneNumberWasReleased = @PhoneNumberWasReleased,
				PromissoryNotesWereReleased = @PromissoryNotesWereReleased,
				SocialSecurityNumbersWereReleased = @SocialSecurityNumbersWereReleased
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END