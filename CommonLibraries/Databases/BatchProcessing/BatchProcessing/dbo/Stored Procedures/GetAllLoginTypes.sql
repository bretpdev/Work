CREATE PROCEDURE [dbo].[GetAllLoginTypes]
AS
	SELECT 
		LoginTypeId,
		LoginType,
		MaxInUse
	FROM
		LoginType
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllLoginTypes] TO [db_executor]
    AS [dbo];

