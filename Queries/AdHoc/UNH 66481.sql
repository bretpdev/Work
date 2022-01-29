--run on UHEAASQLDB
BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 2;

	UPDATE
		ULS.dasforbfed.Disasters
	SET 
		Active = 0 --deactivates
	WHERE 
		AddedBy IN ('UNH 66293','UNH 66340')
	--2 ROWS

	SELECT @ROWCOUNT = @@ROWCOUNT;

	IF @ROWCOUNT = @ExpectedRowCount
		BEGIN
			PRINT 'Transaction committed.'
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			PRINT 'Transaction NOT committed.';
			PRINT 'Expected row count not met. Expecting ' +  CAST(@ExpectedRowCount AS VARCHAR(10)) + ' rows, but returned ' + CAST(@ROWCOUNT AS VARCHAR(10))+ ' rows.';
			ROLLBACK TRANSACTION;
		END
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed. Errors found in SQL statement.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;