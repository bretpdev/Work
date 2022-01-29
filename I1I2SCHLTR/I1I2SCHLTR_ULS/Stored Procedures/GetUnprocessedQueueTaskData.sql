CREATE PROCEDURE [i1i2schltr].[GetUnprocessedQueueTaskData]
AS
	UPDATE QTD
	SET 
		ProcessingAttempts = ProcessingAttempts + 1,
		DeletedAt = CASE WHEN ProcessingAttempts > 3 THEN GETDATE() ELSE NULL END,
		DeletedBy = CASE WHEN ProcessingAttempts > 3 THEN SUSER_NAME() ELSE NULL END
	FROM
		i1i2schltr.QueueTaskData QTD
	WHERE
		QTD.ProcessedAt IS NULL
		AND QTD.DeletedAt IS NULL
		AND QTD.DeletedBy IS NULL

	--This maps to the QueueTaskData object in c#
	SELECT
		QTD.QueueTaskDataId,
		QTD.SSN,
		QTD.[Queue],
		QTD.RunDateId
	FROM
		i1i2schltr.QueueTaskData QTD
	WHERE
		QTD.ProcessedAt IS NULL
		AND QTD.DeletedAt IS NULL
		AND QTD.DeletedBy IS NULL
RETURN 0
