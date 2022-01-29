-- =============================================
-- Author:		Daren Beattie
-- Create date: September 9, 2011
-- Description:	Retrieves all history records for a ticket from the DATHistory table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetHistory]
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
		UpdateDateTime,
		SqlUserId,
		[Status],
		StatusChangeDescription,
		[UpdateText]
	FROM DATHistory
	WHERE TicketNumber = @TicketNumber
	AND TicketType = @TicketType
	ORDER BY UpdateDateTime DESC
END