CREATE PROCEDURE [dbo].[spGetSystemsForTicket]
	@TicketID			BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT [System]
	FROM dbo.REF_System
	WHERE Ticket = @TicketID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetSystemsForTicket] TO [db_executor]
    AS [dbo];

