SELECT DISTINCT
	--PD10.DF_SPE_ACC_ID AS AccountNumber,
	PD10.DF_PRS_ID AS SSN,
	RTRIM(PD10.DM_PRS_1) AS FirstName,
	RTRIM(PD10.DM_PRS_LST) AS LastName,
	LN10.IC_LON_PGM AS LoanType,
	LN10.LN_SEQ AS LoanNumber,
	LN10.LA_CUR_PRI AS CurrentPrincipalBalance,
	CONVERT(VARCHAR,LN10.LD_LON_1_DSB,101) AS FirstDisbursementDate,
	LN10.LA_LON_AMT_GTR AS OriginalPrincipalBalance,
	CONVERT(VARCHAR,RS10.BD_CRT_RS05,101) AS IbrCreateDate,
	CONVERT(VARCHAR,RS10.LD_RPS_1_PAY_DU,101) AS IbrFirstPaymentDue
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..AY10_BR_LON_ATY AY10
		ON PD10.DF_PRS_ID = AY10.BF_SSN
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
	INNER JOIN UDW..RS05_IBR_RPS RS05
		ON RS05.BF_SSN = LN10.BF_SSN
		AND RS05.BC_STA_RS05 = 'A'
	INNER JOIN UDW..RS10_BR_RPD RS10
		ON RS10.BF_SSN = RS05.BF_SSN
		AND RS10.BD_CRT_RS05 = RS05.BD_CRT_RS05
		AND RS10.BN_IBR_SEQ = RS05.BN_IBR_SEQ
WHERE
	AY10.PF_REQ_ACT = 'IBAPV'
	AND AY10.LD_ATY_REQ_RCV BETWEEN '2018-01-01' AND '2018-12-31'