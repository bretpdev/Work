--RUN ON UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 8647

INSERT INTO ULS.quecomplet.Queues(Queue, SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, PickedUpForProcessing, ProcessedAt, HadError, AddedAt, AddedBy, DeletedAt, DeletedBy)
SELECT
	WF_QUE AS Queue,
	WF_SUB_QUE AS SubQueue,
	WN_CTL_TSK AS TaskControlNumber,
	BF_SSN AS AccountIdentifier,
	7 AS TaskStatusId, --Cancel
	1 AS ActionResponseId, --CANCL
	NULL AS PickedUpForProcessing,
	NULL AS ProcessedAt,
	0 AS HadError,
	GETDATE() AS AddedAt,
	'UNH 54957' AS AddedBy,
	NULL AS DeletedAt,
	NULL AS DeletedBy
FROM 
	[UDW].[dbo].[WQ20_TSK_QUE]
WHERE 
	WF_QUE = 'R9' 
	AND WF_SUB_QUE = '02' 
	AND WF_USR_ASN_TSK = 'UT00020'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
