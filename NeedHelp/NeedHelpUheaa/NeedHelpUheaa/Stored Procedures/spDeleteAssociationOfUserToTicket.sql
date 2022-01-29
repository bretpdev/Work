CREATE PROCEDURE [dbo].[spDeleteAssociationOfUserToTicket] 
	@TicketID				BIGINT,
	@AssociationType		VARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE dbo.DAT_TicketsAssociatedUserID
    SET SqlUserId = NULL
	WHERE Ticket = @TicketID
		AND [Role] = @AssociationType
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDeleteAssociationOfUserToTicket] TO [db_executor]
    AS [dbo];

