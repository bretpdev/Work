BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 12

--Query Body
UPDATE
	[ULS].[print].[PrintProcessing] 
SET
	PrintedAt = NULL
WHERE
	PrintedAt >= '3/20/17' AND PrintedAt < '3/21/17' AND 
	(SourceFile LIKE '%R13%' OR
	 SourceFile LIKE '%R15%' OR
	 SourceFile LIKE '%R16%')
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

