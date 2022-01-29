USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	
	INSERT INTO CLS..ArcAddProcessing(AccountNumber, ARC, ArcTypeId, Comment, IsEndorser, IsReference, ProcessOn, ScriptId)
	SELECT
		AccountNumber,
		'NDSNF',
		X,
		'Disaster email notice sent to borrower',
		X,
		X,
		GETDATE(),
		'EmailBatch'
	FROM
		tempdb..Accounts

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XXXXX AND @ERROR = X
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
