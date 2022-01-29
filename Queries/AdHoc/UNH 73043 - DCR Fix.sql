USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 3025

UPDATE
	ULS.quecomplet.Queues
SET
	PickedUpForProcessing = NULL,
	ProcessedAt = NULL,
	HadError = NULL,
	WasFound = NULL,
	ActionResponseId = 8
WHERE
	Queue = 'SZ' 
	AND SubQueue = '01'
	AND AddedBy = 'UNH 73043'
	
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