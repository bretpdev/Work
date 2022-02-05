CREATE PROCEDURE [dbo].[GetUsersRole]
	@WindowsUserId varchar(50)
AS
	
	SELECT DISTINCT
		[Role]
	FROM
		[dbo].[SYSA_DAT_Users] 
	WHERE
		WindowsUserName = @WindowsUserId
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersRole] TO [db_executor]
    AS [dbo];

