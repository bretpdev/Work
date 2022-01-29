CREATE PROCEDURE [dbo].[GetDocIds]

AS
	SELECT DISTINCT	
		DocIdName
	FROM	
		[print].DocIds
RETURN 0