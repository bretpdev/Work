USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters] 
	SET BusinessUnitId = 59
	WHERE BusinessUnitId = 58
		AND CostCenterId = 24
		AND [Weight] = 100
		AND EffectiveBegin = '2016-07-01'
	;
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--INSERT INTO [CSYS].[dbo].[GENR_LST_BusinessUnits] ([Name])
	--VALUES	('Subcontracting Services')
	--;	
	---- Save/Set the row count and error number (if any) from the previously executed statement
	--SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


	--INSERT INTO [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters] 
	--(
	--   [BusinessUnitId]
 --     ,[CostCenterId]
 --     ,[Weight]
 --     ,[EffectiveBegin]
 --     ,[EffectiveEnd]
	--)
	--VALUES	
	--	 (58, 24, 100, '2016-07-01', null)
	--;	
	---- Save/Set the row count and error number (if any) from the previously executed statement
	--SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--UPDATE [CSYS].[dbo].[COST_DAT_AgentWeights]
	--SET EffectiveEnd = '2016-06-30'
	--WHERE EffectiveEnd IS NULL
	--;
	---- Save/Set the row count and error number (if any) from the previously executed statement
	--SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--INSERT INTO [CSYS].[dbo].[COST_DAT_AgentWeights]
	--(
	--   [SqlUserId]
 --     ,[Weight]
 --     ,[EffectiveBegin]
 --     ,[EffectiveEnd]
	--)
	--VALUES 
	--	(1448, 0.0574, '2016-07-01' , NULL),
	--	(1276, 0.0518, '2016-07-01' , NULL),
	--	(1399, 0.0351, '2016-07-01' , NULL),
	--	(1268, 0.0419, '2016-07-01' , NULL),
	--	(1519, 0.0353, '2016-07-01' , NULL),
	--	(1161, 0.0879, '2016-07-01' , NULL),
	--	(1417, 0.0378, '2016-07-01' , NULL),
	--	(1492, 0.0773, '2016-07-01' , NULL),
	--	(1451, 0.0685, '2016-07-01' , NULL),
	--	(1198, 0.0411, '2016-07-01' , NULL),
	--	(1256, 0.0411, '2016-07-01' , NULL),
	--	(1518, 0.0352, '2016-07-01' , NULL),
	--	(1573, 0.0514, '2016-07-01' , NULL),
	--	(1734, 0.0466, '2016-07-01' , NULL),
	--	(1280, 0.0554, '2016-07-01' , NULL),
	--	(1395, 0.0681, '2016-07-01' , NULL),
	--	(1415, 0.0416, '2016-07-01' , NULL),
	--	(1298, 0.0441, '2016-07-01' , NULL),
	--	(1550, 0.0377, '2016-07-01' , NULL),
	--	(1152, 0.0448, '2016-07-01' , NULL)
	--;
	---- Save/Set the row count and error number (if any) from the previously executed statement
	--SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--IF @ROWCOUNT = 47 AND @ERROR = 0
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
	