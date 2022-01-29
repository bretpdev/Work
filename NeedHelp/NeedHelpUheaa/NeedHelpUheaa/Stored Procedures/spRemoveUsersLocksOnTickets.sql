CREATE PROCEDURE [dbo].[spRemoveUsersLocksOnTickets]
	@SqlUserId			int
AS
BEGIN 

	SET NOCOUNT ON;

	DELETE FROM dbo.LST_InUse WHERE SqlUserId = @SqlUserId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spRemoveUsersLocksOnTickets] TO [db_executor]
    AS [dbo];

