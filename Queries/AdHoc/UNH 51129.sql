USE BSYS
GO

BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 355

UPDATE
	BSYS..PRNT_DAT_Print 
SET
	PrintedAt = NULL
WHERE 
	LetterID IN ('IBRDN','AUTOPAYAPV','IDRRCVINC','ALT15','UNATTFU','AUTOPAYDEN')
	AND DATEDIFF(DAY,PrintedAt,'2017-03-20') = 0

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END

