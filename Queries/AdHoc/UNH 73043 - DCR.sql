USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 3025


INSERT INTO ULS.quecomplet.Queues(Queue,SubQueue,TaskControlNumber,ARC,AccountIdentifier,TaskStatusId,ActionResponseId,AddedAt,AddedBy)
SELECT 
	WF_QUE,
	WF_SUB_QUE,
	WN_CTL_TSK,
	PF_REQ_ACT,
	BF_SSN,
	7,--X
	1,--CANCL
	GETDATE(),
	'UNH 73043' 
FROM 
	UDW..WQ20_TSK_QUE 
WHERE
	WF_QUE = 'SZ' 
	AND WF_SUB_QUE = '01' 
	AND WD_ACT_REQ BETWEEN '2019-08-21' AND '2019-08-26' 
	AND WC_STA_WQUE20 NOT IN('X','C')

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END