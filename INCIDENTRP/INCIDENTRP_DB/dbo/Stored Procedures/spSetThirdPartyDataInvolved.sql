-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_ThirdPartyDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThirdPartyDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Name VARCHAR(100) = NULL,
	@AccountNumber VARCHAR(10) = NULL,
	@State CHAR(2) = NULL,
	@DataRegion VARCHAR(15),
	@NotifierKnowsPiiOwner BIT,
	@NotifierRelationshipToPiiOwner VARCHAR(50) = NULL,
	@SocialSecurityNumbersWereReleased BIT,
	@LoanIdsOrNumbersWereReleased BIT,
	@LoanAmountsOrBalancesWereReleased BIT,
	@LoanPaymentHistoriesWereReleased BIT,
	@PayoffAmountsWereReleased BIT,
	@BankAccountNumbersWereReleased BIT,
	@DateOfBirthWasReleased BIT,
	@MedicalOrConditionalDisabilityWasReleased BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThirdPartyDataInvolved WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_ThirdPartyDataInvolved (
				TicketNumber,
				TicketType,
				[Name],
				AccountNumber,
				[State],
				DataRegion,
				NotifierKnowsPiiOwner,
				NotifierRelationshipToPiiOwner,
				SocialSecurityNumbersWereReleased,
				LoanIdsOrNumbersWereReleased,
				LoanAmountsOrBalancesWereReleased,
				LoanPaymentHistoriesWereReleased,
				PayoffAmountsWereReleased,
				BankAccountNumbersWereReleased,
				DateOfBirthWasReleased,
				MedicalOrConditionalDisabilityWasReleased
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@Name,
				@AccountNumber,
				@State,
				@DataRegion,
				@NotifierKnowsPiiOwner,
				@NotifierRelationshipToPiiOwner,
				@SocialSecurityNumbersWereReleased,
				@LoanIdsOrNumbersWereReleased,
				@LoanAmountsOrBalancesWereReleased,
				@LoanPaymentHistoriesWereReleased,
				@PayoffAmountsWereReleased,
				@BankAccountNumbersWereReleased,
				@DateOfBirthWasReleased,
				@MedicalOrConditionalDisabilityWasReleased
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_ThirdPartyDataInvolved
			SET [Name] = @Name,
				AccountNumber = @AccountNumber,
				[State] = @State,
				DataRegion = @DataRegion,
				NotifierKnowsPiiOwner = @NotifierKnowsPiiOwner,
				NotifierRelationshipToPiiOwner = @NotifierRelationshipToPiiOwner,
				SocialSecurityNumbersWereReleased = @SocialSecurityNumbersWereReleased,
				LoanIdsOrNumbersWereReleased = @LoanIdsOrNumbersWereReleased,
				LoanAmountsOrBalancesWereReleased = @LoanAmountsOrBalancesWereReleased,
				LoanPaymentHistoriesWereReleased = @LoanPaymentHistoriesWereReleased,
				PayoffAmountsWereReleased = @PayoffAmountsWereReleased,
				BankAccountNumbersWereReleased = @BankAccountNumbersWereReleased,
				DateOfBirthWasReleased = @DateOfBirthWasReleased,
				MedicalOrConditionalDisabilityWasReleased = @MedicalOrConditionalDisabilityWasReleased
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END