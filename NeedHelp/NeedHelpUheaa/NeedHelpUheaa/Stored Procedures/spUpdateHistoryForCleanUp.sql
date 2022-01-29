-- =============================================
-- Author:		Jarom Ryan
-- Create date: 09/30/2013
-- Description:	Will update the history of a given ticket.  Theis is a clean-up to fix duplicating history
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateHistoryForCleanUp]
	-- Add the parameters for the stored procedure here
	@TicketId INT,
	@NewHistory TEXT

	
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE
		[dbo].[DAT_Ticket]
	SET 
		[History] = @NewHistory
	WHERE 
		Ticket = @TicketId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spUpdateHistoryForCleanUp] TO [db_executor]
    AS [dbo];

