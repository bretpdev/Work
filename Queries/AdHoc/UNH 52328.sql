USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 48

	UPDATE [CSYS].[dbo].[COST_DAT_AgentWeights]
	SET EffectiveEnd = '2017-03-31'
	WHERE EffectiveEnd IS NULL
	;
	--21
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO [CSYS].[dbo].[COST_DAT_AgentWeights]
		([SqlUserId],[Weight],[EffectiveBegin],[EffectiveEnd])
	VALUES	
		 (3922,	0.0531, '2017-04-01', NULL)
		,(1492,	0.0651, '2017-04-01', NULL)
		,(1298,	0.0358, '2017-04-01', NULL)
		,(1198,	0.0334, '2017-04-01', NULL)
		,(1399,	0.0290, '2017-04-01', NULL)
		,(1801,	0.0227, '2017-04-01', NULL)
		,(1395,	0.0554, '2017-04-01', NULL)
		,(1686,	0.0302, '2017-04-01', NULL)
		,(1256,	0.0334, '2017-04-01', NULL)
		,(1152,	0.0364, '2017-04-01', NULL)
		,(1519,	0.0290, '2017-04-01', NULL)
		,(1518,	0.0290, '2017-04-01', NULL)
		,(1820,	0.0290, '2017-04-01', NULL)
		,(1818,	0.0254, '2017-04-01', NULL)
		,(1703,	0.0290, '2017-04-01', NULL)
		,(1268,	0.0340, '2017-04-01', NULL)
		,(1734,	0.0380, '2017-04-01', NULL)
		,(1570,	0.0310, '2017-04-01', NULL)
		,(1276,	0.0422, '2017-04-01', NULL)
		,(1161,	0.0715, '2017-04-01', NULL)
		,(1417,	0.0290, '2017-04-01', NULL)
		,(1550,	0.0334, '2017-04-01', NULL)
		,(1280,	0.0451, '2017-04-01', NULL)
		,(1415,	0.0000, '2017-04-01', NULL)
		,(3910,	0.0424, '2017-04-01', NULL)
		,(1451,	0.0557, '2017-04-01', NULL)
		,(1573,	0.0418, '2017-04-01', NULL)
		;
		--27
		
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