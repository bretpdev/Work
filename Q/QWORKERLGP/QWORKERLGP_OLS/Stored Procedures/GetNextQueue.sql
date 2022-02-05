CREATE PROCEDURE [qworkerlgp].[GetNextQueue]
AS
	BEGIN TRANSACTION

	DECLARE @ERROR INT
	DECLARE @QueueTable TABLE
	(
		QueueId INT NOT NULL
	);

	UPDATE TOP (1)
		OLS.qworkerlgp.Queues
	SET
		PickedUpForProcessing = GETDATE()
	OUTPUT
		inserted.QueueId
	INTO
		@QueueTable
	WHERE
		ProcessedAt IS NULL
		AND PickedUpForProcessing IS NULl
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL

	SELECT @ERROR = @@ERROR

	SELECT
		Q.QueueId,
		Q.Ssn,
		Q.Department,
		Q.WorkGroupId,
		Q.ActionCode,
		Q.ActivityType,
		Q.ActivityContactType,
		Q.TaskComment
	FROM
		OLS.qworkerlgp.Queues Q
		INNER JOIN @QueueTable QT
			ON QT.QueueId = Q.QueueId
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
		RAISERROR ('Failed to retrieve qworkerlgp.Queues record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
		ROLLBACK TRANSACTION
	END
RETURN 0
