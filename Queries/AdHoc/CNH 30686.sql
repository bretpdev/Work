BEGIN TRANSACTION
DECLARE @ERROR INT = X
DECLARE @ROWCOUNT INT = X
DECLARE @ExpectedRowCount INT = X

--Query Body
UPDATE
	CLS.dbo.BatchEmail
SET
	SubjectLine = 'XXXX Tax Information'
WHERE
	BatchEmailId = XX
--Query Body

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END

