USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = XX


UPDATE
	PP
SET
	PP.PrintedAt = NULL,
	PP.EcorrDocumentCreatedAt = NULL,
	PP.DeletedAt = GETDATE(),
	PP.DeletedBy = 'CNH XXXXX'
FROM
	CLS.[print].PrintProcessing PP
WHERE 
	ScriptDataId IN (X,X) 
	AND 
	(
		CAST(PrintedAt AS DATE) = CAST(GETDATE() AS DATE) 
		OR CAST(EcorrDocumentCreatedAt AS DATE) = CAST(GETDATE() AS DATE)
	)
	
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