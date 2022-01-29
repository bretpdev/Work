CREATE PROCEDURE [dbo].[spSaveAssociatedUserToTicket] 
	@TicketID				BIGINT,
	@SqlUserId				INT,
	@AssociationType		VARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;

    IF (SELECT COUNT(*) FROM dbo.DAT_TicketsAssociatedUserID WHERE Ticket = @TicketID AND [Role] = @AssociationType) > 0 
		BEGIN
			UPDATE dbo.DAT_TicketsAssociatedUserID
			SET SqlUserId = @SqlUserId
			WHERE Ticket = @TicketID
				AND [Role] = @AssociationType
		END
	ELSE
		BEGIN
			INSERT INTO dbo.DAT_TicketsAssociatedUserID (
				SqlUserId,
				Ticket,
				[Role]
			)
			VALUES (
				@SqlUserId,
				@TicketID,
				@AssociationType
			)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSaveAssociatedUserToTicket] TO [db_executor]
    AS [dbo];

