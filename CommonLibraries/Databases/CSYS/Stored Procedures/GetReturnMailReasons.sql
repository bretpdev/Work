CREATE PROCEDURE [dbo].[GetReturnMailReasons]
	
AS
	SELECT 
		ReturnMailReason
	FROM
		ReturnMailReasons
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReturnMailReasons] TO [db_executor]
    AS [dbo];

