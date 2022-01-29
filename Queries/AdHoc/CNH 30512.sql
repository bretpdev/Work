USE CLS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		[print].PrintProcessing
	SET
		ImagedAt = NULL
	WHERE
		PrintProcessingId IN
		(
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX,
			XXXXXXX
		)

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
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
