CREATE PROCEDURE [dbo].[spCheckForLockOnTicket] 
	@TicketID				BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.LST_InUse WHERE Ticket = @TicketID AND DATEDIFF(MI, Since, GETDATE()) > 5

    SELECT SqlUserId FROM dbo.LST_InUse WHERE Ticket = @TicketID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCheckForLockOnTicket] TO [db_executor]
    AS [dbo];

