USE UDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		UDW..LT20_LTR_REQ_PRC
	SET
		ProcessingAttempts = 0
	WHERE
		RM_DSC_LTR_PRC = 'US06BFAPRL'
		AND ((OnEcorr = 0 AND PrintedAt IS NULL) OR (OnEcorr = 1 AND EcorrDocumentCreatedAt IS NULL))
		AND InactivatedAt IS NULL

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 19 AND @ERROR = 0
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