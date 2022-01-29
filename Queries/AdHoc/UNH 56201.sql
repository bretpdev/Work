--RUN ON NOCHOUSE

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 72
	
	--ends prior quarter
	UPDATE [CSYS].[dbo].[COST_DAT_AgentWeights]
	SET EffectiveEnd = '2017-12-31'
	WHERE EffectiveEnd IS NULL
		AND EffectiveBegin = '2017-10-01'
	;--26
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--FY18 Q3 data
	INSERT INTO [CSYS].[dbo].[COST_DAT_AgentWeights]
		([SqlUserId],[Weight],[EffectiveBegin],[EffectiveEnd])
	VALUES	
		 (1161, 0.0724, '2018-01-01', NULL) /*Debbie*/
		,(1152, 0.0398, '2018-01-01', NULL) /*Wendy*/
		,(1198, 0.0363, '2018-01-01', NULL) /*Jeremy*/
		,(1519, 0.0293, '2018-01-01', NULL) /*David*/
		,(1399, 0.0293, '2018-01-01', NULL) /*Candice*/
		,(1818, 0.0257, '2018-01-01', NULL) /*Adam*/
		,(1801, 0.0228, '2018-01-01', NULL) /*Alyssa*/
		,(1280, 0.0579, '2018-01-01', NULL) /*Jarom*/
		,(3922, 0.0539, '2018-01-01', NULL) /*Ryan*/
		,(1451, 0.0564, '2018-01-01', NULL) /*Evan*/
		,(1734, 0.0381, '2018-01-01', NULL) /*JR*/
		,(1276, 0.0425, '2018-01-01', NULL) /*Bret*/
		,(1573, 0.0472, '2018-01-01', NULL) /*Josh*/
		,(3910, 0.0428, '2018-01-01', NULL) /*Aaron*/
		,(1395, 0.0576, '2018-01-01', NULL) /*Melanie*/
		,(1256, 0.0363, '2018-01-01', NULL) /*Jesse*/
		,(1298, 0.0366, '2018-01-01', NULL) /*Riley*/
		,(1268, 0.0344, '2018-01-01', NULL) /*Colton*/
		,(1518, 0.0317, '2018-01-01', NULL) /*Jessica*/
		,(1748, 0.0290, '2018-01-01', NULL) /*Nicholas*/
		,(1624, 0.0301, '2018-01-01', NULL) /*AJ*/
		,(1685, 0.0271, '2018-01-01', NULL) /*Shannon*/
		,(1570, 0.0316, '2018-01-01', NULL) /*Steven*/
		,(1703, 0.0293, '2018-01-01', NULL) /*Conor*/
		,(1686, 0.0305, '2018-01-01', NULL) /*Savanna*/
		,(1820, 0.0316, '2018-01-01', NULL) /*Jared*/
	;--26
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
	SET EffectiveEnd = '2017-09-30'
	WHERE EffectiveEnd IS NULL
		AND EffectiveBegin = '2017-04-01'
	;--7

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
		([CostCenterId],[Weight],[EffectiveBegin],[EffectiveEnd])
	VALUES
	 (4, '0.487495734412324', '2017-10-01', '2017-12-31')--Q2
	,(6, '0.601244739108533', '2017-10-01', '2017-12-31')--Q2
	,(8, '50.6686816490356',  '2017-10-01', '2017-12-31')--Q2
	,(13,'41.5541364013065',  '2017-10-01', '2017-12-31')--Q2
	,(16,'0.601244739108533', '2017-10-01', '2017-12-31')--Q2
	,(20,'6.08719673702855',  '2017-10-01', '2017-12-31')--Q2
	,(1, '0.0401574170749337','2018-01-01', NULL)--Q3
	,(4, '0.481889004899205', '2018-01-01', NULL)--Q3
	,(6, '0.990549621181699', '2018-01-01', NULL)--Q3
	,(8, '42.9657591090408',  '2018-01-01', NULL)--Q3
	,(13,'43.0808770379889',  '2018-01-01', NULL)--Q3
	,(16,'0.990549621181699', '2018-01-01', NULL)--Q3
	,(20,'11.4502181886328',  '2018-01-01', NULL)--Q3
	;--13

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
