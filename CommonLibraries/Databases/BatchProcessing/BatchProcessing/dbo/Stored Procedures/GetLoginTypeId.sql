CREATE PROCEDURE [dbo].[GetLoginTypeId]
	@LoginType varchar(150)
AS
	SELECT 
		LoginTypeId
	FROM
		LoginType
	WHERE 
		LoginType = @LoginType
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLoginTypeId] TO [db_executor]
    AS [dbo];

