USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = XXX

INSERT INTO CLS.quecomplet.Queues(Queue, SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, AddedAt, AddedBy)
SELECT
	WQXX.WF_QUE,
	WQXX.WF_SUB_QUE,
	WQXX.WN_CTL_TSK,
	PDXX.DF_SPE_ACC_ID,
	X,
	X,
	GETDATE(),
	SUSER_SNAME()
FROM
	CDW..WQXX_TSK_QUE WQXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = WQXX.BF_SSN
WHERE
	WQXX.WF_QUE = 'XM'
	AND WQXX.WD_ACT_REQ >= 'XXXX-XX-XX'
	AND WQXX.WC_STA_WQUEXX NOT IN('X','C')

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END