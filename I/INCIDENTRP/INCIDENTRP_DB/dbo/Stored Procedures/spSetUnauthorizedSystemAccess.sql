-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_UnauthorizedSystemAccess table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetUnauthorizedSystemAccess]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@SuspiciousEntryInSystemOrNetworkLog BIT,
	@SystemAccountDiscrepancy BIT,
	@UnauthorizedUseOfUserCredentials BIT,
	@UnexplainedNewUserAccount BIT,
	@UnusualTimeOfUsage BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_UnauthorizedSystemAccess WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_UnauthorizedSystemAccess (
				TicketNumber,
				TicketType,
				SuspiciousEntryInSystemOrNetworkLog,
				SystemAccountDiscrepancy,
				UnauthorizedUseOfUserCredentials,
				UnexplainedNewUserAccount,
				UnusualTimeOfUsage
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@SuspiciousEntryInSystemOrNetworkLog,
				@SystemAccountDiscrepancy,
				@UnauthorizedUseOfUserCredentials,
				@UnexplainedNewUserAccount,
				@UnusualTimeOfUsage
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_UnauthorizedSystemAccess
			SET SuspiciousEntryInSystemOrNetworkLog = @SuspiciousEntryInSystemOrNetworkLog,
				SystemAccountDiscrepancy = @SystemAccountDiscrepancy,
				UnauthorizedUseOfUserCredentials = @UnauthorizedUseOfUserCredentials,
				UnexplainedNewUserAccount = @UnexplainedNewUserAccount,
				UnusualTimeOfUsage = @UnusualTimeOfUsage
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END