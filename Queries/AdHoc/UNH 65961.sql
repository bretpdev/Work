USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	
	UPDATE
		ULS.[print].PrintProcessing
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_SNAME()
	WHERE
		PrintProcessingId = 4862341

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END