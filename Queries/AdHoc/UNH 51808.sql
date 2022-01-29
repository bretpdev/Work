USE BSYS
GO

BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 1

--Query Body
UPDATE 
	BSYS..QBLR_LST_QueueBuilderLists
SET
	[FileName] = 'UTLWD43.ULWD43R2.*' 
WHERE
	[FileName] = 'UTLWD43.ULWD43R2'
--Query Body

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



