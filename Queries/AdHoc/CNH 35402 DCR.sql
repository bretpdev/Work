USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedCount INT = X

UPDATE 
	CSYS..LetterFormMapping 
SET
	[Form] = 'PSLF Employment Cert.pdf'
WHERE 
	[Form] = 'XXXX-XXXX_XX-XX-XX_PSLF_XX-XX-XX_vX_IAI Submission.pdf'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

DELETE 
FROM 
	csys..LetterFormMapping 
WHERE 
	[form] = 'Public Service Deferment.pdf'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedCount AND @ERROR = X
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