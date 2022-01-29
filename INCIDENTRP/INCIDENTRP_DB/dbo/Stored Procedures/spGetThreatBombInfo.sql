-- =============================================
-- Author:		Daren Beattie
-- Create date: September 8, 2011
-- Description:	Retrieves a record from the DAT_ThreatBombInfo table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetThreatBombInfo]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		Location,
		DetonationTime,
		Appearance,
		WhoPlacedAndWhy,
		CallerName
	FROM DAT_ThreatBombInfo
	WHERE TicketNumber = @TicketNumber
	AND TicketType = @TicketType
END