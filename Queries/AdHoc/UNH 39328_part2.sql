USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--fixes business unit id reference
	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET BusinessUnitId = 54 --AUDIT COORDINATION
	WHERE BusinessUnitCostCenterId = 176
		AND BusinessUnitId = 57
		AND CostCenterId = 28
		AND [Weight] = 100
		AND EffectiveBegin = '2016-07-01'
		AND EffectiveEnd IS NULL
	;--1
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	

IF @ROWCOUNT = 1 AND @ERROR = 0
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

