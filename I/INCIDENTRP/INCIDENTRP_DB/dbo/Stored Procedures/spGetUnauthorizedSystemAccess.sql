-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_UnauthorizedSystemAccess table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetUnauthorizedSystemAccess]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		SuspiciousEntryInSystemOrNetworkLog,
		SystemAccountDiscrepancy,
		UnauthorizedUseOfUserCredentials,
		UnexplainedNewUserAccount,
		UnusualTimeOfUsage
	FROM DAT_UnauthorizedSystemAccess
	WHERE TicketNumber = @TicketNumber
END