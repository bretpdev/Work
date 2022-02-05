CREATE PROCEDURE [emailbatch].[GetBulkLoadCount]

AS
	SELECT 
		COUNT(*) 
	FROM 
		[emailbatch]._BulkLoad
RETURN 0
