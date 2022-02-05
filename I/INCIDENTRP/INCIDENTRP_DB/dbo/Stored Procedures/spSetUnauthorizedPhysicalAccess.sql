-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_UnauthorizedPhysicalAccess table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetUnauthorizedPhysicalAccess]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@AccessAccountingDiscrepancy BIT,
	@BuildingBreakIn BIT,
	@Piggybacking BIT,
	@SuspiciousEntryInAccessLog BIT,
	@SuspiciousEntryInVideoLog BIT,
	@UnauthorizedUseOfKeycard BIT,
	@UnexplainedNewKeycard BIT,
	@UnusualTimeOfUsage BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_UnauthorizedPhysicalAccess WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_UnauthorizedPhysicalAccess (
				TicketNumber,
				TicketType,
				AccessAccountingDiscrepancy,
				BuildingBreakIn,
				Piggybacking,
				SuspiciousEntryInAccessLog,
				SuspiciousEntryInVideoLog,
				UnauthorizedUseOfKeycard,
				UnexplainedNewKeycard,
				UnusualTimeOfUsage
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@AccessAccountingDiscrepancy,
				@BuildingBreakIn,
				@Piggybacking,
				@SuspiciousEntryInAccessLog,
				@SuspiciousEntryInVideoLog,
				@UnauthorizedUseOfKeycard,
				@UnexplainedNewKeycard,
				@UnusualTimeOfUsage
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_UnauthorizedPhysicalAccess
			SET AccessAccountingDiscrepancy = @AccessAccountingDiscrepancy,
				BuildingBreakIn = @BuildingBreakIn,
				Piggybacking = @Piggybacking,
				SuspiciousEntryInAccessLog = @SuspiciousEntryInAccessLog,
				SuspiciousEntryInVideoLog = @SuspiciousEntryInVideoLog,
				UnauthorizedUseOfKeycard = @UnauthorizedUseOfKeycard,
				UnexplainedNewKeycard = @UnexplainedNewKeycard,
				UnusualTimeOfUsage = @UnusualTimeOfUsage
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END