-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatInfo table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatInfo]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Wording TEXT = NULL,
	@Nature TEXT = NULL,
	@Remarks TEXT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatInfo WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatInfo (TicketNumber, TicketType, WordingOfThreat, NatureOfCall, AdditionalRemarks)
			VALUES (@TicketNumber, 'Threat', @Wording, @Nature, @Remarks)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatInfo
			SET WordingOfThreat = @Wording,
				NatureOfCall = @Nature,
				AdditionalRemarks = @Remarks
			WHERE TicketNumber = @TicketNumber
		END
END