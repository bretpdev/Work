USE UDW
GO

SELECT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	LN10.LD_LON_ACL_ADD,
	LN10.LC_STA_LON10,
	LN10.LD_STA_LON10,
	LN10.LF_LON_CUR_OWN,
	LN10.IF_GTR,
	LN10.IC_LON_PGM,
	LN10.LD_END_GRC_PRD,
	LN10.LA_CUR_PRI,
	LN10.LA_NSI_OTS,
	LN10.LD_GTE_LOS,
	DW01.WC_DW_LON_STA,
	DW01.DW_LON_STA,
	DW01.WD_LON_RPD_SR,
	DW01.WA_TOT_BRI_OTS,
	LN16.LD_DLQ_OCC,
	LN16.LN_DLQ_MAX,
	LN16.LD_DLQ_MAX,
	LN72.LR_ITR,
	LN72.LR_INT_RDC_PGM_ORG
FROM
	dbo.PD10_Borrower PD10
	INNER JOIN dbo.[LN10_LON] LN10 ON LN10.BF_SSN = PD10.BF_SSN
	LEFT JOIN dbo.DW01_Loan DW01 ON DW01.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID AND DW01.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN LN16_Delinquency LN16 ON LN16.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID AND LN16.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN LN72_InterestRate LN72 ON LN72.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID AND LN72.LN_SEQ = LN10.LN_SEQ
WHERE
	LN10.LD_LON_ACL_ADD = '2016-02-26'