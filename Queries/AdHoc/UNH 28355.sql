BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 1

--Query Body
INSERT INTO [UDW].[dbo].[FormatTranslation] (FmtName, Label, Start)
VALUES ('$FORSTA', 'Student Loan Debt Burden Forb', '44')

INSERT INTO [CDW].[dbo].[FormatTranslation] (FmtName, Label, Start)
VALUES ('$FORSTA', 'Student Loan Debt Burden Forb', '44')
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

