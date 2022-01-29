-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_ThirdPartyDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetThirdPartyDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		Name,
		AccountNumber,
		State,
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
	FROM DAT_ThirdPartyDataInvolved
	WHERE TicketNumber = @TicketNumber
END