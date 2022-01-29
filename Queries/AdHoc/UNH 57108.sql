BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 2

	UPDATE 
		CSYS..GENR_DAT_EnterpriseFileSystem
	SET 
		[Path] = 'X:\PADD\FTP\Test\SCRA_*_*_UTLWD42*'
	WHERE
		[Path] = 'X:\PADD\FTP\Test\SCRA_4_4_UTLWD42*'
		AND [Key] = 'UTLWD43'
			
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE 
		CSYS..GENR_DAT_EnterpriseFileSystem
	SET 
		[Path] = 'X:\PADD\FTP\SCRA_*_*_UTLWD42*'
	WHERE
		[Path] = 'X:\PADD\FTP\SCRA_4_4_UTLWD42*'
		AND [Key] = 'UTLWD43'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
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
