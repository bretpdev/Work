USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 2

	UPDATE [dbo].[COST_DAT_BusinessUnitCostCenters]
	SET EffectiveEnd = '2016-12-31'
	WHERE BusinessUnitCostCenterId IN (169,170)
		AND BusinessUnitId = 17
		AND CostCenterId IN (1,15)
		AND [Weight] = 50
		AND EffectiveBegin = '2014-01-01';
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
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
