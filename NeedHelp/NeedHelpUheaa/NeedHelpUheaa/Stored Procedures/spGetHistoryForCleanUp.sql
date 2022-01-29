-- =============================================
-- Author:		Jarom Ryan
-- Create date: 09/30/2013	
-- Description:	Will get the history for a specified ticket.  This sp will be used for a clean up of need help
-- =============================================
CREATE PROCEDURE [dbo].[spGetHistoryForCleanUp]
	
	@TicketId int

AS
BEGIN

	SET NOCOUNT ON;

    SELECT [History]
	FROM [dbo].[DAT_Ticket]
	WHERE [Ticket] = @TicketId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetHistoryForCleanUp] TO [db_executor]
    AS [dbo];

