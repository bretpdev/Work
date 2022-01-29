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
		,'10-01-2015'
	FROM 
		dbo.COST_DAT_AgentWeights
	WHERE
		EffectiveEnd IS NULL 
		AND SqlUserId NOT IN (1124,1244,1423)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	UPDATE dbo.COST_DAT_AgentWeights
	SET	EffectiveEnd = '2015-09-30'
	--SELECT * FROM dbo.COST_DAT_AgentWeights
	WHERE
		EffectiveBegin = '2015-04-01'
	
		-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO dbo.COST_DAT_AgentWeights
		(
			SqlUserId
			,[Weight]
			,EffectiveBegin
		)
	VALUES
		(1734, 0, '2015-10-01')--NOLASCO

		-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

DECLARE @Weights TABLE (SqlUserId int, [weight] money)

INSERT INTO @Weights
(
	SqlUserId,
	[Weight]
)
VALUES 
	(1492,.0618),--BARNES
	(1298,.0381),--BIGELOW
	(1198,.0357),--BLAIR
	(1449,.0452),--BRIGGS
	(1455,.0328),--CHRISTENSEN
	(1399,.0306),--COLE
	(1395,.0591),--GARFIELD
	(1633,.0307),--GIBSON
	(1256,.0358),--GUTIERREZ
	(1152,.039),--HACK
	(1519,.0307),--HALLADAY
	(1518,.0307),--HANSON
	(1268,.0365),--MCCOMB
	(1734,.0409),--NOLASCO
	(1570,.0325),--OSTLER
	(1276,.0452),--PEHRSON
	(1161,.0764),--PHILLIPS
	(1417,.0328),--PILI
	(1550,.0327),--PRATT
	(1280,.0482),--RYAN
	(1451,.0597),--WALKER
	(1526,.0307),--WALLACE
	(1448,.0497),--WIXOM
	(1573,.0445)--WRIGHT
	
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
