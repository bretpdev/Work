CREATE PROCEDURE [dbo].[spRemoveInactiveUser]
	@TicketID			bigint,
	@SqlUserId			int
AS
BEGIN 

	SET NOCOUNT ON;

	DELETE FROM dbo.REF_EMailRecipient
	WHERE Ticket = @TicketID AND SqlUserId = @SqlUserId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spRemoveInactiveUser] TO [db_executor]
    AS [dbo];

