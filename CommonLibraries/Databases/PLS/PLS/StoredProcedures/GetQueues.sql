CREATE PROCEDURE [crpqassign].[GetQueues]

AS
	SELECT
		Q.[Queue],
		Q.SubQueue,
		Q.Arc
	FROM
		crpqassign.Queues Q
RETURN 0
