-- =============================================
-- Author:		Daren Beattie
-- Create date: September 8, 2011
-- Description:	Retrieves a record from the DAT_ThreatDialect table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetThreatDialect]
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
		English,
		RegionalAmerican,
		RegionalAmericanDescription,
		ForeignAccent,
		ForeignAccentDescription
	FROM DAT_ThreatDialect
	WHERE TicketNumber = @TicketNumber
	AND TicketType = @TicketType
END