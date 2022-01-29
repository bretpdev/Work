--run on NOCHOUSE

BEGIN TRY;
	BEGIN TRANSACTION;

	DECLARE @ROWCOUNT INT = 0,
		@ExpectedRowCount TINYINT = 1;

	DELETE FROM
		BSYS..QBLR_LST_QueueBuilderLists
	WHERE
		[System] = 'OneLINK'
		AND [FileName] = 'ULWK31.LWK31R3'
		AND [Empty] = 'Y'
		AND NoFile = 'N'
		AND MultiFile = 'N'
	;--1

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