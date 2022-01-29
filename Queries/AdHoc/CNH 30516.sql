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
	PrintedAt IS NULL AND InactivatedAt IS NULL 
	AND DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX') 

	
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
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
