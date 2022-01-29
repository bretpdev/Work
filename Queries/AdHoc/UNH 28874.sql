--to be run on UHEAASQLDB
USE [ULS]
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

UPDATE
	ULS.[print].PrintProcessing
SET
	DeletedAt = GETDATE(),
	DeletedBy = 'DCR'
WHERE 
	ScriptFileId IN (1,2,3,4,5,6) 
	AND DATEDIFF(DAY,AddedAt,GETDATE()) < = 1 
	AND SourceFile LIKE '%.U0000'
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 156 AND @ERROR = 0
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