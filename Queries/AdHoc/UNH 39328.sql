USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--adds compliance group to cost center
	INSERT INTO [CSYS].[dbo].[COST_DAT_CostCenters]
	(
		[CostCenter]
		,[IsBillable]
		,[IsChargedOverHead]
	)
	VALUES ('Compliance', 1, 1)
	;--1
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	--links compliance group to other business units
	INSERT INTO [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	(
		[BusinessUnitId]
		,[CostCenterId]
		,[Weight]
		,[EffectiveBegin]
		,[EffectiveEnd]
	)
	VALUES (57,28,100,'2016-07-01',NULL)
	;--1	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	
	UPDATE [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
	SET EffectiveEnd = '2015-03-31'
	WHERE EffectiveEnd IS NULL
		AND EffectiveBegin = '2014-10-01'
	;--7
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	INSERT INTO [CSYS].[dbo].[COST_DAT_BatchScriptWeights]
	(
		[CostCenterId]
		,[Weight]
		,[EffectiveBegin]
		,[EffectiveEnd]
	)
	VALUES
		(1,	0.0199656590664058, '2015-01-01', '2015-03-31'),
		(4,	0.339416204128898, '2015-01-01', '2015-03-31'),
		(6,	1.10742855621664, '2015-01-01', '2015-03-31'),
		(8,	57.2575170706385, '2015-01-01', '2015-03-31'),
		(13, 32.0888072515274, '2015-01-01', '2015-03-31'),
		(16, 0.17303571190885, '2015-01-01', '2015-03-31'),
		(20, 9.01382954651333, '2015-01-01', '2015-03-31'),
		(1,	0.0199656590664058, '2015-04-01', '2015-06-30'),
		(4,	0.339416204128898, '2015-04-01', '2015-06-30'),
		(6,	1.10742855621664, '2015-04-01', '2015-06-30'),
		(8,	57.2575170706385, '2015-04-01', '2015-06-30'),
		(13, 32.0888072515274, '2015-04-01', '2015-06-30'),
		(16, 0.17303571190885, '2015-04-01', '2015-06-30'),
		(20, 9.01382954651333, '2015-04-01', '2015-06-30'),
		(1,	0.0219038857493319, '2015-07-01', '2015-12-31'),
		(4,	0.372366057738643, '2015-07-01', '2015-12-31'),
		(6,	0.294972328091003, '2015-07-01', '2015-12-31'),
		(8,	61.6112498357208, '2015-07-01', '2015-12-31'),
		(13, 27.6120383756078, '2015-07-01', '2015-12-31'),
		(16, 0.18983367649421, '2015-07-01', '2015-12-31'),
		(20, 9.89763584059812, '2015-07-01', '2015-12-31'),
		(1,	0.0100841014057237, '2016-01-01', '2016-03-31'),
		(4,	0.413448157634673, '2016-01-01', '2016-03-31'),
		(6,	0.432271813592024, '2016-01-01', '2016-03-31'),
		(8,	62.6800852442706, '2016-01-01', '2016-03-31'),
		(13, 21.0878728596495, '2016-01-01', '2016-03-31'),
		(16, 0.151933794512904, '2016-01-01', '2016-03-31'),
		(20, 14.6595943502141, '2016-01-01', '2016-03-31'),
		(1,	0.0100841014057237, '2016-04-01', '2016-06-30'),
		(4,	0.413448157634673, '2016-04-01', '2016-06-30'),
		(6,	0.432271813592024, '2016-04-01', '2016-06-30'),
		(8,	62.6800852442706, '2016-04-01', '2016-06-30'),
		(13, 21.65258253837, '2016-04-01', '2016-06-30'),
		(16, 0.151933794512904, '2016-04-01', '2016-06-30'),
		(20, 14.6595943502141, '2016-04-01', '2016-06-30'),
		(1,	0.0100719134621195, '2016-07-01', NULL),
		(4,	0.412948451946901, '2016-07-01', NULL),
		(6,	0.431749357076191, '2016-07-01', NULL),
		(8,	62.1208763907634, '2016-07-01', NULL),
		(13, 22.2307273935902, '2016-07-01', NULL),
		(16, 0.151750162829268, '2016-07-01', NULL),
		(20, 14.6418763303319, '2016-07-01', NULL)
	;--42
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = 51 AND @ERROR = 0
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

