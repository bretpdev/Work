USE [ECorrFed]
GO

--SELECT * FROM [dbo].[DocumentDetails] WHERE [ZipFileName] IN 
--('KU_IN_ZIP_XXXXXXXXXXXX_m-UXHXCViXaXUkhHXGXtBw.zip','KU_IN_ZIP_XXXXXXXXXXXX_XXbodh_aCEevXkMJIXeXKA.zip');
--XXXX

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		[dbo].[DocumentDetails]
	SET 
		[Printed] = NULL
	WHERE 
		[ZipFileName] IN ('KU_IN_ZIP_XXXXXXXXXXXX_m-UXHXCViXaXUkhHXGXtBw.zip','KU_IN_ZIP_XXXXXXXXXXXX_XXbodh_aCEevXkMJIXeXKA.zip');
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XXXX AND @ERROR = X
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


