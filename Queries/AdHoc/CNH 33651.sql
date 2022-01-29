BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX

	INSERT INTO [CLS].[quecomplet].[Queues]
	(
		[Queue]
		,[SubQueue]
		,[TaskControlNumber]
		,[AccountIdentifier]
		,[TaskStatusId]
		,[ActionResponseId]
		,[PickedUpForProcessing]
		,[ProcessedAt]
		,[HadError]
		,[AddedAt]
		,[AddedBy]
		,[DeletedAt]
		,[DeletedBy]
	)
	SELECT DISTINCT
		'XX' [Queue]
		,'XX' [SubQueue]
		,WQXX.WN_CTL_TSK [TaskControlNumber]
		,WQXX.BF_SSN [AccountIdentifier]
		,X [TaskStatusId]
		,X [ActionResponseId]
		,NULL [PickedUpForProcessing]
		,NULL [ProcessedAt]
		,X [HadError]
		,GETDATE() [AddedAt]
		,'CNH XXXXX' [AddedBy]
		,NULL [DeletedAt]
		,NULL [DeletedBy]
	FROM
		[CDW].[dbo].[WQXX_TSK_QUE] WQXX
	WHERE
		WQXX.WF_QUE = 'XX'
		AND WQXX.WX_MSG_X_TSK LIKE '%MUST ENTER%'
		AND WQXX.WC_STA_WQUEXX = 'U'

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