-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates or updates a record in the DAT_Incident table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetIncident]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Cause VARCHAR(50),
	@BorrowerSsnAndDobAreVerified BIT,
	@Priority VARCHAR(10),
	@Location VARCHAR(100) = NULL,
	@Narrative TEXT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_Incident WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_Incident (
				TicketNumber,
				TicketType,
				Cause,
				BorrowerSsnAndDobAreVerified,
				Priority,
				Location,
				Narrative
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@Cause,
				@BorrowerSsnAndDobAreVerified,
				@Priority,
				@Location,
				@Narrative
			)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_Incident
			SET Cause = @Cause,
				BorrowerSsnAndDobAreVerified = @BorrowerSsnAndDobAreVerified,
				Priority = @Priority,
				Location = @Location,
				Narrative = @Narrative
			WHERE TicketNumber = @TicketNumber
		END
END