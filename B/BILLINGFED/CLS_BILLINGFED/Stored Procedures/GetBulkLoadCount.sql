CREATE PROCEDURE billing.[GetBulkLoadCount]
AS
	SELECT COUNT(*) FROM billing._BulkLoad
RETURN 0