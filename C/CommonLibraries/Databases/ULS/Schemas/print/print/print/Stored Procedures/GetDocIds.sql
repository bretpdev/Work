CREATE PROCEDURE [print].[GetDocIds]

AS
	SELECT DISTINCT	
		DocIdName
	FROM	
		[print].DocIds
RETURN 0
