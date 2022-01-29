USE UDW
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT DISTINCT
	POP.BF_SSN,
	(LTRIM(RTRIM(PD10.DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10.DM_PRS_LST))) AS [NAME],
	POP.LD_ATY_REQ_RCV AS DATE_OF_COMMENT,
	LK10.PX_DSC_MDM AS LOAN_STATUS,
	RS.LC_TYP_SCH_DIS AS CURRENT_REPAYMENT_PLAN,
	RS.LA_RPS_ISL AS REPAYMENT_AMOUNT,
	LN10.TOTAL_BALANCE,
	CASE WHEN DW01.WC_DW_LON_STA = '12' THEN 'Y' END AS IS_DEFAULTED
FROM
(
	SELECT DISTINCT
		AY20.BF_SSN,
		AY10.LN_ATY_SEQ,
		AY10.LD_ATY_REQ_RCV,
		AY10.PF_REQ_ACT,
		STUFF(
		(
			SELECT 
					' ' + SUB.LX_ATY AS [text()]
			FROM 
				UDW..AY20_ATY_TXT SUB
			WHERE
				SUB.BF_SSN = AY20.BF_SSN
				AND SUB.LN_ATY_SEQ = AY20.LN_ATY_SEQ
		FOR XML PATH('')
		)
		,1,1, '') AS LX_ATY
			
	FROM	
		UDW..AY10_BR_LON_ATY AY10
		INNER JOIN UDW..AY20_ATY_TXT AY20
			ON AY10.BF_SSN = AY20.BF_SSN
			AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = AY20.BF_SSN
	WHERE
		AY10.PF_REQ_ACT IN ('FBAPV','FBDNY')
		AND AY10.LC_STA_ACTY10 = 'A'
) POP
INNER JOIN UDW..PD10_PRS_NME PD10
	ON PD10.DF_PRS_ID = POP.BF_SSN
INNER JOIN
(
	SELECT
		BF_SSN,
		SUM(LA_CUR_PRI) AS TOTAL_BALANCE
	FROM
		UDW..LN10_LON
	GROUP BY
		BF_SSN 
) LN10
	ON LN10.BF_SSN = POP.BF_SSN
INNER JOIN UDW..DW01_DW_CLC_CLU DW01
	ON POP.BF_SSN = DW01.BF_SSN
INNER JOIN UDW..LK10_LS_CDE_LKP LK10
	ON LK10.PM_ATR = 'WC-DW-LON-STA'
	AND LK10.PX_ATR_VAL = DW01.WC_DW_LON_STA
LEFT JOIN
(
	SELECT
		BF_SSN,
		LC_TYP_SCH_DIS,
		SUM(LA_RPS_ISL) AS LA_RPS_ISL
	FROM
		UDW.calc.RepaymentSchedules RS
	WHERE RS.CurrentGradation = 1
	GROUP BY
		BF_SSN,
		LC_TYP_SCH_DIS
 ) RS
	ON RS.BF_SSN = POP.BF_SSN
WHERE 
	POP.LX_ATY LIKE '%FSA%'