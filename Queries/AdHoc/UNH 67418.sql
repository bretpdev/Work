--a list of all consolidation loans with the code CNSLDN, SUBCNS, UNCNS; include the following: 
--Loan Code
--Account #
--Customer Name
--Balance
--Repayment Plan
--Expected Payoff Date



--query used for the ticket (LEFT JOIN to RS10 and LN65)
SELECT
	PD10.DF_SPE_ACC_ID AS [Account #],
	LN10.LN_SEQ,
	LN10.IC_LON_PGM AS [Loan Code],
	RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [Customer Name],
	LN10.LA_CUR_PRI + DW01.WA_TOT_BRI_OTS AS [Balance],
	RS.LC_TYP_SCH_DIS AS [Repayment Plan],
	RS.REPAY_PLAN AS [Repayment Plan Description],
	DW01.WD_XPC_POF_TS26 AS [Expected Payoff Date]
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	LEFT JOIN 
	(
		SELECT
			LN65.BF_SSN,
			LN65.LN_SEQ,
			LN65.LC_TYP_SCH_DIS,
			ISNULL(LK10.PX_DSC_MDM,LN65.LC_TYP_SCH_DIS) AS REPAY_PLAN
		FROM
			UDW..RS10_BR_RPD RS10
			INNER JOIN UDW..LN65_LON_RPS LN65
				ON RS10.BF_SSN = LN65.BF_SSN
				AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
			LEFT JOIN UDW..LK10_LS_CDE_LKP LK10
				ON LN65.LC_TYP_SCH_DIS = LK10.PX_ATR_VAL
				AND LK10.PM_ATR = 'LC-TYP-SCH-DIS' 
			WHERE
				LN65.LC_STA_LON65 = 'A'
				AND RS10.LC_STA_RPST10 = 'A'
	) RS
		ON LN10.BF_SSN = RS.BF_SSN
		AND LN10.LN_SEQ = RS.LN_SEQ
WHERE
	LN10.IC_LON_PGM IN ('CNSLDN', 'SUBCNS', 'UNCNS')
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI + DW01.WA_TOT_BRI_OTS > 0
	--AND RS.LC_TYP_SCH_DIS IS NOT NULL

--this query leverages the calc.repaymentschedules table and returns the same number of rows as the query above but more of the repayment plans are null (e.g 0046996991)
--UDW.calc.RepaymentSchedules
SELECT
	PD10.DF_SPE_ACC_ID AS [Account #],
	LN10.LN_SEQ,
	LN10.IC_LON_PGM AS [Loan Code],
	RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [Customer Name],
	LN10.LA_CUR_PRI + DW01.WA_TOT_BRI_OTS AS [Balance],
	RS.LC_TYP_SCH_DIS AS [Repayment Plan],
	ISNULL(LK10.PX_DSC_MDM,RS.LC_TYP_SCH_DIS) AS [Repayment Plan Description],
	DW01.WD_XPC_POF_TS26 AS [Expected Payoff Date],
	DATEADD(MONTH,RS.GradationMonths,RS.TermStartDate) 
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	LEFT JOIN UDW.calc.RepaymentSchedules RS
		ON LN10.BF_SSN = RS.BF_SSN
		AND LN10.LN_SEQ = RS.LN_SEQ
		AND RS.CurrentGradation = 1
	LEFT JOIN UDW..LK10_LS_CDE_LKP LK10
		ON RS.LC_TYP_SCH_DIS = LK10.PX_ATR_VAL
		AND LK10.PM_ATR = 'LC-TYP-SCH-DIS' 
WHERE
	LN10.IC_LON_PGM IN ('CNSLDN', 'SUBCNS', 'UNCNS')
	AND LN10.LC_STA_LON10 != 'R'
	AND LN10.LA_CUR_PRI + DW01.WA_TOT_BRI_OTS > 0
	--AND PD10.DF_SPE_ACC_ID = '0046996991'
	--AND RS.LC_TYP_SCH_DIS IS NOT NULL