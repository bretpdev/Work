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
		And DeletedAt IS NULL
		AND DeletedBy IS NULL

	SELECT @ERROR = @ERROR + @@ERROR

	SELECT
		Q.QueueId,
		Q.[Queue],
		Q.SubQueue,
		Q.AccountIdentifier,
		Q.TaskControlNumber,
		Q.ARC,
		TS.TaskStatus,
		AR.ActionResponse,
		WQ10.WC_TYP_NUM_CTL_TSK
	FROM 
		ULS.quecomplet.Queues Q
		INNER JOIN ULS.quecomplet.TaskStatuses TS 
			ON TS.TaskStatusId = Q.TaskStatusId
		INNER JOIN @MyTableVar V
			ON V.QueueId = Q.QueueId
		INNER JOIN UDW..WQ10_TSK_QUE_DFN WQ10
			ON WQ10.WF_QUE = Q.[Queue]
		LEFT JOIN ULS.quecomplet.ActionResponses AR
			ON AR.ActionResponseId = Q.ActionResponseId
	WHERE 
		 Q.DeletedAt IS NULL
		 AND Q.DeletedBy IS NULL
		
	SELECT @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve quecomplet.Queue record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END
RETURN 0