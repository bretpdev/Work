--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 2158

	INSERT INTO ULS.quecomplet.Queues(Queue, SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, HadError, AddedAt, AddedBy)
	SELECT
		WF_QUE,
		WF_SUB_QUE,
		WN_CTL_TSK,
		BF_SSN,
		2, --Complete
		3, --COMPL
		0, --HadError
		GETDATE(),
		SUSER_SNAME()
	FROM
		UDW..WQ20_TSK_QUE
	WHERE
		WF_QUE ='40'
		AND WF_SUB_QUE = '01'
		AND CAST(WD_ACT_REQ AS DATE) = '2018-07-16'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END