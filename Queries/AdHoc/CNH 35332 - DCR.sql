USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedCount INT = XXXXX

UPDATE
	CLS.scra.ScriptProcessing
SET
	DeletedAt = GETDATE(),
	DeletedBy = 'CNH XXXXX'
WHERE
	DeletedAt IS NULL
	AND CreatedAt > 'XXXX-XX-XX'
	AND ErroredAt IS NULL
	--XXXXX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

UPDATE
	CLS.scra.ScriptProcessing
SET
	DeletedAt = NULL,
	DeletedBy = NULL
WHERE
	DeletedAt IS NOT NULL
	AND CreatedAt > 'XXXX-XX-XX'
	AND ErroredAt IS NULL
	AND DeletedBy = 'CNH XXXXX'
	--XXXXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedCount AND @ERROR = X
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