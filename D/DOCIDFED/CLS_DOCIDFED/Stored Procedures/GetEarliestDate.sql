CREATE PROCEDURE [docid].[GetEarliestDate]
AS
	SELECT
		MIN(AddedAt)
	FROM
		docid.DocumentsProcessed

RETURN 0