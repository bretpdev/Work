USE NobleCalls
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		NobleCallHistory
	SET
		AccountIdentifier = 'XXXXXXXXX'
	WHERE
		AccountIdentifier = 'XXXXXXXXXX'

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

USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		ArcAddProcessing
	SET
		AccountNumber = 'XXXXXXXXXX'
	WHERE
		AccountNumber = 'XXXXXXXXXX'

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