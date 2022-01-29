USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		[print].PrintProcessing
	SET
		PrintedAt = NULL
	WHERE
		(OnEcorr = X
		AND PrintedAt IS NOT NULL
		AND ScriptFileId NOT IN (X,X,X,XX,XX,XX,XX,XX))
		AND
		(PrintProcessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintProcessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX
		OR PrintPRocessingId BETWEEN XXXXXXX AND XXXXXXX)
				
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
