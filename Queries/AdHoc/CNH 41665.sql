USE CDW
GO

SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	LNXX.LN_SEQ AS LOAN_SEQ,
	LNXX.LA_CUR_PRI AS CURRENT_PRINCIPAL,
	LNXX.DSB_AMT,
	LNXX.LF_DOE_SCL_ORG AS ORIGINAL_SCHOOL_CODE,
	ISNULL(AYXXC.LX_ATY,'') AS COMMENT
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = PDXX.DF_PRS_ID
		AND AYXX.PF_REQ_ACT = 'DICSK'
		AND AYXX.LC_STA_ACTYXX = 'A'
	
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN 
	(
		SELECT 
			BF_SSN,
			LN_SEQ,
			SUM(LA_DSB - ISNULL(LA_DSB_CAN,X)) AS DSB_AMT
		FROM 
			CDW..LNXX_DSB
		WHERE
			LC_STA_LONXX = 'X'
		GROUP BY
			BF_SSN,
		LN_SEQ
	) LNXX
		 ON LNXX.BF_SSN = LNXX.BF_SSN
		 AND LNXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			AYXX.BF_SSN,
			STUFF(
			(
				SELECT 
						' ' + SUB.LX_ATY AS [text()]
				FROM 
					CDW..AYXX_ATY_TXT SUB
				WHERE
					SUB.BF_SSN = AYXX.BF_SSN
					AND SUB.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
			FOR XML PATH('')
			)
			,X,X, '') AS LX_ATY
		
			FROM	
			CDW..AYXX_BR_LON_ATY AYXX
			left JOIN CDW..AYXX_ATY_TXT AYXX
				ON AYXX.BF_SSN = AYXX.BF_SSN
				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
			left JOIN CDW..PDXX_PRS_NME PDXX
				ON PDXX.DF_PRS_ID = AYXX.BF_SSN
			WHERE
			AYXX.PF_REQ_ACT = 'DICSK'
			AND AYXX.LC_STA_ACTYXX = 'A'
		) AYXXC
			ON AYXXC.BF_SSN = PDXX.DF_PRS_ID
WHERE
	LNXX.LF_DOE_SCL_ORG IN 
	(
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX',
	'XXXXXXXX'
	)
ORDER BY
	ACCOUNT_NUMBER,
	LOAN_SEQ


	SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	LNXX.LN_SEQ AS LOAN_SEQ,
	LNXX.LA_CUR_PRI AS CURRENT_PRINCIPAL,
	LNXX.DSB_AMT,
	LNXX.LF_DOE_SCL_ORG AS ORIGINAL_SCHOOL_CODE,
	SCXX.IC_SCL_DOM_ST AS ST,
	SCXX.IC_CUR_SCL_STA,
	MAX(AYXX.LD_ATY_REQ_RCV) OVER (PARTITION BY AYXX.BF_SSN, LNXX.LN_SEQ) AS DICSK_ARC_DATE,
	CASE WHEN AYXXX.PF_REQ_ACT = 'CSFSA' THEN 'Y' ELSE 'N' END AS HAS_CSFSA,
	CASE WHEN AYXXX.PF_REQ_ACT = 'ADCSH' THEN 'Y' ELSE 'N' END AS HAS_ADCSH,
	CASE WHEN AYXXX.PF_REQ_ACT = 'CSDNY' THEN 'Y' ELSE 'N' END AS HAS_CSDNY 

FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = PDXX.DF_PRS_ID
		AND AYXX.PF_REQ_ACT IN ('DICSK','DRDCR','ADCSH')
		AND AYXX.LC_STA_ACTYXX = 'A'
		and ayXX.LD_ATY_REQ_RCV >= 'X/XX/XXXX'
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN 
	(
		SELECT 
			BF_SSN,
			LN_SEQ,
			SUM(LA_DSB - ISNULL(LA_DSB_CAN,X)) AS DSB_AMT
		FROM 
			CDW..LNXX_DSB
		WHERE
			LC_STA_LONXX = 'X'
		GROUP BY
			BF_SSN,
		LN_SEQ
	) LNXX
		 ON LNXX.BF_SSN = LNXX.BF_SSN
		 AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..SCXX_SCH_DMO SCXX
		ON SCXX.IF_DOE_SCL = LNXX.LF_DOE_SCL_ORG
	LEFT JOIN CDW..AYXX_BR_LON_ATY AYXXX
		ON AYXXX.BF_SSN = PDXX.DF_PRS_ID
		AND AYXXX.PF_REQ_ACT IN ( 'CSFSA', 'ADCSH','CSDNY')
		AND AYXXX.LC_STA_ACTYXX = 'A'
		AND AYXXX.LD_ATY_REQ_RCV >= 'XX/XX/XXXX'
	LEFT JOIN CDW..SCXX_SCH_DPT SCXX
		ON SCXX.IF_DOE_SCL = SCXX.IF_DOE_SCL

ORDER BY
	ACCOUNT_NUMBER,
	LOAN_SEQ
