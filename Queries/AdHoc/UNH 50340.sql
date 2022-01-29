USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
	SET EffectiveEnd = '2016-09-30'
	WHERE EffectiveEnd IS NULL
		AND	BatchScriptWeightId IN (
			 109
			,110
			,111
			,112
			,113
			,114
			,115
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
		 (1, '0.0187013764213046', '2016-10-01', NULL)
		,(4, '1.34649910233393', '2016-10-01', NULL)
		,(6, '0.835328146818273', '2016-10-01', NULL)
		,(8, '69.5242369838421', '2016-10-01', NULL)
		,(13, '21.918013165769', '2016-10-01', NULL)
		,(16, '0.46130061839218', '2016-10-01', NULL)
		,(20, '5.8959206064233', '2016-10-01', NULL)
		;
		--7
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 14 AND @ERROR = 0
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


