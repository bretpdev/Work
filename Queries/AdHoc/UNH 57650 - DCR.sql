USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedCount INT = 26584

--26584
UPDATE
	Uls.[print].PrintProcessing 
SET
	PrintedAt = NULL,
	EcorrDocumentCreatedAt = NULL
WHERE 
	ScriptDataId in (6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75) 
	AND 
	(
		CAST(PrintedAt AS DATE) = CAST(GETDATE() AS DATE)
		OR CAST(EcorrDocumentCreatedAt AS DATE) = CAST(GETDATE() AS DATE)
	)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


IF @ROWCOUNT = @ExpectedCount AND @ERROR = 0
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