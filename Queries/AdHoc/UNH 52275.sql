USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 14

	UPDATE [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
	SET EffectiveEnd = '2016-12-31'
	WHERE EffectiveEnd IS NULL
		AND	BatchScriptWeightId IN 
		(
			 116
			,117
			,118
			,119
			,120
			,121
			,122
		);
		--7
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
		(
			 [CostCenterId]
			,[Weight]
			,[EffectiveBegin]
			,[EffectiveEnd]
		)
	VALUES	
		 (1, '0.0187013764213046','2017-01-01',NULL)
		,(4, '1.34649910233393',  '2017-01-01',NULL)
		,(6, '0.835328146818273', '2017-01-01',NULL)
		,(8, '69.4494314781568',  '2017-01-01',NULL)
		,(13,'21.918013165769',   '2017-01-01',NULL)
		,(16,'0.46130061839218',  '2017-01-01',NULL)
		,(20,'5.97072611210852',  '2017-01-01',NULL)
		;
		--7
		
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
