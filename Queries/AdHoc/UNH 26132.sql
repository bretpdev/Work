USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		[print].[PrintProcessing]
	SET
		ArcAddedAt = null,
		ImagedAt = null,
		DocumentCreatedAt = null,
		PrintedAt = null,
		DeletedAt = GETDATE(),
		DeletedBy = SYSTEM_USER
	WHERE
		CAST(AddedAt AS DATE) = '2/19/2016'
		AND ScriptFileId = 7
		AND CAST(AddedAt AS time) < '8:00'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 14494 AND @ERROR = 0
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
