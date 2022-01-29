USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

UPDATE
	CDW..LTXX_LetterRequests 
SET
	InactivatedAt = GETDATE(),
	SystemLetterExclusionReasonId = X
WHERE 
	LTXX_LETTER_REQUEST_ID IN 
	(
		XXXXXX,
		XXXXXX,
		XXXXXXX,
		XXXXXXX,
		XXXXXXX,
		XXXXXXX,
		XXXXXXX,
		XXXXXXX,
		XXXXXXX,
		XXXXXXX
	)


	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XX AND @ERROR = X
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
