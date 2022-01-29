CREATE PROCEDURE [dbo].[MABCGetSqlUsers]
AS
	SELECT
		SqlUserId AS ID,
		WindowsUserName,
		AesUserId AS AesId,
		BusinessUnit,
		[Role]
	FROM
		SYSA_DAT_Users
	WHERE
		[Status] = 'Active'
RETURN 0

GRANT EXECUTE ON MABCGetSqlUsers TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MABCGetSqlUsers] TO [db_executor]
    AS [dbo];

