CREATE PROCEDURE [Noble].[GetGroups]

AS
BEGIN

	SELECT G.GroupName 
	FROM Noble.Groups G 
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[GetGroups] TO [db_executor]
    AS [dbo];

