USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET CostCenterId = 30, EffectiveBegin = '2016-07-01'
	WHERE BusinessUnitCostCenterId = 173
		AND BusinessUnitId = 62
		AND CostCenterId = 24
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR



	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET EffectiveEnd = '2016-10-01'
	WHERE BusinessUnitCostCenterId = 178
		AND BusinessUnitId = 60
		AND CostCenterId = 29

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 2 AND @ERROR = 0
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


