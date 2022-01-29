USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

--UTLWS75 prod
	UPDATE
		[CSYS].[dbo].[GENR_DAT_EnterpriseFileSystem]
	SET
		[Path] = 'X\PADD\FTP\SCRA_4_4_ULWOO3'
	WHERE
		[Key] = 'UTLWS75'
		AND
		TestMode = 0
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


--UTLWS75 test
	UPDATE
		[CSYS].[dbo].[GENR_DAT_EnterpriseFileSystem]
	SET
		[Path] = 'X:\PADD\FTP\Test\SCRA_4_4_ULWOO3'
	WHERE
		[Key] = 'UTLWS75'
		AND
		TestMode = 1
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


--UTLWD43 prod
	UPDATE
		[CSYS].[dbo].[GENR_DAT_EnterpriseFileSystem]
	SET
		[Path] = 'X\PADD\FTP\SCRA_4_4_UTLWD42*'
	WHERE
		[Key] = 'UTLWD43'
		AND
		TestMode = 0
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


--UTLWD43 test
	UPDATE
		[CSYS].[dbo].[GENR_DAT_EnterpriseFileSystem]
	SET
		[Path] = 'X:\PADD\FTP\Test\SCRA_4_4_UTLWD42*'
	WHERE
		[Key] = 'UTLWD43'
		AND
		TestMode = 1
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR




IF @ROWCOUNT = 4 AND @ERROR = 0
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