USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = XXX

	--XXX
UPDATE 
	CDW..LTXX_LTR_REQ_PRC 
SET 
	PrintedAt = NULL 
WHERE 
	RM_DSC_LTR_PRC != 'TSXXBQRTLY' 
	AND CAST(PrintedAt AS DATE) = CAST('XXXX-XX-XX' AS DATE) 
	AND InactivatedAt IS NULL

	-- Save/Set the row count and error number (if any) from the previously executed statement
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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END