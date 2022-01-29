CREATE PROCEDURE [dbo].[spAddRecipients]
	@TicketID			bigint,
	@SqlUserId			int
AS
BEGIN 

	SET NOCOUNT ON;

	INSERT INTO dbo.REF_EMailRecipient
	VALUES (@TicketID, @SqlUserId)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddRecipients] TO [db_executor]
    AS [dbo];

