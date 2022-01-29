--run on NOCHOUSE
BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 4;

	UPDATE 
		CSYS..COST_DAT_BusinessUnitCostCenters
	SET 
		EffectiveEnd = CONVERT(DATE,'20191231'), 
		CostCenterId = 8 --LPP portfolio servicing
	WHERE 
		BusinessUnitCostCenterId = 233
		AND BusinessUnitId = 63; --Client Relations
	--1 row

	SELECT @ROWCOUNT = @@ROWCOUNT;

	
	UPDATE
		CSYS..COST_DAT_MailCodeCostCenters
	SET 
		EffectiveEnd = CONVERT(DATE,'20191231'),
		CostCenterId = 14 --CornerStone SChool Services
	WHERE
		MailCodeCostCenterId = 11
		AND MailCode = 'MA4483'; --School Services Federal Servicing
	--1 row
	
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT;


	INSERT INTO
		CSYS..COST_DAT_BusinessUnitCostCenters
		(
		     BusinessUnitId
			,CostCenterId
			,[Weight]
			,EffectiveBegin
			,EffectiveEnd
		)
	VALUES
		(
			63,
			13,
			100,
			CONVERT(DATE,'20200101'),
			NULL
		);
	--1 row

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT;


	INSERT INTO
		CSYS..COST_DAT_MailCodeCostCenters
		(
			 MailCode
			,CostCenterId
			,[Weight]
			,EffectiveBegin
			,EffectiveEnd
		)
	VALUES
		(
			'MA4483',
			13,
			100,
			CONVERT(DATE,'20200101'),
			NULL
		);
	--1 row

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