CREATE PROCEDURE [i1i2schltr].[MarkQueueTaskDataProcessed]
	@QueueTaskDataId INT
AS
	UPDATE
		i1i2schltr.QueueTaskData
	SET
		ProcessedAt = GETDATE()
	WHERE
		QueueTaskDataId = @QueueTaskDataId
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0
