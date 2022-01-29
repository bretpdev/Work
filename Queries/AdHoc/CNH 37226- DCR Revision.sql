USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX

UPDATE 
	CLS..ArcAddProcessing 
SET 
	ProcessingAttempts=X, ProcessedAt = NULL, ArcTypeId = X 
WHERE 
	ARC = 'BRTXT' AND CreatedBy = 'CNH XXXXX'

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --XX rows

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
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