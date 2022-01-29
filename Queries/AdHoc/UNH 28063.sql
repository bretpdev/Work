USE CSYS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

INSERT INTO dbo.COST_DAT_AgentWeights
	(
		--AgentWeightId,
		SqlUserId,
		[Weight],
		EffectiveBegin
	)
	SELECT
		--AgentWeightId
		SqlUserId
		,0
		,'4-01-2016'
	FROM 
		dbo.COST_DAT_AgentWeights
	WHERE
		EffectiveEnd IS NULL 
		AND SqlUserId NOT IN (1455)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	UPDATE dbo.COST_DAT_AgentWeights
	SET	EffectiveEnd = '2016-03-31'
	--SELECT * FROM dbo.COST_DAT_AgentWeights
	WHERE
		EffectiveBegin = '2016-01-01'
--		AND SqlUserId IN (1691, 1449, 1633, 1703, 1570, 1448)
	
		-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--	INSERT INTO dbo.COST_DAT_AgentWeights
--		(
--			SqlUserId
--			,[Weight]
--			,EffectiveBegin
--		)
--	VALUES
--		(1691, 0, '2016-1-01')--ANDRUS
--		,(1703, 0, '2016-1-01')--MACDONALD
--		,(1415, 0, '2016-1-01')--TITCOMB

		-- Save/Set the row count and error number (if any) from the previously executed statement
--	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

DECLARE @Weights TABLE (SqlUserId int, [weight] money)

INSERT INTO @Weights
(
	SqlUserId,
	[Weight]
)
VALUES 
	(1691,.0322),--ANDRUS
	(1492,.0648),--BARNES
	(1298,.0366),--BIGELOW
	(1198,.0342),--BLAIR
	(1449,.0434),--BRIGGS
--	(1455,.0328),--CHRISTENSEN
	(1399,.0294),--COLE
	(1395,.0567),--GARFIELD
	(1633,.0295),--GIBSON
	(1256,.0344),--GUTIERREZ
	(1152,.0375),--HACK
	(1519,.0295),--HALLADAY
	(1518,.0295),--HANSON
	(1703,.0289),--MACDONALD
	(1268,.0350),--MCCOMB
	(1734,.0392),--NOLASCO
	(1570,.0314),--OSTLER
	(1276,.0433),--PEHRSON
	(1161,.0733),--PHILLIPS
	(1417,.0314),--PILI
	(1550,.0313),--PRATT
	(1280,.0463),--RYAN
	(1415,.0345),--TITCOMB
	(1451,.0573),--WALKER
	(1448,.0477),--WIXOM
	(1573,.0427)--WRIGHT
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

UPDATE 
	AW
SET 
	AW.[Weight] = T.[WEIGHT] 
	--SELECT*
FROM 
	dbo.COST_DAT_AgentWeights AW
	JOIN @Weights T
		ON AW.SqlUserId = T.SqlUserId
WHERE 
	EffectiveEnd IS NULL 
	
		-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF  @ERROR = 0
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
