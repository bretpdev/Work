-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Removes a record from the DAT_ThreatBombInfo table.
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteThreatBombInfo]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM DAT_ThreatBombInfo
	WHERE TicketNumber = @TicketNumber
END