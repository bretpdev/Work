USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		[CSYS].[dbo].[GENR_DAT_EnterpriseFileSystem]
	SET
		[Path] = 'Z:\Batch\FTP\SCRA_4_4_UNWS81*'
	WHERE
		[Key] = 'UTNW021'
		AND
		TestMode = 0

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		[CSYS].[dbo].[GENR_DAT_EnterpriseFileSystem]
	SET
		[Path] = 'Y:\Batch\FTP\SCRA_4_4_UNWS81*'
	WHERE
		[Key] = 'UTNW021'
		AND
		TestMode = 1

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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