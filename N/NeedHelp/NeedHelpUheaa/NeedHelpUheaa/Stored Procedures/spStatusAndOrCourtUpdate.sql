CREATE PROCEDURE [dbo].[spStatusAndOrCourtUpdate] 

	@Ticket     BIGINT,
	@Status		VARCHAR(50),
	@Agent		INT

AS
BEGIN
	SET NOCOUNT ON;

	/* end last status */
	UPDATE REF_Status
	SET EndDate = GETDATE()
	WHERE Ticket = @Ticket 
		AND EndDate = ''

	/* begin new status */
	INSERT INTO REF_Status (
		Ticket,
		[Status],
		Court
	) 
	VALUES (
		@Ticket,
		@Status,
		@Agent
	)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spStatusAndOrCourtUpdate] TO [db_executor]
    AS [dbo];

