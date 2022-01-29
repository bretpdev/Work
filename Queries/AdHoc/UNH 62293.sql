--run on UHEAASQLDB

BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @ROWCOUNT TINYINT = 0,
			@ExpectedRowCount TINYINT = 1;

	--select * from [ULS].[dasforbfed].[Disasters] where DisasterId = 24 AND Disaster = 'South Dakota Severe Winter Storm, Snowstorm, And Flooding (DR-4440)';

	UPDATE 
		ULS.dasforbfed.Disasters
	SET 
		Active = 1
	WHERE
		DisasterId = 24
		AND Disaster = 'South Dakota Severe Winter Storm, Snowstorm, And Flooding (DR-4440)';

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
