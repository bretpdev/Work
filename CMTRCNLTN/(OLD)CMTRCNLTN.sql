SET CONCAT_NULL_YIELDS_NULL OFF

DECLARE
	@RowCount INT = 1,
	@CycleCount INT = 0,
	@ARCList VARCHAR(MAX),
	@SSNList VARCHAR(MAX),
	@SQLStatement VARCHAR(MAX)


PRINT 'CornerStone'
-- create a comma delimited list of ARCs
SELECT @ARCList = 
	STUFF(
			(
				SELECT 
					''''',''''' + ARC AS [text()] 
				FROM   
				(
					SELECT DISTINCT 
						ARC 
					FROM 
						NobleCalls..Arcs
				) X
				FOR XML PATH ('')
			),1,1, ''''''
		) + ''''''

PRINT 'ARC List:  ' + @ARCList

-- do nothing if no rows were updated during the last cycle or 10 cycles of updates have already been processed
WHILE @RowCount > 0 AND @CycleCount <= 10
BEGIN
	-- limit cycles to 10
	SET @CycleCount = @CycleCount + 1

	-- create a comma delimited list of SSNs
	SELECT @SSNList = 
		STUFF(
				(
					SELECT DISTINCT
						''''',''''' + SSN
					FROM   
						(	-- limited to 480 SSNs to avoid going over the 8000 character limit imposed by OPENQUERY
							SELECT DISTINCT TOP 480 
								COALESCE(BSSN.BF_SSN, BACT.BF_SSN) [SSN],
								NCH.CreatedAt
							FROM
								NobleCalls..NobleCallHistory NCH
								LEFT JOIN CDW..PD10_Borrower BSSN on BSSN.BF_SSN = NCH.AccountIdentifier
								LEFT JOIN CDW..PD10_Borrower BACT on BACT.DF_SPE_ACC_ID = NCH.AccountIdentifier
							WHERE
								NCH.CallCampaign != 'VABC'
								AND
								COALESCE(BSSN.BF_SSN, BACT.BF_SSN) IS NOT NULL
								AND
								NCH.ReconciledAt IS NULL
								AND
								NCH.DeletedAt IS NULL
								AND
								NCH.ArcAddProcessingId IS NOT NULL
								AND
								NCH.CreatedAt < DATEADD(HOUR, -4, GETDATE()) -- don't try to reconcile anything added within the last 4 hours
							ORDER BY
								NCH.CreatedAt DESC							
						) X
					FOR XML PATH ('')
				),1,1, ''''''
			) + ''''''
			
	PRINT 'SSN List:  ' + @SSNList		
		
	-- create sql statemen to be executed
	SELECT @SQLStatement = 
	'
		UPDATE
			NCH
		SET
			IsReconciled = 1,
			ReconciledAt = GETDATE()
		FROM
			OPENQUERY
			(
				LEGEND,
				''	
					SELECT
						AY10.BF_SSN,
						AY10.LD_ATY_REQ_RCV,
						AY10.PF_REQ_ACT,
						SUBSTR(AY20.LX_ATY, LOCATE(''''NOBLECALLHISTORYID:'''', AY20.LX_ATY) + 19, (LOCATE(''''AGENT:'''', AY20.LX_ATY) - (LOCATE(''''NOBLECALLHISTORYID:'''', AY20.LX_ATY) + 19))) AS NobleCallHistoryId
					FROM
						PKUB.AY10_BR_LON_ATY AY10
						INNER JOIN PKUB.PD10_PRS_NME PD10 ON PD10.DF_PRS_ID = AY10.BF_SSN
						INNER JOIN PKUB.AY15_ATY_CMT AY15 ON AY10.BF_SSN = AY15.BF_SSN AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
						INNER JOIN PKUB.AY20_ATY_TXT AY20 ON AY20.BF_SSN = AY15.BF_SSN AND AY20.LN_ATY_SEQ = AY15.LN_ATY_SEQ AND AY20.LN_ATY_CMT_SEQ = AY15.LN_ATY_CMT_SEQ
					WHERE
						AY10.LD_ATY_REQ_RCV > CURRENT TIMESTAMP -1 DAY
						AND
						AY20.LX_ATY LIKE ''''NOBLECALLHISTORYID:%''''
						AND
						AY10.PF_REQ_ACT IN (' + @ARCList + ')
						AND
						AY10.BF_SSN IN (' + @SSNList + ')
				''
			) L
			INNER JOIN NobleCalls..NobleCallHistory NCH ON NCH.NobleCallHistoryId = L.NobleCallHistoryId
		WHERE
			NCH.ReconciledAt is NULL
			OR
			NCH.IsReconciled is NULL
	'
	
	-- remove extra quotes from beginning of ARCList and SSNList
	SET @SQLStatement = REPLACE(@SQLStatement, ''''''',', '')
	PRINT @SQLStatement
	EXEC (@SQLStatement)
	
	SET @RowCount = @@ROWCOUNT
	PRINT 'CornerStone SLQStatement Length:  ' + CAST(LEN(@SQLStatement) as VARCHAR(6))
	PRINT 'CornerStone CycleCount:  ' + CAST(@CycleCount as VARCHAR(2))
	PRINT 'CornerStone RowCount:  ' + CAST(@RowCount as VARCHAR(6))
END


PRINT 'UHEAA'
-- reset tracking variables
SET @RowCount = 1
SET @CycleCount = 0

-- create a comma delimited list of ARCs
SELECT @ARCList = 
	STUFF(
			(
				SELECT 
					''''',''''' + ARC AS [text()] 
				FROM   
				(
					SELECT DISTINCT 
						ARC 
					FROM 
						NobleCalls..Arcs
				) X
				FOR XML PATH ('')
			),1,1, ''''''
		) + ''''''

PRINT 'ARC List:  ' + @ARCList

-- do nothing if no rows were updated during the last cycle or if 10 cycles of updates have already been processed
WHILE @RowCount > 0 AND @CycleCount <= 10
BEGIN
	-- limit cycles to 10
	SET @CycleCount = @CycleCount + 1

	-- create a comma delimited list of SSNs
	SELECT @SSNList = 
		STUFF(
				(
					SELECT DISTINCT
						''''',''''' + SSN
					FROM   
						(	-- limited to 480 SSNs to avoid going over the 8000 character limit imposed by OPENQUERY
							SELECT TOP 480 
								COALESCE(BSSN.BF_SSN, BACT.BF_SSN) [SSN],
								NCH.CreatedAt
							FROM
								NobleCalls..NobleCallHistory NCH
								LEFT JOIN UDW..PD10_Borrower BSSN on BSSN.BF_SSN = NCH.AccountIdentifier
								LEFT JOIN UDW..PD10_Borrower BACT on BACT.DF_SPE_ACC_ID = NCH.AccountIdentifier
							WHERE
								NCH.CallCampaign != 'VABU'
								AND
								COALESCE(BSSN.BF_SSN, BACT.BF_SSN) IS NOT NULL
								AND
								NCH.ReconciledAt IS NULL
								AND
								NCH.DeletedAt IS NULL
								AND
								NCH.ArcAddProcessingId IS NOT NULL
								AND
								NCH.CreatedAt < DATEADD(HOUR, -4, GETDATE()) -- don't try to reconcile anything added within the last 4 hours
							ORDER BY
								NCH.CreatedAt DESC
						) X
					FOR XML PATH ('')
				),1,1, ''''''
			) + ''''''
			
	PRINT 'SSN List:  ' + @SSNList		
		
	-- create sql statemen to be executed
	SELECT @SQLStatement = 
	'
		UPDATE
			NCH
		SET
			IsReconciled = 1,
			ReconciledAt = GETDATE()
		FROM
			OPENQUERY
			(
				DUSTER,
				''	
					SELECT
						AY10.BF_SSN,
						AY10.LD_ATY_REQ_RCV,
						AY10.PF_REQ_ACT,
						SUBSTR(AY20.LX_ATY, LOCATE(''''NOBLECALLHISTORYID:'''', AY20.LX_ATY) + 19, (LOCATE(''''AGENT:'''', AY20.LX_ATY) - (LOCATE(''''NOBLECALLHISTORYID:'''', AY20.LX_ATY) + 19))) AS NobleCallHistoryId
					FROM
						OLWHRM1.AY10_BR_LON_ATY AY10
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10 ON PD10.DF_PRS_ID = AY10.BF_SSN
						INNER JOIN OLWHRM1.AY15_ATY_CMT AY15 ON AY10.BF_SSN = AY15.BF_SSN AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
						INNER JOIN OLWHRM1.AY20_ATY_TXT AY20 ON AY20.BF_SSN = AY15.BF_SSN AND AY20.LN_ATY_SEQ = AY15.LN_ATY_SEQ AND AY20.LN_ATY_CMT_SEQ = AY15.LN_ATY_CMT_SEQ
					WHERE
						AY10.LD_ATY_REQ_RCV > CURRENT TIMESTAMP -1 DAY
						AND
						AY20.LX_ATY LIKE ''''NOBLECALLHISTORYID:%''''
						AND
						AY10.PF_REQ_ACT IN (' + @ARCList + ') 
						AND 
						AY10.BF_SSN IN (' + @SSNList + ')
				''
			) L
			INNER JOIN NobleCalls..NobleCallHistory NCH ON NCH.NobleCallHistoryId = L.NobleCallHistoryId
		WHERE
			NCH.ReconciledAt is NULL
			OR
			NCH.IsReconciled is NULL
	'
	
	-- remove extra quotes from beginning of ARCList and SSNList
	SET @SQLStatement = REPLACE(@SQLStatement, ''''''',', '')
	PRINT @SQLStatement
	EXEC (@SQLStatement)
	
	SET @RowCount = @@ROWCOUNT
	PRINT 'UHEAA SLQStatement Length:  ' + CAST(LEN(@SQLStatement) as VARCHAR(6))
	PRINT 'UHEAA CycleCount:  ' + CAST(@CycleCount as VARCHAR(2))
	PRINT 'UHEAA RowCount:  ' + CAST(@RowCount as VARCHAR(6))
END