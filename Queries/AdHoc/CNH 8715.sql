USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		CentralizedPrintingLetter
	SET
		BusinessUnitId = X
	WHERE
		SeqNum IN (XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX,XXXXX)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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