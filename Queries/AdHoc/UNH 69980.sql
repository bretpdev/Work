
DROP TABLE IF EXISTS #BASE_DATA

SELECT
	PD10.DF_SPE_ACC_ID AS [ACCOUNT NUMBER],
	PD10.DF_PRS_ID,
	LN10.LN_SEQ,
	SUM(LN10.LA_CUR_PRI) OVER (PARTITION BY PD10.DF_SPE_ACC_ID) AS [TOTAL OUTSTANDING PRINCIPAL]
INTO #BASE_DATA
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
WHERE
	PD30.DF_LST_DTS_PD30 <= '2020-12-31'
	AND PD30.DC_DOM_ST = 'CA'
	AND PD30.DC_ADR = 'L'
	AND PD30.DI_VLD_ADR = 'Y' 
	AND LN10.LA_CUR_PRI > 0 
	AND LN10.LC_STA_LON10 = 'R'




--QUERY 1
SELECT DISTINCT
	[ACCOUNT NUMBER],
	[TOTAL OUTSTANDING PRINCIPAL]
FROM 
	#BASE_DATA
WHERE
	[TOTAL OUTSTANDING PRINCIPAL] > 0.00

SELECT 
	[ACCOUNT NUMBER],
	[TOTAL OUTSTANDING PRINCIPAL],
	CASE WHEN DW01.WC_DW_LON_STA = '01' THEN 'IN GRACE'
		 WHEN DW01.WC_DW_LON_STA = '02' THEN 'IN SCHOOL'
		 WHEN DW01.WC_DW_LON_STA = '03' THEN 
			CASE WHEN RS.LC_TYP_SCH_DIS = 'IB' THEN 'IBR'
				 ELSE 'REPAYMENT (NON IBR)'
			END 
		 WHEN DW01.WC_DW_LON_STA = '04' THEN 'IN DEFERMENT'
		 WHEN DW01.WC_DW_LON_STA = '05' THEN 'IN FORBEARANCE'
		 WHEN DW01.WC_DW_LON_STA = '06' THEN 'IN CURE'
		 WHEN DW01.WC_DW_LON_STA = '07' THEN 'CLAIM PENDING'
		 WHEN DW01.WC_DW_LON_STA = '08' THEN 'CLAIM SUBMITTED'
		 WHEN DW01.WC_DW_LON_STA = '09' THEN 'CLAIM CANCELLED'
		 WHEN DW01.WC_DW_LON_STA = '10' THEN 'CLAIM REJECTED'
		 WHEN DW01.WC_DW_LON_STA = '11' THEN 'CLAIM RETURNED'
		 WHEN DW01.WC_DW_LON_STA = '12' THEN 'CLAIM PAID'
		 WHEN DW01.WC_DW_LON_STA = '13' THEN 'PRE-CLAIM PENDING'
		 WHEN DW01.WC_DW_LON_STA = '14' THEN 'PRE-CLAIM SUBMITTED'
		 WHEN DW01.WC_DW_LON_STA = '15' THEN 'PRE-CLAIM CANCELLED'
		 WHEN DW01.WC_DW_LON_STA = '16' THEN 'DEATH ALLEGED'
		 WHEN DW01.WC_DW_LON_STA = '17' THEN 'DEATH VERIFIED'
		 WHEN DW01.WC_DW_LON_STA = '18' THEN 'DISABILITY ALLEGED'
		 WHEN DW01.WC_DW_LON_STA = '19' THEN 'DISABILITY VERIFIED'
		 WHEN DW01.WC_DW_LON_STA = '20' THEN 'BANKRUPTCY ALLEGED'
		 WHEN DW01.WC_DW_LON_STA = '21' THEN 'BANKRUPTCY VERIFIED'
		 WHEN DW01.WC_DW_LON_STA = '22' THEN 'PAID IN FULL'
		 WHEN DW01.WC_DW_LON_STA = '23' THEN 'NOT FULLY ORIGINATED'
		 WHEN DW01.WC_DW_LON_STA = '88' THEN 'PROCESSING ERROR'
		 WHEN DW01.WC_DW_LON_STA = '98' THEN 'UNKNOWN'
		 ELSE 'OTHER'
	END AS LoanStatus
FROM 
	#BASE_DATA BD
	INNER JOIN AuditUDW..DW01_DW_CLC_CLU_Dec2020 DW01
		ON DW01.BF_SSN = BD.DF_PRS_ID
		AND DW01.LN_SEQ = BD.LN_SEQ
	LEFT JOIN UDW.CALC.RepaymentSchedules RS
		ON RS.BF_SSN = BD.DF_PRS_ID
		AND RS.LN_SEQ = BD.LN_SEQ
		AND RS.CurrentGradation = 1
WHERE
	BD.[TOTAL OUTSTANDING PRINCIPAL] > 0.00
		

--SELECT
--	COUNT(DISTINCT LN10.BF_SSN) AS NumberOfBorrowers,
--	SUM(COALESCE(LN10.LA_CUR_PRI,0.00) + COALESCE(DW01.WA_TOT_BRI_OTS,0.00)) AS TotalOutstandingBalance,
--	COUNT(DISTINCT Students.LF_STU_SSN) + COUNT(DISTINCT Endorsers.LF_EDS) AS Relationships,
--	CASE WHEN DW01.WC_DW_LON_STA = '01' THEN 'IN GRACE'
--		 WHEN DW01.WC_DW_LON_STA = '02' THEN 'IN SCHOOL'
--		 WHEN DW01.WC_DW_LON_STA = '03' THEN 
--			CASE WHEN LN65.LC_TYP_SCH_DIS = 'IB' THEN 'IBR'
--				 ELSE 'Repayment (non IBR)'
--			END 
--		 WHEN DW01.WC_DW_LON_STA = '04' THEN 'IN DEFERMENT'
--		 WHEN DW01.WC_DW_LON_STA = '05' THEN 'IN FORBEARANCE'
--		 WHEN DW01.WC_DW_LON_STA = '06' THEN 'IN CURE'
--		 WHEN DW01.WC_DW_LON_STA = '07' THEN 'CLAIM PENDING'
--		 WHEN DW01.WC_DW_LON_STA = '08' THEN 'CLAIM SUBMITTED'
--		 WHEN DW01.WC_DW_LON_STA = '09' THEN 'CLAIM CANCELLED'
--		 WHEN DW01.WC_DW_LON_STA = '10' THEN 'CLAIM REJECTED'
--		 WHEN DW01.WC_DW_LON_STA = '11' THEN 'CLAIM RETURNED'
--		 WHEN DW01.WC_DW_LON_STA = '12' THEN 'CLAIM PAID'
--		 WHEN DW01.WC_DW_LON_STA = '13' THEN 'PRE-CLAIM PENDING'
--		 WHEN DW01.WC_DW_LON_STA = '14' THEN 'PRE-CLAIM SUBMITTED'
--		 WHEN DW01.WC_DW_LON_STA = '15' THEN 'PRE-CLAIM CANCELLED'
--		 WHEN DW01.WC_DW_LON_STA = '16' THEN 'DEATH ALLEGED'
--		 WHEN DW01.WC_DW_LON_STA = '17' THEN 'DEATH VERIFIED'
--		 WHEN DW01.WC_DW_LON_STA = '18' THEN 'DISABILITY ALLEGED'
--		 WHEN DW01.WC_DW_LON_STA = '19' THEN 'DISABILITY VERIFIED'
--		 WHEN DW01.WC_DW_LON_STA = '20' THEN 'BANKRUPTCY ALLEGED'
--		 WHEN DW01.WC_DW_LON_STA = '21' THEN 'BANKRUPTCY VERIFIED'
--		 WHEN DW01.WC_DW_LON_STA = '22' THEN 'PAID IN FULL'
--		 WHEN DW01.WC_DW_LON_STA = '23' THEN 'NOT FULLY ORIGINATED'
--		 WHEN DW01.WC_DW_LON_STA = '88' THEN 'PROCESSING ERROR'
--		 WHEN DW01.WC_DW_LON_STA = '98' THEN 'UNKNOWN'
--		 ELSE 'OTHER'
--	END AS LoanStatus
--FROM
--	AuditUDW..LN10_LON_Dec2019 LN10
--	INNER JOIN AuditUDW..DW01_DW_CLC_CLU_Dec2019 DW01
--		ON DW01.BF_SSN = LN10.BF_SSN
--		AND DW01.LN_SEQ = LN10.LN_SEQ
--	INNER JOIN
--	( 
--		SELECT DISTINCT
--			PD30.DF_PRS_ID
--		FROM 
--			UDW..PD30_PRS_ADR PD30 
--		WHERE
--			PD30.DF_LST_DTS_PD30 <= '2019-12-31'
--			AND PD30.DC_DOM_ST = 'CA'
--			AND PD30.DC_ADR = 'L'
		
--		--UNION

--		--SELECT * FROM OPENQUERY(DUSTER,
--		--'
--		--SELECT DISTINCT
--		--	PD31.DF_PRS_ID
--		--FROM
--		--	OLWHRM1.PD31_PRS_INA PD31
--		--	INNER JOIN
--		--	(
--		--		SELECT
--		--			MaxAdr.DF_PRS_ID,
--		--			MaxAdr.DC_DOM_ST_HST,
--		--			MaxAdr.DC_ADR_HST,
--		--			MAX(MaxAdr.DF_LST_DTS_PD31) AS MaxDate
--		--		FROM
--		--			OLWHRM1.PD31_PRS_INA MaxAdr
--		--		WHERE
--		--			MaxAdr.DF_LST_DTS_PD31 <= ''2018-12-31''
--		--			AND MaxAdr.DC_DOM_ST_HST = ''CA''
--		--			AND MaxAdr.DC_ADR_HST = ''L''
--		--		GROUP BY
--		--			MaxAdr.DF_PRS_ID,
--		--			MaxAdr.DC_DOM_ST_HST,
--		--			MaxAdr.DC_ADR_HST
--		--	) MaxPD31
--		--		ON MaxPD31.DF_PRS_ID = PD31.DF_PRS_ID
--		--		AND MaxPD31.DC_DOM_ST_HST = PD31.DC_DOM_ST_HST
--		--		AND MaxPD31.DC_ADR_HST = PD31.DC_ADR_HST
--		--		AND MaxPD31.MaxDate = PD31.DF_LST_DTS_PD31
--		--WHERE
--		--	PD31.DF_LST_DTS_PD31 <= ''2018-12-31''
--		--	AND PD31.DC_DOM_ST_HST = ''CA''
--		--	AND PD31.DC_ADR_HST = ''L''
--		--')
--	) PD30
--		ON LN10.BF_SSN = PD30.DF_PRS_ID
--	LEFT JOIN
--	(
--		SELECT DISTINCT
--			LN20.BF_SSN,
--			LN20.LN_SEQ,
--			LN20.LF_EDS
--		FROM
--			UDW..LN20_EDS LN20
--			INNER JOIN UDW..PD30_PRS_ADR PD30
--				ON PD30.DF_PRS_ID = LN20.LF_EDS
--				AND PD30.DC_ADR = 'L'
--				AND PD30.DC_DOM_ST = 'CA'
--	) Endorsers
--		ON Endorsers.BF_SSN = LN10.BF_SSN
--		AND Endorsers.LN_SEQ = LN10.LN_SEQ
--	LEFT JOIN
--	(
--		SELECT DISTINCT
--			SD10.LF_STU_SSN
--		FROM
--			UDW..SD10_STU_SPR SD10
--			INNER JOIN UDW..PD30_PRS_ADR PD30
--				ON PD30.DF_PRS_ID = SD10.LF_STU_SSN
--				AND PD30.DC_ADR = 'L'
--				AND PD30.DC_DOM_ST = 'CA'
--	) Students
--		ON Students.LF_STU_SSN = LN10.LF_STU_SSN
--		AND Students.LF_STU_SSN != LN10.BF_SSN
--	LEFT JOIN 
--	(
--		SELECT DISTINCT
--			LN65.BF_SSN,
--			LN65.LN_SEQ,
--			LN65.LC_TYP_SCH_DIS
--		FROM
--			UDW..RS10_BR_RPD RS10
--			INNER JOIN UDW..LN65_LON_RPS LN65
--				ON RS10.BF_SSN = LN65.BF_SSN
--				AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
--			INNER JOIN 
--			(
--				SELECT
--					LN65.BF_SSN,
--					LN65.LN_SEQ,
--					MAX(RS10.LD_RPS_1_PAY_DU) AS MaxDate
--				FROM
--					UDW..RS10_BR_RPD RS10
--					INNER JOIN UDW..LN65_LON_RPS LN65
--						ON RS10.BF_SSN = LN65.BF_SSN
--						AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
--				WHERE
--					 RS10.LD_RPS_1_PAY_DU <= '2019-12-31'
--				GROUP BY
--					LN65.BF_SSN,
--					LN65.LN_SEQ
--			)MaxTime
--				ON MaxTime.BF_SSN = LN65.BF_SSN
--				AND MaxTime.LN_SEQ = LN65.LN_SEQ
--				AND MaxTime.MaxDate = RS10.LD_RPS_1_PAY_DU
--	) LN65
--		ON LN65.BF_SSN = LN10.BF_SSN
--		AND LN65.LN_SEQ = LN10.LN_SEQ
--WHERE
--	LN10.LC_STA_LON10 = 'R'
--	AND COALESCE(LN10.LA_CUR_PRI,0.00) + COALESCE(DW01.WA_TOT_BRI_OTS,0.00) > 0 /*Has balance*/
--GROUP BY
--	CASE WHEN DW01.WC_DW_LON_STA = '01' THEN 'IN GRACE'
--		 WHEN DW01.WC_DW_LON_STA = '02' THEN 'IN SCHOOL'
--		 WHEN DW01.WC_DW_LON_STA = '03' THEN
--			CASE WHEN LN65.LC_TYP_SCH_DIS = 'IB' THEN 'IN REPAYMENT - INCOME BASED'
--				 ELSE 'IN REPAYMENT - OTHER'
--			END
--		 WHEN DW01.WC_DW_LON_STA = '04' THEN 'IN DEFERMENT'
--		 WHEN DW01.WC_DW_LON_STA = '05' THEN 'IN FORBEARANCE'
--		 WHEN DW01.WC_DW_LON_STA = '06' THEN 'IN CURE'
--		 WHEN DW01.WC_DW_LON_STA = '07' THEN 'CLAIM PENDING'
--		 WHEN DW01.WC_DW_LON_STA = '08' THEN 'CLAIM SUBMITTED'
--		 WHEN DW01.WC_DW_LON_STA = '09' THEN 'CLAIM CANCELLED'
--		 WHEN DW01.WC_DW_LON_STA = '10' THEN 'CLAIM REJECTED'
--		 WHEN DW01.WC_DW_LON_STA = '11' THEN 'CLAIM RETURNED'
--		 WHEN DW01.WC_DW_LON_STA = '12' THEN 'CLAIM PAID'
--		 WHEN DW01.WC_DW_LON_STA = '13' THEN 'PRE-CLAIM PENDING'
--		 WHEN DW01.WC_DW_LON_STA = '14' THEN 'PRE-CLAIM SUBMITTED'
--		 WHEN DW01.WC_DW_LON_STA = '15' THEN 'PRE-CLAIM CANCELLED'
--		 WHEN DW01.WC_DW_LON_STA = '16' THEN 'DEATH ALLEGED'
--		 WHEN DW01.WC_DW_LON_STA = '17' THEN 'DEATH VERIFIED'
--		 WHEN DW01.WC_DW_LON_STA = '18' THEN 'DISABILITY ALLEGED'
--		 WHEN DW01.WC_DW_LON_STA = '19' THEN 'DISABILITY VERIFIED'
--		 WHEN DW01.WC_DW_LON_STA = '20' THEN 'BANKRUPTCY ALLEGED'
--		 WHEN DW01.WC_DW_LON_STA = '21' THEN 'BANKRUPTCY VERIFIED'
--		 WHEN DW01.WC_DW_LON_STA = '22' THEN 'PAID IN FULL'
--		 WHEN DW01.WC_DW_LON_STA = '23' THEN 'NOT FULLY ORIGINATED'
--		 WHEN DW01.WC_DW_LON_STA = '88' THEN 'PROCESSING ERROR'
--		 WHEN DW01.WC_DW_LON_STA = '98' THEN 'UNKNOWN'
--		 ELSE 'OTHER'
--	END