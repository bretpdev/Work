--SELECT * FROM [CLS].[quecomplet].[Queues] WHERE [Queue] = 'XX' and SubQueue = 'XX'--XXX

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX

	UPDATE 
		[CLS].[quecomplet].[Queues]
	SET 
		PickedUpForProcessing = NULL
		,ProcessedAt = Null
		,HadError = X
	WHERE
		[Queue] = 'XX'
		AND SubQueue = 'XX'
	;

  	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
