-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatBombInfo table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatBombInfo]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Location VARCHAR(100) = NULL,
	@DetonationTime VARCHAR(100) = NULL,
	@Appearance VARCHAR(200) = NULL,
	@WhoPlacedAndWhy VARCHAR(200) = NULL,
	@CallerName VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatBombInfo WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatBombInfo (TicketNumber, TicketType, Location, DetonationTime, Appearance, WhoPlacedAndWhy, CallerName)
			VALUES (@TicketNumber, 'Threat', @Location, @DetonationTime, @Appearance, @WhoPlacedAndWhy, @CallerName)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatBombInfo
			SET Location = @Location,
				DetonationTime = @DetonationTime,
				Appearance = @Appearance,
				WhoPlacedAndWhy = @WhoPlacedAndWhy,
				CallerName = @CallerName
			WHERE TicketNumber = @TicketNumber
		END
END