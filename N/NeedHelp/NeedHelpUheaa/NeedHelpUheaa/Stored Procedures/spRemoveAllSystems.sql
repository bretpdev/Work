
CREATE PROCEDURE [dbo].[spRemoveAllSystems]
	@TicketID			bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM dbo.REF_System WHERE Ticket = @TicketID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spRemoveAllSystems] TO [db_executor]
    AS [dbo];

