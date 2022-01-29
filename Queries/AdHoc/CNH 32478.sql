USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	
	INSERT INTO cls..DueDateChange(Ssn, AccountNumber, DueDate)
	SELECT
		PDXX.BF_SSN [Ssn],
		DD.DF_SPE_ACC_ID [AccountNumber],
		XX [DueDate]
	FROM
		tempdb..DueDateAccounts DD
		LEFT JOIN CDW..PDXX_Borrower PDXX ON DD.DF_SPE_ACC_ID = PDXX.DF_SPE_ACC_ID

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
