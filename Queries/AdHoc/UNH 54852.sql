--RUN ON NOCHOUSE

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 53
	
	--ends prior quarter
	UPDATE [CSYS].[dbo].[COST_DAT_AgentWeights]
	SET EffectiveEnd = '2017-09-30'
	WHERE EffectiveEnd IS NULL
		AND EffectiveBegin = '2017-07-01'
	;
	--27
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--FY18 Q2 data
	INSERT INTO [CSYS].[dbo].[COST_DAT_AgentWeights]
		([SqlUserId],[Weight],[EffectiveBegin],[EffectiveEnd])
	VALUES	
		 (3922, 0.0542, '2017-10-01', NULL) /*Ryan*/
		,(1298, 0.0368, '2017-10-01', NULL) /*Riley*/
		,(1198, 0.0365, '2017-10-01', NULL) /*Jeremy*/
		,(1748, 0.0292, '2017-10-01', NULL) /*Nicholas*/
		,(1399, 0.0295, '2017-10-01', NULL) /*Candice*/
		,(1801, 0.0229, '2017-10-01', NULL) /*Alyssa*/
		,(1395, 0.0580, '2017-10-01', NULL) /*Melanie*/
		,(1686, 0.0307, '2017-10-01', NULL) /*Savanna*/
		,(1256, 0.0365, '2017-10-01', NULL) /*Jesse*/
		,(1152, 0.0401, '2017-10-01', NULL) /*Wendy*/
		,(1519, 0.0295, '2017-10-01', NULL) /*David*/
		,(1518, 0.0319, '2017-10-01', NULL) /*Jessica*/
		,(1818, 0.0259, '2017-10-01', NULL) /*Adam*/
		,(1820, 0.0318, '2017-10-01', NULL) /*Jared*/
		,(1624, 0.0303, '2017-10-01', NULL) /*AJ*/
		,(1685, 0.0213, '2017-10-01', NULL) /*Shannon*/
		,(1703, 0.0294, '2017-10-01', NULL) /*Conor*/
		,(1268, 0.0346, '2017-10-01', NULL) /*Colton*/
		,(1734, 0.0383, '2017-10-01', NULL) /*JR*/
		,(1570, 0.0317, '2017-10-01', NULL) /*Steven*/
		,(1276, 0.0428, '2017-10-01', NULL) /*Bret*/
		,(1161, 0.0728, '2017-10-01', NULL) /*Debbie*/
		,(1280, 0.0583, '2017-10-01', NULL) /*Jarom*/
		,(3910, 0.0430, '2017-10-01', NULL) /*Aaron*/
		,(1451, 0.0567, '2017-10-01', NULL) /*Evan*/
		,(1573, 0.0475, '2017-10-01', NULL) /*Josh*/
	--26
		
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
