--SELECT * FROM [CLS].[print].[PrintProcessing] WHERE ScriptDataId = X AND AddedAt = 'XXXX-XX-XX XX:XX:XX.XXX' AND OnEcorr = 'X'
--XX

USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX

	UPDATE
		[print].[PrintProcessing]
	SET
		[PrintedAt] = NULL
	WHERE
		[ScriptDataId] = X
		AND [AddedAt] = 'XXXX-XX-XX XX:XX:XX.XXX'
		AND [OnEcorr] = 'X'
	
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


