-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_AgencyDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetAgencyDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		AccountingOrAdministrativeRecordsWereReleased,
		ClosedSchoolRecordsWereReleased,
		ConfidentialCaseFilesWereReleased,
		ContractInformationWasReleased,
		OperationsReportsWereReleased,
		ProposalAndLoanPurchaseRequestsWereReleased,
		UespParticipantRecordsWereReleased,
		OtherInformationWasReleased,
		OtherInformation
	FROM DAT_AgencyDataInvolved
	WHERE TicketNumber = @TicketNumber
END