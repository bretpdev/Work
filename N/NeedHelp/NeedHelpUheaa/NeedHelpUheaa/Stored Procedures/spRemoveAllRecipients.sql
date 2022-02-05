CREATE PROCEDURE [dbo].[spRemoveAllRecipients]
	@TicketID			bigint
AS
BEGIN 

	SET NOCOUNT ON;

	DELETE FROM dbo.REF_EMailRecipient
	WHERE Ticket = @TicketID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spRemoveAllRecipients] TO [db_executor]
    AS [dbo];

