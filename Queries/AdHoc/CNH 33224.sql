USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	INSERT INTO CDW..FormatTranslation(FmtName, Label, Start)
	VALUES
		('$FATRAN','GOV INT CREDIT - XX% INTEREST SUBSIDY','XXAX'),
		('$FATRAN','GOV INT CREDIT - XX% INTEREST SUBSIDY','XXAX'),
		('$FATRAN','WRITE OFF - AUTOMATIC','XXXX'),
		('$FATRAN','DECONVERSION OFFSET - AUTOMATIC','XXXX'),
		('$FATRAN','NON-SUB INT ACCRUAL - AUTOMATIC','XXXX')


	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
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