SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
GO

USE UDW;
GO

--interest rate
SELECT DISTINCT
	LN10.BF_SSN AS BorrowerSSN
	,LN10.LN_SEQ AS LoanSequenceNumber
	,LN72.LR_ITR AS InterestRate
	,LN10.IC_LON_PGM AS LoanProgramCode
	,LN72.LC_ITR_TYP AS InterestType 
	,LN72.LD_ITR_EFF_BEG AS InterestRateBeginningEffectiveDate

FROM
	LN10_LON LN10
	INNER JOIN WQ20_TSK_QUE WQ20
		ON LN10.BF_SSN = WQ20.BF_SSN
	INNER JOIN LN72_INT_RTE_HST LN72
		ON LN10.BF_SSN = LN72.BF_SSN
		AND LN10.LN_SEQ = LN72.LN_SEQ
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND WQ20.WF_QUE = 'SZ'
	AND WQ20.WF_SUB_QUE = '01'
	AND LN72.LC_STA_LON72 = 'A'
	AND CAST(WQ20.WD_ACT_REQ AS DATE) > '2019-03-14'
ORDER BY
	LN10.BF_SSN
	,LN10.LN_SEQ
	,LN72.LD_ITR_EFF_BEG
;

--repayment
SELECT DISTINCT
	LN10.BF_SSN AS BorrowerSSN
	,DW01.WD_LON_RPD_SR AS RepaymentStartDate
FROM
	LN10_LON LN10
	INNER JOIN WQ20_TSK_QUE WQ20
		ON LN10.BF_SSN = WQ20.BF_SSN
	INNER JOIN DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND WQ20.WF_QUE = 'SZ'
	AND WQ20.WF_SUB_QUE = '01'
	AND CAST(WQ20.WD_ACT_REQ AS DATE) > '2019-03-14'
ORDER BY
	LN10.BF_SSN
	,DW01.WD_LON_RPD_SR
;

--deferment
SELECT DISTINCT
	LN10.BF_SSN AS BorrowerSSN
	,DF10.LD_DFR_REQ_BEG AS DefermentBeginDate
	,DF10.LD_DFR_REQ_END AS DefermentEndDate
FROM
	LN10_LON LN10
	INNER JOIN WQ20_TSK_QUE WQ20
		ON LN10.BF_SSN = WQ20.BF_SSN
	INNER JOIN DF10_BR_DFR_REQ DF10
		ON LN10.BF_SSN = DF10.BF_SSN		
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND WQ20.WF_QUE = 'SZ'
	AND WQ20.WF_SUB_QUE = '01'
	AND DF10.LC_STA_DFR10 = 'A'
	AND DF10.LC_DFR_STA = 'A'
	AND CAST(WQ20.WD_ACT_REQ AS DATE) > '2019-03-14'
ORDER BY
	LN10.BF_SSN
	,DF10.LD_DFR_REQ_BEG
	,DF10.LD_DFR_REQ_END
;

--disbursement
SELECT 
	LN10.BF_SSN AS BorrowerSSN
	,LN15.LD_DSB AS DisbursementDate
	,SUM(LN15.LA_DSB) AS DisbursementAmount
FROM
	LN10_LON LN10
	INNER JOIN WQ20_TSK_QUE WQ20
		ON LN10.BF_SSN = WQ20.BF_SSN
	LEFT JOIN LN15_DSB LN15
		ON LN10.BF_SSN = LN15.BF_SSN
		AND LN10.LN_SEQ = LN15.LN_SEQ
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND WQ20.WF_QUE = 'SZ'
	AND WQ20.WF_SUB_QUE = '01'
	AND CAST(WQ20.WD_ACT_REQ AS DATE) > '2019-03-14'
GROUP BY
	LN10.BF_SSN
	,LN15.LD_DSB
ORDER BY
	LN10.BF_SSN
	,LN15.LD_DSB
;