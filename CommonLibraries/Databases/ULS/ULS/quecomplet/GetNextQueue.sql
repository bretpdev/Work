CREATE PROCEDURE [quecomplet].[GetNextQueue]
AS
BEGIN

	BEGIN TRANSACTION

	DECLARE @ERROR INT

	DECLARE @MyTableVar 
		TABLE
		(
			QueueId int NOT NULL
		);

	SELECT @ERROR = @@ERROR

	UPDATE TOP (1)
		ULS.[quecomplet].Queues
	SET
		PickedUpForProcessing = GETDATE()
	OUTPUT inserted.QueueId
	INTO @MyTableVar
	WHERE
		ProcessedAt IS NULL
		AND PickedUpForProcessing IS NULL
	
	SELECT @ERROR = @ERROR + @@ERROR

	SELECT
		Q.QueueId,
		Q.[Queue],
		Q.SubQueue,
		Q.AccountIdentifier,
		Q.TaskControlNumber,
		TS.TaskStatus,
		AR.ActionResponse
	FROM 
		ULS.quecomplet.Queues Q
		INNER JOIN ULS.quecomplet.TaskStatuses TS 
			ON TS.TaskStatusId = Q.TaskStatusId
		INNER JOIN ULS.quecomplet.ActionResponses AR
			ON AR.ActionResponseId = Q.ActionResponseId
		INNER JOIN @MyTableVar V
			ON V.QueueId = Q.QueueId
		
	SELECT @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve dbo.ArcAddProcessing record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END
RETURN 0