USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	--sql code
	UPDATE
		CDW..LTXX_LetterRequests 
	SET
		PrintedAt = NULL
	WHERE 
		RM_DSC_LTR_PRC IN ('TSXXBDXXX','TSXXBFXXX','TSXXBFXXXJ','TSXXBFXXXC')
		AND PrintedAt BETWEEN 'XXXX-XX-XX XX:XX:XX.XXX' and 'XXXX-XX-XX XX:XX:XX.XXX'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
IF @ROWCOUNT = XXX AND @ERROR = X
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
