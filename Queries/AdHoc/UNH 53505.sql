--TO BE RUN ON NOCHOUSE

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 70

	--ends prior quarter
	UPDATE [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
	SET EffectiveEnd = '2017-03-31'
	WHERE EffectiveEnd IS NULL
		AND	BatchScriptWeightId IN 
		(
			130,
			131,
			132,
			133,
			134,
			135,
			136
		);
	--7
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--FY17 Q4 and FY18 Q1 data (same)
	INSERT INTO [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
		(
			 [CostCenterId]
			,[Weight]
			,[EffectiveBegin]
			,[EffectiveEnd]
		)
	VALUES	
		 (1,'0.0206987911905945','2017-04-01',NULL)
		,(4,'0.248385494287134','2017-04-01',NULL)
		,(6,'1.13153391841916',	'2017-04-01',NULL)
		,(8,'41.7701606226196',	'2017-04-01',NULL)
		,(13,'50.6706408345753','2017-04-01',NULL)
		,(16,'0.51057018270133','2017-04-01',NULL)
		,(20,'5.45758127725341','2017-04-01',NULL)
		;
	--7
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--cost center fix
	UPDATE [CSYS].[dbo].[COST_DAT_CostCenters]
	SET IsBillable = 1
	WHERE CostCenterId = 12
		AND CostCenter = 'CornerStone Subcontractor Services'
		AND IsBillable = 0
		AND IsChargedOverHead = 0
	--1

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--ends prior quarter
	UPDATE [CSYS].[dbo].[COST_DAT_AgentWeights]
	SET EffectiveEnd = '2017-06-30'
	WHERE EffectiveEnd IS NULL
	;
	--27
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--FY18 Q1 data
	INSERT INTO [CSYS].[dbo].[COST_DAT_AgentWeights]
		([SqlUserId],[Weight],[EffectiveBegin],[EffectiveEnd])
	VALUES	
		 (3922, 0.0514, '2017-07-01', NULL)	--Ryan
		,(1492, 0.0626, '2017-07-01', NULL)	--Eric
		,(1298, 0.0349, '2017-07-01', NULL)	--Riley
		,(1198, 0.0346, '2017-07-01', NULL)	--Jeremy
		,(1748, 0.0276, '2017-07-01', NULL)	--Nicholas
		,(1399, 0.0279, '2017-07-01', NULL)	--Candice
		,(1801, 0.0217, '2017-07-01', NULL)	--Alyssa
		,(1395, 0.0549, '2017-07-01', NULL)	--Melanie
		,(1686, 0.0291, '2017-07-01', NULL)	--Savanna
		,(1256, 0.0346, '2017-07-01', NULL)	--Jesse
		,(1152, 0.0380, '2017-07-01', NULL)	--Wendy
		,(1519, 0.0280, '2017-07-01', NULL)	--David
		,(1518, 0.0303, '2017-07-01', NULL)	--Jessica
		,(1818, 0.0245, '2017-07-01', NULL)	--Adam
		,(1820, 0.0280, '2017-07-01', NULL)	--Jared
		,(1624, 0.0287, '2017-07-01', NULL)	--AJ
		,(1685, 0.0202, '2017-07-01', NULL)	--Shannon
		,(1703, 0.0279, '2017-07-01', NULL)	--Conor
		,(1268, 0.0328, '2017-07-01', NULL)	--Colton
		,(1734, 0.0363, '2017-07-01', NULL)	--JR
		,(1570, 0.0301, '2017-07-01', NULL)	--Steven
		,(1276, 0.0405, '2017-07-01', NULL)	--Bret
		,(1161, 0.0690, '2017-07-01', NULL)	--Debbie
		,(1280, 0.0468, '2017-07-01', NULL)	--Jarom
		,(3910, 0.0408, '2017-07-01', NULL)	--Aaron
		,(1451, 0.0537, '2017-07-01', NULL)	--Evan
		,(1573, 0.0450, '2017-07-01', NULL)	--Josh
		--,(1417, 0.0290, '2017-07-01', NULL)	--Devin (exclude for now)
		--,(1550, 0.0334, '2017-07-01', NULL)	--Seth (exclude for now)
	--27
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	--fixes time tracking error
	UPDATE [Reporting].[dbo].[TimeTracking]
	SET EndTime = '2017-08-18 16:28:00.000'
	WHERE EndTime = '2017-10-02 10:51:04.240'
		AND StartTime = '2017-08-18 16:25:38.570'
		AND TimeTrackingId = '79366'
		AND SqlUserID = '1558' --Peter Busche
		AND TicketID = '32129'
	--1

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
		