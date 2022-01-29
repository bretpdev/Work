USE UDW
GO
DROP TABLE IF EXISTS #BASE_POP

SELECT DISTINCT
	AY10.LF_ATY_RCP AS ENDORSER_COMAKER,
	LT20.RF_SBJ_PRC AS BORROWER,
	dd.LD_DLQ_OCC AS ORIG_DATE_OF_DELQ,
	CASE 
		WHEN LT20.RM_DSC_LTR_PRC = 'US09B10CP' THEN DATEADD(DAY, 10, DD.LD_DLQ_OCC) 
		WHEN LT20.RM_DSC_LTR_PRC = 'US09B110CP' THEN DATEADD(DAY, 10, DD.LD_DLQ_OCC) 
		WHEN LT20.RM_DSC_LTR_PRC = 'US09BFDCP' THEN DATEADD(DAY, 10, DD.LD_DLQ_OCC) 
		ELSE NULL
	END AS [10_DAY_DATE],
	CASE 
		WHEN LT20.RM_DSC_LTR_PRC = 'US09B110CP' THEN DATEADD(DAY, 110, DD.LD_DLQ_OCC) 
		WHEN LT20.RM_DSC_LTR_PRC = 'US09BFDCP' THEN DATEADD(DAY, 110, DD.LD_DLQ_OCC)
	END AS [110_DAY_DATE],
	CASE	
		WHEN LT20.RM_DSC_LTR_PRC = 'US09BFDCP' THEN CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
	END AS FINAL_DATE,
	LT2010.RT_RUN_SRT_DTS_PRC AS [10_LETTER_SENT_DATE],
	LT20110.RT_RUN_SRT_DTS_PRC AS [110_LETTER_SENT_DATE],
	CASE	
		WHEN LT20.RM_DSC_LTR_PRC = 'US09BFDCP' THEN LT20.RT_RUN_SRT_DTS_PRC 
	END FINAL_LETTER_SENT_DATE
INTO #BASE_POP
FROM
	UDW..LT20_LTR_REQ_PRC LT20
	INNER JOIN
	(
		SELECT
			AY10.*,
			LN85.LN_SEQ
		FROM
			UDW..AY10_BR_LON_ATY AY10
			INNER JOIN udw..LN85_LON_ATY LN85
				ON LN85.BF_SSN = AY10.BF_SSN
				AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
		WHERE
			PF_REQ_ACT IN ('DL101','DL911','DLFD1')
			AND LD_ATY_REQ_RCV BETWEEN '02/01/2021' AND '04/26/2021'
	)AY10
		ON AY10.BF_SSN = LT20.RF_SBJ_PRC
		and AY10.LN_ATY_SEQ = LT20.RN_ATY_SEQ_PRC
	LEFT JOIN UDW.calc.DailyDelinquency DD
		ON DD.BF_SSN = LT20.RF_SBJ_PRC
		AND DD.LN_SEQ = AY10.LN_SEQ
		AND DD.AddedAt = CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
	LEFT JOIN UDW..LT20_LTR_REQ_PRC LT2010	
		ON LT2010.RF_SBJ_PRC = LT20.RF_SBJ_PRC
		AND LT2010.RM_DSC_LTR_PRC = 'US09B10CP'
		AND LT2010.CreatedAt BETWEEN DATEADD(DAY, 7, DD.LD_DLQ_OCC) AND DATEADD(DAY, 13, DD.LD_DLQ_OCC)
	LEFT JOIN UDW..LT20_LTR_REQ_PRC LT20110	
		ON LT20110.RF_SBJ_PRC = LT20.RF_SBJ_PRC
		AND LT20110.RM_DSC_LTR_PRC = 'US09B110CP'
		AND LT20110.CreatedAt BETWEEN DATEADD(DAY, 100, DD.LD_DLQ_OCC) AND DATEADD(DAY, 130, DD.LD_DLQ_OCC)

WHERE
	LT20.RM_DSC_LTR_PRC IN ('US09B10CP','US09B110CP','US09BFDCP')
	AND LT20.CreatedAt BETWEEN '03/01/2021' AND '04/26/2021'
	AND LT20.SystemLetterExclusionReasonId IS NULL
	
SELECT DISTINCT * FROM #BASE_POP 