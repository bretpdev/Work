CREATE PROCEDURE [dbo].[GetAllUsers]

AS

	SELECT
		L.LoginId,
		L.UserName,
		L.LoginTypeId,
		LT.LoginType, 
		L.Notes
	FROM
		[Login] L
	INNER JOIN [LoginType] LT
		ON LT.LoginTypeId = L.LoginTypeId	
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllUsers] TO [db_executor]
    AS [dbo];

