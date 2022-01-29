--to be run on NOCHOUSE
USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 2

	INSERT INTO [dbo].[COST_DAT_CostCenters] 
	(
		 [CostCenter]
		,[IsBillable]
		,[IsChargedOverHead]
	)
	VALUES
	(
		 'Audit Services'
		,1
		,0
	)
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO [dbo].[COST_DAT_BusinessUnitCostCenters]
	(
		 [BusinessUnitId]
		,[CostCenterId]
		,[Weight]
		,[EffectiveBegin]
		,[EffectiveEnd]
	)
	VALUES
	(
		 17
		,31
		,100
		,'2017-01-01'
		,NULL
	)
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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
	