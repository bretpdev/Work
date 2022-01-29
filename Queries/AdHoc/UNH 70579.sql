USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		ULS.emailbatch.EmailProcessing
	SET
		EmailData = '4217805067,JAMAIYA,JAMAIYA.JAMES@GMAIL.COM,Demo Change Notice sent to Borrower: ACH Routing Number changedACH Account Number changed'
	WHERE
		EmailProcessingId = 1630606

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		ULS.emailbatch.EmailProcessing
	SET
		EmailData = '7832180458,JENNIFER,SCHEMING_GODDESS@YAHOO.COM,Demo Change Notice sent to Borrower: ACH Routing Number changedACH Account Number changed'
	WHERE
		EmailProcessingId = 1630607

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

IF @ROWCOUNT = 2 AND @ERROR = 0
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