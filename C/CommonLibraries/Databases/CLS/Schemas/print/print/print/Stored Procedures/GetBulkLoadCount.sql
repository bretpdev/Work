CREATE PROCEDURE [print].[GetBulkLoadCount]
AS
	SELECT COUNT(*) FROM [print]._BulkLoad
RETURN 0