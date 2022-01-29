USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		ULS.emailbatch.EmailProcessing
	SET
		EmailData = '5937564809,STEPHEN,DRBURTENSHAW@PIONEERCLINIC.COM,Demo Change Notice sent to Borrower: ACH Routing Number changedACH Account Number changed'
	WHERE
		EmailProcessingId = 1631047

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		ULS.emailbatch.EmailProcessing
	SET
		EmailData = '9753233095,ANGELA,ANGELAJEAN04@GMAIL.COM,Demo Change Notice sent to Borrower: ACH Routing Number changedACH Account Number changed'
	WHERE
		EmailProcessingId = 1631048

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

	UPDATE
		ULS.emailbatch.EmailProcessing
	SET
		EmailData = '9787368269,CRYSTAL,CRY5TA1P@HOTMAIL.COM,Demo Change Notice sent to Borrower: ACH Account Number changed'
	WHERE
		EmailProcessingId = 1631049

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

IF @ROWCOUNT = 3 AND @ERROR = 0
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