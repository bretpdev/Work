USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		[print].PrintProcessing
	SET
		PrintedAt = NULL
	WHERE
		CAST(AddedAt AS DATE) = 'X/XX/XX'
		AND PrintedAt IS NOT NULL

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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