SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
USE CDW
GO

IF OBJECT_ID('tempdb..#BASE_POP') IS NOT NULL 
	DROP TABLE #BASE_POP

IF OBJECT_ID('tempdb..#COMMENTSUMS') IS NOT NULL 
	DROP TABLE #COMMENTSUMS

DECLARE @YESTERDAY DATE = DATEADD(DAY,-1,GETDATE())

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID
	,LN10.BF_SSN
	,LN90.LN_SEQ
	,LN90.LN_FAT_SEQ
	,LN90.LD_FAT_EFF
	,LN90.LA_FAT_NSI
	,LN90.LA_FAT_CUR_PRI
	,LN90.LA_FAT_LTE_FEE
	,LN90.PC_FAT_TYP
	,LN90.PC_FAT_SUB_TYP
	,COALESCE(LN90.LD_FAT_PST, LN90.LD_FAT_APL) AS LD_FAT_PST
	,AY10.PF_REQ_ACT
INTO
	#BASE_POP
FROM
	LN10_LON LN10
	INNER JOIN PD10_PRS_NME PD10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN LN90_FIN_ATY LN90
		ON LN10.BF_SSN = LN90.BF_SSN
	INNER JOIN 
	(
		SELECT
			BF_SSN
			,PF_REQ_ACT
			,MAX(LD_ATY_REQ_RCV) AS MAX_LD_ATY_REQ_RCV
		FROM
			AY10_BR_LON_ATY 
		WHERE
			PF_REQ_ACT IN ('APPMT', 'APMIL', 'APAMC', 'APACH')
			AND LC_STA_ACTY10 = 'A'
		GROUP BY
			BF_SSN
			,PF_REQ_ACT
	) AY10
		ON LN90.BF_SSN = AY10.BF_SSN
	LEFT JOIN
	(--flagged for exclusion: adjustment cancellation arc
		SELECT
			BF_SSN
			,MAX(LD_ATY_REQ_RCV) AS MAX_LD_ATY_REQ_RCV
		FROM
			AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT = 'CXPAJ'
			AND LC_STA_ACTY10 = 'A'
		GROUP BY
			BF_SSN
	) CXPAJ
		ON CXPAJ.BF_SSN = LN90.BF_SSN
		AND AY10.MAX_LD_ATY_REQ_RCV < CXPAJ.MAX_LD_ATY_REQ_RCV--CXPAJ arc date created after adjustment arc
WHERE
	CXPAJ.BF_SSN IS NULL --excludes
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND LN90.LC_STA_LON90 = 'A' --active
	AND ISNULL(LN90.LC_FAT_REV_REA,'') = '' --non-reversed
	AND LN90.LD_FAT_APL = @YESTERDAY
;

--select * from #BASE_POP order by DF_SPE_ACC_ID,LN_SEQ,LN_FAT_SEQ --TEST

/******** prepare fields for report comments ********/

SELECT DISTINCT
	DF_SPE_ACC_ID
	,FORMAT(LD_FAT_EFF, 'MM/dd/yy', 'en-US') AS PaymentEffectiveDate
	,CONCAT(PC_FAT_TYP,PC_FAT_SUB_TYP) AS [Type]
	,CAST(ABS(SUM(COALESCE(LA_FAT_NSI,0) + COALESCE(LA_FAT_CUR_PRI,0) + COALESCE(LA_FAT_LTE_FEE,0))) AS VARCHAR(10)) AS Amount
	,LD_FAT_EFF
	,PC_FAT_TYP
	,PC_FAT_SUB_TYP
	,PF_REQ_ACT
INTO
	#COMMENTSUMS
FROM
	#BASE_POP
GROUP BY
	DF_SPE_ACC_ID
	,LD_FAT_EFF
	,PC_FAT_TYP
	,PC_FAT_SUB_TYP
	,PF_REQ_ACT
;

--select * from #commentsums --TEST

/********************* R2 ***************************/

MERGE
	CLS.dbo.ArcAddProcessing AAP
USING
(
	SELECT DISTINCT
		2 AS ArcTypeId
		,BP.DF_SPE_ACC_ID AS AccountNumber
		,'PMTAJ' AS ARC
		,'UTNWF07' AS ScriptId
		,GETDATE() AS ProcessOn
		,'See instruction to reapply payment in ARC ' + BP.PF_REQ_ACT + '.  Payment Effect Date = ' + CS.PaymentEffectiveDate + '; Amount = ' + CS.Amount + '; Type = ' + CS.[Type] + '.' AS Comment
		,0 AS IsReference
		,0 AS IsEndorser
		,0 AS ProcessingAttempts
		,GETDATE() AS CreatedAt
		,SYSTEM_USER AS CreatedBy
		,NULL AS ProcessedAt
	FROM
		#BASE_POP BP
		INNER JOIN #COMMENTSUMS CS
			ON BP.DF_SPE_ACC_ID = CS.DF_SPE_ACC_ID
			AND BP.LD_FAT_EFF = CS.LD_FAT_EFF
			AND BP.PC_FAT_TYP = CS.PC_FAT_TYP
			AND BP.PC_FAT_SUB_TYP = CS.PC_FAT_SUB_TYP
			AND BP.PF_REQ_ACT = CS.PF_REQ_ACT
		LEFT JOIN 
		(--flagged for exclusion: autopay
			SELECT
				BF_SSN
				,LN_SEQ
				,LN_FAT_SEQ
			FROM
				LN94_LON_PAY_FAT
			WHERE
				LC_RMT_BCH_SRC_IPT = 'E' --autopay
		) LN94
			ON  BP.BF_SSN = LN94.BF_SSN
			AND BP.LN_SEQ = LN94.LN_SEQ
			AND BP.LN_FAT_SEQ = LN94.LN_FAT_SEQ
		LEFT JOIN AY10_BR_LON_ATY PMTAD
			ON PMTAD.BF_SSN = BP.BF_SSN
			AND PMTAD.PF_REQ_ACT = 'PMTAD'
			AND PMTAD.LD_ATY_REQ_RCV = BP.LD_FAT_PST --PMTAD arc date equals payment post date
			AND PMTAD.LC_STA_ACTY10 = 'A'
	WHERE
		PMTAD.BF_SSN IS NULL --excludes
		AND LN94.BF_SSN IS NULL --exclude autopay
		AND BP.PF_REQ_ACT = 'APPMT'
		AND BP.PC_FAT_TYP = '10'
		AND BP.PC_FAT_SUB_TYP IN ('10','11','12','41')
) NEWDATA
	ON NEWDATA.ArcTypeId = AAP.ArcTypeId
	AND NEWDATA.AccountNumber = AAP.AccountNumber
	AND NEWDATA.ARC = AAP.ARC
	AND NEWDATA.ScriptId = AAP.ScriptId
	AND CAST(NEWDATA.ProcessOn AS DATE) = CAST(AAP.ProcessOn AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.Comment = COALESCE(AAP.Comment,'') --nullable
	AND NEWDATA.IsReference = AAP.IsReference
	AND NEWDATA.IsEndorser = AAP.IsEndorser
	AND NEWDATA.ProcessingAttempts = AAP.ProcessingAttempts
	AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(AAP.CreatedAt AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.CreatedBy = AAP.CreatedBy
	AND COALESCE(NEWDATA.ProcessedAt,'') = COALESCE(AAP.ProcessedAt,'') --nullable
WHEN NOT MATCHED THEN
	INSERT
	(
		 ArcTypeId
		,AccountNumber
		,ARC
		,ScriptId
		,ProcessOn
		,Comment
		,IsReference
		,IsEndorser
		,ProcessingAttempts
		,CreatedAt
		,CreatedBy
		,ProcessedAt
	)
	VALUES
	(
		 NEWDATA.ArcTypeId
		,NEWDATA.AccountNumber
		,NEWDATA.ARC
		,NEWDATA.ScriptId
		,NEWDATA.ProcessOn
		,NEWDATA.Comment
		,NEWDATA.IsReference
		,NEWDATA.IsEndorser
		,NEWDATA.ProcessingAttempts
		,NEWDATA.CreatedAt
		,NEWDATA.CreatedBy
		,NEWDATA.ProcessedAt
	)
;

/********************* R3 ***************************/

MERGE
	CLS.dbo.ArcAddProcessing AAP
USING
(
	SELECT DISTINCT
		2 AS ArcTypeId
		,BP.DF_SPE_ACC_ID AS AccountNumber
		,'PMTAJ' AS ARC
		,'UTNWF07' AS ScriptId
		,GETDATE() AS ProcessOn
		,'See instruction to reapply payment in ARC ' + BP.PF_REQ_ACT + '.  Payment Effect Date = ' + CS.PaymentEffectiveDate + '; Amount = ' + CS.Amount + '; Type = ' + CS.[Type] + '.' AS Comment
		,0 AS IsReference
		,0 AS IsEndorser
		,0 AS ProcessingAttempts
		,GETDATE() AS CreatedAt
		,SYSTEM_USER AS CreatedBy
		,NULL AS ProcessedAt
	FROM
		#BASE_POP BP
		INNER JOIN #COMMENTSUMS CS
			ON BP.DF_SPE_ACC_ID = CS.DF_SPE_ACC_ID
			AND BP.LD_FAT_EFF = CS.LD_FAT_EFF
			AND BP.PC_FAT_TYP = CS.PC_FAT_TYP
			AND BP.PC_FAT_SUB_TYP = CS.PC_FAT_SUB_TYP
			AND BP.PF_REQ_ACT = CS.PF_REQ_ACT
		LEFT JOIN AY10_BR_LON_ATY PMTML
			ON PMTML.BF_SSN = BP.BF_SSN
			AND PMTML.PF_REQ_ACT = 'PMTML'
			AND PMTML.LD_ATY_REQ_RCV = BP.LD_FAT_PST --PMTML arc date equals payment post date
			AND PMTML.LC_STA_ACTY10 = 'A'
	WHERE
		PMTML.BF_SSN IS NULL --excludes
		AND BP.PF_REQ_ACT = 'APMIL'
		AND BP.PC_FAT_TYP = '10'
		AND BP.PC_FAT_SUB_TYP = '35'
) NEWDATA
	ON NEWDATA.ArcTypeId = AAP.ArcTypeId
	AND NEWDATA.AccountNumber = AAP.AccountNumber
	AND NEWDATA.ARC = AAP.ARC
	AND NEWDATA.ScriptId = AAP.ScriptId
	AND CAST(NEWDATA.ProcessOn AS DATE) = CAST(AAP.ProcessOn AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.Comment = COALESCE(AAP.Comment,'') --nullable
	AND NEWDATA.IsReference = AAP.IsReference
	AND NEWDATA.IsEndorser = AAP.IsEndorser
	AND NEWDATA.ProcessingAttempts = AAP.ProcessingAttempts
	AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(AAP.CreatedAt AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.CreatedBy = AAP.CreatedBy
	AND COALESCE(NEWDATA.ProcessedAt,'') = COALESCE(AAP.ProcessedAt,'') --nullable
WHEN NOT MATCHED THEN
	INSERT
	(
		 ArcTypeId
		,AccountNumber
		,ARC
		,ScriptId
		,ProcessOn
		,Comment
		,IsReference
		,IsEndorser
		,ProcessingAttempts
		,CreatedAt
		,CreatedBy
		,ProcessedAt
	)
	VALUES
	(
		 NEWDATA.ArcTypeId
		,NEWDATA.AccountNumber
		,NEWDATA.ARC
		,NEWDATA.ScriptId
		,NEWDATA.ProcessOn
		,NEWDATA.Comment
		,NEWDATA.IsReference
		,NEWDATA.IsEndorser
		,NEWDATA.ProcessingAttempts
		,NEWDATA.CreatedAt
		,NEWDATA.CreatedBy
		,NEWDATA.ProcessedAt
	)
;

/********************* R4 ***************************/

MERGE
	CLS.dbo.ArcAddProcessing AAP
USING
(
	SELECT DISTINCT
		2 AS ArcTypeId
		,BP.DF_SPE_ACC_ID AS AccountNumber
		,'PMTAJ' AS ARC
		,'UTNWF07' AS ScriptId
		,GETDATE() AS ProcessOn
		,'See instruction to reapply payment in ARC ' + BP.PF_REQ_ACT + '.  Payment Effect Date = ' + CS.PaymentEffectiveDate + '; Amount = ' + CS.Amount + '; Type = ' + CS.[Type] + '.' AS Comment
		,0 AS IsReference
		,0 AS IsEndorser
		,0 AS ProcessingAttempts
		,GETDATE() AS CreatedAt
		,SYSTEM_USER AS CreatedBy
		,NULL AS ProcessedAt
	FROM
		#BASE_POP BP
		INNER JOIN #COMMENTSUMS CS
			ON BP.DF_SPE_ACC_ID = CS.DF_SPE_ACC_ID
			AND BP.LD_FAT_EFF = CS.LD_FAT_EFF
			AND BP.PC_FAT_TYP = CS.PC_FAT_TYP
			AND BP.PC_FAT_SUB_TYP = CS.PC_FAT_SUB_TYP
			AND BP.PF_REQ_ACT = CS.PF_REQ_ACT
		LEFT JOIN AY10_BR_LON_ATY PMTAM
			ON PMTAM.BF_SSN = BP.BF_SSN
			AND PMTAM.PF_REQ_ACT = 'PMTAM'
			AND PMTAM.LD_ATY_REQ_RCV = BP.LD_FAT_PST --PMTAM arc date equals payment post date
			AND PMTAM.LC_STA_ACTY10 = 'A'
	WHERE
		PMTAM.BF_SSN IS NULL --excludes
		AND BP.PF_REQ_ACT = 'APAMC'
		AND BP.PC_FAT_TYP = '10'
		AND BP.PC_FAT_SUB_TYP IN ('20','21','22','36','37')
) NEWDATA
	ON NEWDATA.ArcTypeId = AAP.ArcTypeId
	AND NEWDATA.AccountNumber = AAP.AccountNumber
	AND NEWDATA.ARC = AAP.ARC
	AND NEWDATA.ScriptId = AAP.ScriptId
	AND CAST(NEWDATA.ProcessOn AS DATE) = CAST(AAP.ProcessOn AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.Comment = COALESCE(AAP.Comment,'') --nullable
	AND NEWDATA.IsReference = AAP.IsReference
	AND NEWDATA.IsEndorser = AAP.IsEndorser
	AND NEWDATA.ProcessingAttempts = AAP.ProcessingAttempts
	AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(AAP.CreatedAt AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.CreatedBy = AAP.CreatedBy
	AND COALESCE(NEWDATA.ProcessedAt,'') = COALESCE(AAP.ProcessedAt,'') --nullable
WHEN NOT MATCHED THEN
	INSERT
	(
		 ArcTypeId
		,AccountNumber
		,ARC
		,ScriptId
		,ProcessOn
		,Comment
		,IsReference
		,IsEndorser
		,ProcessingAttempts
		,CreatedAt
		,CreatedBy
		,ProcessedAt
	)
	VALUES
	(
		 NEWDATA.ArcTypeId
		,NEWDATA.AccountNumber
		,NEWDATA.ARC
		,NEWDATA.ScriptId
		,NEWDATA.ProcessOn
		,NEWDATA.Comment
		,NEWDATA.IsReference
		,NEWDATA.IsEndorser
		,NEWDATA.ProcessingAttempts
		,NEWDATA.CreatedAt
		,NEWDATA.CreatedBy
		,NEWDATA.ProcessedAt
	)
;

/********************* R5 ***************************/

MERGE
	CLS.dbo.ArcAddProcessing AAP
USING
(
	SELECT DISTINCT
		2 AS ArcTypeId
		,BP.DF_SPE_ACC_ID AS AccountNumber
		,'PMTAJ' AS ARC
		,'UTNWF07' AS ScriptId
		,GETDATE() AS ProcessOn
		,'See instruction to reapply payment in ARC ' + BP.PF_REQ_ACT + '.  Payment Effect Date = ' + CS.PaymentEffectiveDate + '; Amount = ' + CS.Amount + '; Type = ' + CS.[Type] + '.' AS Comment
		,0 AS IsReference
		,0 AS IsEndorser
		,0 AS ProcessingAttempts
		,GETDATE() AS CreatedAt
		,SYSTEM_USER AS CreatedBy
		,NULL AS ProcessedAt
	FROM
		#BASE_POP BP
		INNER JOIN #COMMENTSUMS CS
			ON BP.DF_SPE_ACC_ID = CS.DF_SPE_ACC_ID
			AND BP.LD_FAT_EFF = CS.LD_FAT_EFF
			AND BP.PC_FAT_TYP = CS.PC_FAT_TYP
			AND BP.PC_FAT_SUB_TYP = CS.PC_FAT_SUB_TYP
			AND BP.PF_REQ_ACT = CS.PF_REQ_ACT
		INNER JOIN LN94_LON_PAY_FAT LN94
			ON  BP.BF_SSN = LN94.BF_SSN
			AND BP.LN_SEQ = LN94.LN_SEQ
			AND BP.LN_FAT_SEQ = LN94.LN_FAT_SEQ
		LEFT JOIN AY10_BR_LON_ATY PMTAD
			ON PMTAD.BF_SSN = BP.BF_SSN
			AND PMTAD.PF_REQ_ACT = 'PMTAD'
			AND PMTAD.LD_ATY_REQ_RCV = BP.LD_FAT_PST --PMTAD arc date equals payment post date
			AND PMTAD.LC_STA_ACTY10 = 'A'
	WHERE
		PMTAD.BF_SSN IS NULL --excludes
		AND	BP.PF_REQ_ACT = 'APACH'
		AND BP.PC_FAT_TYP = '10'
		AND BP.PC_FAT_SUB_TYP = '10'
		AND LN94.LC_RMT_BCH_SRC_IPT = 'E'
) NEWDATA
	ON NEWDATA.ArcTypeId = AAP.ArcTypeId
	AND NEWDATA.AccountNumber = AAP.AccountNumber
	AND NEWDATA.ARC = AAP.ARC
	AND NEWDATA.ScriptId = AAP.ScriptId
	AND CAST(NEWDATA.ProcessOn AS DATE) = CAST(AAP.ProcessOn AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.Comment = COALESCE(AAP.Comment,'') --nullable
	AND NEWDATA.IsReference = AAP.IsReference
	AND NEWDATA.IsEndorser = AAP.IsEndorser
	AND NEWDATA.ProcessingAttempts = AAP.ProcessingAttempts
	AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(AAP.CreatedAt AS DATE) --allows multiple runs per day w/o duplicating data
	AND NEWDATA.CreatedBy = AAP.CreatedBy
	AND COALESCE(NEWDATA.ProcessedAt,'') = COALESCE(AAP.ProcessedAt,'') --nullable
WHEN NOT MATCHED THEN
	INSERT
	(
		 ArcTypeId
		,AccountNumber
		,ARC
		,ScriptId
		,ProcessOn
		,Comment
		,IsReference
		,IsEndorser
		,ProcessingAttempts
		,CreatedAt
		,CreatedBy
		,ProcessedAt
	)
	VALUES
	(
		 NEWDATA.ArcTypeId
		,NEWDATA.AccountNumber
		,NEWDATA.ARC
		,NEWDATA.ScriptId
		,NEWDATA.ProcessOn
		,NEWDATA.Comment
		,NEWDATA.IsReference
		,NEWDATA.IsEndorser
		,NEWDATA.ProcessingAttempts
		,NEWDATA.CreatedAt
		,NEWDATA.CreatedBy
		,NEWDATA.ProcessedAt
	)
;

--TEST:
--select 'r2'[r2],* from CLS..ArcAddProcessing where comment like '%APPMT%'
--select 'r3'[r3],* from CLS..ArcAddProcessing where comment like '%APMIL%'
--select 'r4'[r4],* from CLS..ArcAddProcessing where comment like '%APAMC%'
--select 'r5'[r5],* from CLS..ArcAddProcessing where comment like '%APACH%'

--delete from CLS..ArcAddProcessing where ScriptId = 'UTNWF07'


