CREATE PROCEDURE [batchesp].[GetAccessQueues]
AS

	SELECT DISTINCT
		[Queue],
		[SubQueue]
	FROM
		batchesp.EspEnrollments
	WHERE
		ProcessedAt IS NULL

RETURN 0
