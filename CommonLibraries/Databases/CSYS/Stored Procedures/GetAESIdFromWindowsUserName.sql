CREATE PROCEDURE [dbo].[GetAESIdFromWindowsUserName]
	@WindowsUserId varchar(50)
AS
	SELECT 
		AesUserId
	FROM
		SYSA_DAT_Users
	WHERE
		WindowsUserName = @WindowsUserId
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAESIdFromWindowsUserName] TO [db_executor]
    AS [dbo];


GO

