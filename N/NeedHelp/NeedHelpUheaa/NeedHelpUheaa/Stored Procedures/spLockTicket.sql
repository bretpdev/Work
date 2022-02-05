CREATE PROCEDURE [dbo].[spLockTicket] 
	@TicketID				BIGINT,
	@SqlUserId				int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO dbo.LST_InUse (Ticket, SqlUserId) VALUES (@TicketID, @SqlUserId)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLockTicket] TO [db_executor]
    AS [dbo];

