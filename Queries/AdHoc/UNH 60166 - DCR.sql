USE UDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

DELETE FROM UDW..PD30_PRS_ADR WHERE DF_PRS_ID IN ('499006086','529755439') --2
	
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

DELETE FROM UDW..PD42_PRS_PHN WHERE DF_PRS_ID IN ('499006086','529755439') --3

	-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 5 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END