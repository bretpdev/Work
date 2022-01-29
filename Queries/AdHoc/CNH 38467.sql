--run on UHEAASQLDB

BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @ROWCOUNT TINYINT = X,
			@ExpectedRowCount TINYINT = X;

	--select * from [CLS].[dasforbfed].[Disasters] where DisasterId = XX AND Disaster = 'South Dakota Severe Winter Storm, Snowstorm, And Flooding (DR-XXXX)'

	UPDATE 
		CLS.dasforbfed.Disasters
	SET 
		Active = X
	WHERE
		DisasterId = XX
		AND Disaster = 'South Dakota Severe Winter Storm, Snowstorm, And Flooding (DR-XXXX)';

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
			PRINT 'Expected row count not met. Expecting ' +  CAST(@ExpectedRowCount AS VARCHAR(XX)) + ' rows, but returned ' + CAST(@ROWCOUNT AS VARCHAR(XX))+ ' rows.';
			ROLLBACK TRANSACTION;
		END
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed. Errors found in SQL statement.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
