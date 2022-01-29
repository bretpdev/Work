--run on UHEAASQLDB
USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX

UPDATE 
	CDW..LTXX_LTR_REQ_PRC 
SET 
	InactivatedAt = GETDATE(), 
	SystemLetterExclusionReasonId = X
WHERE 
	RM_DSC_LTR_PRC IN('TSXXBTRNCL','TSXXBPSTFR','TSXXBSPLIT','TSXXBPRTCH')
	AND CreatedAt <= 'XXXX-XX-XX'
	AND InactivatedAt IS NULL


	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END