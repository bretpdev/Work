USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET BusinessUnitId = 57
	WHERE BusinessUnitCostCenterId = 176
		AND BusinessUnitId = 54
		AND CostCenterId = 28
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET BusinessUnitId = 62, CostCenterId = 24
	WHERE BusinessUnitCostCenterId = 173
		AND EffectiveBegin = '2016-08-04'
		AND BusinessUnitId = 57
		AND CostCenterId = 12	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET CostCenterId = 28
	WHERE BusinessUnitCostCenterId = 160
		AND BusinessUnitId = 31
		AND CostCenterId = 8
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET CostCenterId = 15
	WHERE BusinessUnitCostCenterId = 125
		AND BusinessUnitId = 50
		AND CostCenterId = 19
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


