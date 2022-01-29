CREATE PROCEDURE [dbo].[spGetLockedTicketUser]
	@TicketID		bigint
AS
BEGIN
	SET NOCOUNT ON;

    SELECT a.FirstName + ' ' + a.LastName
	from CSYS.dbo.SYSA_DAT_Users a
	join dbo.LST_InUse b
	on a.SqlUserId = b.SqlUserId
	where b.Ticket = @TicketID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetLockedTicketUser] TO [db_executor]
    AS [dbo];

