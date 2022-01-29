--RUN ON UHEAASQLDB
USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX

UPDATE 
	CDW..LTXX_LTR_REQ_PRC
SET
	InactivatedAt = NULL,
	SystemLetterExclusionReasonId = NULL,
	PrintedAt = NULL
WHERE
	RM_DSC_LTR_PRC = 'TSXXBTMXLM' 
	AND InactivatedAt IS NOT NULL 
	AND SystemLetterExclusionReasonId = X 

	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
