CREATE PROCEDURE [olqtskbldr].[GetUnprocessedQueues]
AS

	UPDATE
		[olqtskbldr].Queues
	SET 
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	WHERE
		DeletedAt IS NULL
		AND ProcessingAttempts > 2
		AND ProcessedAt IS NULL

	SELECT
		QueueId,
		TargetId,
		QueueName,
		InstitutionId,
		InstitutionType,
		DateDue,
		TimeDue,
		Comment,
		SourceFilename AS [Filename]
	FROM
		[olqtskbldr].Queues
	WHERE
		DeletedAt IS NULL
		AND DeletedBy IS NULL
		AND ProcessedAt IS NULL
RETURN 0
