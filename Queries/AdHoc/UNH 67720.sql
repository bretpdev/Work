/*
Requesting a query of all loans with a remaining balance, and zero terms left on their repayment schedule.
Requested output: SSN, Accnt#, outstanding principal balance
Requested by 6/24/2020
*/
USE UDW
GO

SELECT DISTINCT
	LN10.BF_SSN AS SSN,
	LN10.LN_SEQ,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	LN10.LA_CUR_PRI AS OutstandingPrincipal,
	LP10.PN_RPD_TRM_MAX,
	LP10.PN_RPS_TRM_MAX_XTN,
	CASE WHEN RS.TermsToDate >= LP10.PN_RPD_TRM_MAX THEN 'Y' ELSE 'N' END AS ExceedsTerms,
	CASE WHEN RS.TermsToDate >= LP10.PN_RPS_TRM_MAX_XTN THEN 'Y' ELSE 'N' END AS ExceedsExtendedTerms,
	RS.LC_TYP_SCH_DIS,
	RS.TermsToDate
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN OPENQUERY(DUSTER, 'SELECT * FROM OLWHRM1.LP10_RPY_PAR WHERE PC_STA_LPD10 = ''A''') LP10
		ON LP10.IC_LON_PGM = LN10.IC_LON_PGM
		AND LP10.PF_RGL_CAT = LN10.LF_RGL_CAT_LP10
		AND LP10.IF_GTR = LN10.IF_GTR
		AND LP10.IF_OWN = LN10.LF_LON_CUR_OWN
	INNER JOIN UDW.calc.RepaymentSchedules RS
		ON RS.BF_SSN = LN10.BF_SSN
		AND RS.LN_SEQ = LN10.LN_SEQ
		AND RS.CurrentGradation = 1 
		AND RS.TermsToDate >= LP10.PN_RPD_TRM_MAX
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00

