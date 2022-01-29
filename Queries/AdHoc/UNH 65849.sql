 drop table if exists #data 

SELECT DISTINCT 
	PD10.DF_SPE_ACC_ID AS [Account Number],
	LN90.LN_SEQ AS [Loan Seq],
	--ISNULL(LN10.LD_END_GRC_PRD, LD_LON_1_DSB) AS [DATE_ENTERED_REPAYMENT],
	--MAX(ISNULL(LN10.LD_PIF_RPT, CAST(LN90.LD_FAT_EFF AS DATE))) OVER (PARTITION BY LN90.BF_SSN, LN90.LN_SEQ) AS CONSOL_DATE,
	DATEDIFF(YEAR, ISNULL(LN10.LD_END_GRC_PRD, LD_LON_1_DSB), ISNULL(LN10.LD_PIF_RPT, CAST(LN90.LD_FAT_EFF AS DATE))) AS [Years In Repayment],
	MAX(MONTH(ISNULL(LN10.LD_PIF_RPT, CAST(LN90.LD_FAT_EFF AS DATE)))) OVER (PARTITION BY LN90.BF_SSN, LN90.LN_SEQ) AS [Months In Repayment],
	FORMAT(ISNULL(PAYMENT.AVG_PAYMENT_AMT,0.00), 'C') AS [Avgerage Payment Amount],
	MAX(LN72.LR_ITR) OVER (PARTITION BY LN90.BF_SSN, LN90.LN_SEQ) AS [Interest Rate At Repayment], 
	ISNULL(FORB.MONTHS_IN_FORB,0) AS [# Of Months In Forb],
	CASE	
		WHEN LN10.LF_LON_CUR_OWN IN ('829769','82976901','82976902','82976903','82976904','82976905','82976906','82976907','82976908') THEN 'Y'
		ELSE 'N'
	END	 AS BANA,
	LN10.LF_DOE_SCL_ORG as [School Code At Origination],
	SUM(ABS(LN90.LA_FAT_CUR_PRI)) OVER (PARTITION BY LN90.BF_SSN, LN90.LN_SEQ) AS PRINCIPAL_CONSOLIDATED,
	SUM(ABS(LN90.LA_FAT_NSI)) OVER (PARTITION BY LN90.BF_SSN, LN90.LN_SEQ) AS INTEREST_CONSOLIDATED
into #data
FROM 
	UDW..LN90_FIN_ATY  LN90
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN90.BF_SSN
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = LN90.BF_SSN
		AND LN10.LN_SEQ = LN90.LN_SEQ
	INNER JOIN
	(	--Adding Active Interest Rate
		SELECT
			LN72.BF_SSN, 
			LN72.LN_SEQ,
			LN72.LR_ITR,
			LN72.LD_ITR_EFF_BEG,
			LN72.LD_ITR_EFF_END
		FROM
			UDW..LN72_INT_RTE_HST LN72
		WHERE
			LN72.LC_STA_LON72 = 'A'
	) LN72 
		ON LN10.BF_SSN = LN72.BF_SSN
		AND LN10.LN_SEQ = LN72.LN_SEQ
		AND  ISNULL(LN10.LD_END_GRC_PRD, LD_LON_1_DSB) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
	LEFT JOIN
	(
		SELECT
			LN65.BF_SSN,
			LN65.LN_SEQ,
			AVG(LA_RPS_ISL) AS AVG_PAYMENT_AMT
		FROM
			UDW..LN65_LON_RPS LN65
			INNER JOIN UDW..LN66_LON_RPS_SPF LN66
				ON LN66.BF_SSN = LN65.BF_SSN
				AND LN66.LN_SEQ = LN65.LN_SEQ
				AND LN66.LN_RPS_SEQ = LN65.LN_RPS_SEQ
		WHERE  
			LN_RPS_TRM > 1 --EXCLUDE THE PAST PAYMENT
		GROUP BY 
			LN65.BF_SSN,
			LN65.LN_SEQ
	) PAYMENT
		ON PAYMENT.BF_SSN = LN10.BF_SSN
		AND PAYMENT.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT 
			LN60.BF_SSN,
			LN60.LN_SEQ,
			SUM(DATEDIFF(MONTH, LN60.LD_FOR_BEG, LN60.LD_FOR_END)) AS MONTHS_IN_FORB
		FROM
			UDW..FB10_BR_FOR_REQ FB10
			INNER JOIN UDW..LN60_BR_FOR_APV LN60
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			FB10.LC_STA_FOR10 = 'A'
			AND FB10.LC_FOR_STA = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND FB10.LC_FOR_TYP = '05'
		GROUP BY
			LN60.BF_SSN,
			LN60.LN_SEQ
	) FORB
		ON FORB.BF_SSN = LN10.BF_SSN
		AND FORB.LN_SEQ = LN10.LN_SEQ

WHERE 
	LN90.PC_FAT_TYP = '10'
	AND LN90.PC_FAT_SUB_TYP IN ('70','80')	
	AND LN90.LD_FAT_EFF >= '01/01/2015'
	AND LN90.LC_STA_LON90 = 'A'
	AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''

ORDER BY
	PD10.DF_SPE_ACC_ID,
	LN90.LN_SEQ

SELECT
	*
FROM
	#DATA

SELECT
	COUNT(*) AS [Loan Count],
	COUNT(DISTINCT  [Account Number]) AS [Borrower Count],
	FORMAT(SUM(PRINCIPAL_CONSOLIDATED), 'C') as [Principal Consolidated],
	FORMAT(SUM(INTEREST_CONSOLIDATED), 'C') AS [Interest Consolidated]
FROM
	#data