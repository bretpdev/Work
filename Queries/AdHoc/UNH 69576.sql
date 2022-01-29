use udw
go
DROP TABLE IF EXISTS #DATA
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	CASE WHEN PD30.DF_PRS_ID IS NULL THEN 'Y' ELSE 'N' END AS ADDRESS_SKIP,
	CASE WHEN PD42.DF_PRS_ID IS NULL THEN 'Y' ELSE 'N' END AS PHONE_SKIP,
	LN10.OUTSTANDING_BALANCE,
	IM_LDR_FUL
INTO #DATA
FROM
	UDW..WQ20_TSK_QUE WQ20
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ20.BF_SSN
	INNER JOIN 
	(
		SELECT
			BF_SSN,
			IM_LDR_FUL,
			SUM(LA_CUR_PRI) AS OUTSTANDING_BALANCE
		FROM
			UDW..LN10_LON LN10
			LEFT JOIN OPENQUERY(DUSTER, 'SELECT * FROM OLWHRM1.LR10_LDR_DMO ') OQ
				ON OQ.IF_DOE_LDR = LN10.IF_DOE_LDR
		GROUP BY
			BF_SSN,
			IM_LDR_FUL
		HAVING SUM(LA_CUR_PRI) > 0
	) LN10
		ON LN10.BF_SSN = WQ20.BF_SSN
	LEFT JOIN UDW..PD27_PRS_SKP_PRC PD27
		ON PD27.DF_PRS_ID = WQ20.BF_SSN
		AND GETDATE() BETWEEN PD27.DD_SKP_BEG AND PD27.DD_SKP_END
	LEFT JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = WQ20.BF_SSN
		AND PD30.DI_VLD_ADR = 'Y'
		AND PD30.DC_ADR = 'L'
	LEFT JOIN UDW..PD42_PRS_PHN PD42
		ON PD42.DF_PRS_ID = WQ20.BF_SSN
		AND PD42.DI_PHN_VLD = 'Y'
		AND PD42.DC_PHN = 'H'
WHERE
	WF_QUE = 'JR'
	AND WF_SUB_QUE = '01' 


	SELECT * FROM #DATA 