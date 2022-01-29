CREATE PROCEDURE [dbo].[spCreateNewTicket]
	@FlowID			VARCHAR(50),
	@Requester		INT,
	@BU				INT,
	@Status			VARCHAR(50)
AS

	SET NOCOUNT ON;

	DECLARE @TicketNumber	bigint

	INSERT INTO dbo.DAT_Ticket (
		TicketCode,
		Unit,
		[Status]
	)
	VALUES (
		@FlowID,
		@BU,
		@Status
	)

	SET @TicketNumber = @@IDENTITY

	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'Requester', @Requester)
	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'Court', @Requester)
	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'AssignedTo', null)
	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'PreviousCourt', null)

	SELECT CAST(@TicketNumber AS BIGINT) as TicketNumber
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCreateNewTicket] TO [db_executor]
    AS [dbo];

