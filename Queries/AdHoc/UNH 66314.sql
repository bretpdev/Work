--run on NOCHOUSE
BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 4;

	UPDATE CSYS..COST_DAT_CostCenters
	SET IsChargedOverHead = 0
	WHERE CostCenter IN ('LGP Special Default Aversion','LGP Special School Aversion');
	--2 ROWS
	SELECT @ROWCOUNT = @@ROWCOUNT;

	UPDATE CSYS..COST_DAT_BusinessUnitCostCenters
	SET CostCenterId = 13 --CornerStone Portfolio Servicing
	WHERE BusinessUnitCostCenterId = 233
		AND BusinessUnitId = 63; --Client Relations
	--1 ROW
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT;

	UPDATE CSYS..COST_DAT_MailCodeCostCenters
	SET CostCenterId = 13 --CornerStone Portfolio Servicing
	WHERE MailCodeCostCenterId = 11
		AND MailCode = 'MA4483'; --School Services Federal Servicing
	--1 ROW
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT;

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