--X. Find all loans where the Repayment Start Date is > XX years in the future.
--Output: 
--Account Number
--Loan Sequence
--Disbursement Date
--Repayment Start Date

USE CDW;
GO

SELECT DISTINCT
	--RS.BF_SSN,
	PDXX.DF_SPE_ACC_ID AS AccountNumber,
	RS.LN_SEQ AS LoanSequence,
	RS.LD_LON_X_DSB AS DisbursementDate,
	RS.LD_RPS_X_PAY_DU AS RepaymentStartDate,
	DATEADD(YEAR,XX,CONVERT(DATE,GETDATE())) AS TenYearsFutureDate
FROM
	calc.RepaymentSchedules RS
	INNER JOIN PDXX_PRS_NME PDXX
		ON RS.BF_SSN = PDXX.DF_PRS_ID
WHERE
	RS.LD_RPS_X_PAY_DU > DATEADD(YEAR,XX,CONVERT(DATE,GETDATE()))
;

--X. Find all borrowers/loans where there has been type XX deferment on the account and the NSLDS reporting for active duty is blank (GR.XX.WC_NDS_ATV_DTY_RPT). 
--Output:
--Account #
--Loan Seq
--Deferment Begin and End Dates

SELECT DISTINCT
	--LNXX.BF_SSN,
	PDXX.DF_SPE_ACC_ID AS Account#,
	LNXX.LN_SEQ AS LoanSeq,
	LNXX.LD_DFR_BEG AS DefermentBegin,
	LNXX.LD_DFR_END AS DefermentEnd,
	--validation fields:
	DFXX.LC_DFR_TYP AS DefermentType,
	GRXX.WC_NDS_ATV_DTY_RPT,
	LNXX.LC_DFR_RSP,
	LNXX.LC_STA_LONXX,
	DFXX.LC_STA_DFRXX,
	DFXX.LC_DFR_STA,
	LNXX.LC_STA_LONXX,
	IIF(LNXX.LA_CUR_PRI > X.XX, '>X','X') AS LA_CUR_PRI
FROM
	LNXX_BR_DFR_APV LNXX
	INNER JOIN GRXX_RPT_LON_APL GRXX
		ON LNXX.BF_SSN = GRXX.BF_SSN
		AND LNXX.LN_SEQ = GRXX.LN_SEQ
	INNER JOIN DFXX_BR_DFR_REQ DFXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM	= DFXX.LF_DFR_CTL_NUM
	INNER JOIN PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	COALESCE(LTRIM(RTRIM(GRXX.WC_NDS_ATV_DTY_RPT)),'') = ''
	AND DFXX.LC_DFR_TYP = 'XX'
	AND LNXX.LC_DFR_RSP != 'XXX' --not denied
	AND LNXX.LC_STA_LONXX = 'A'
	AND DFXX.LC_STA_DFRXX = 'A'
	AND DFXX.LC_DFR_STA = 'A'
	AND LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X.XX
;


--X. Find all loans where there is an active SCRA rate (LNXX.LC_INT_RDC_PGM = "M"; LNXX.LC_STA_LONXX = "A") and the NSLDS SCRA Military Record is blank (GRXX.WC_NDS_SCRA_RPT).
--Output:
--Account #
--Loan Seq

SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID AS Account#,
	LNXX.LN_SEQ AS LoanSequence,
	--validation fields:
	GRXX.WC_NDS_SCRA_RPT,
	LNXX.LC_INT_RDC_PGM,
	LNXX.LC_STA_LONXX,
	LNXX.LC_STA_LONXX,
	IIF(LNXX.LA_CUR_PRI > X.XX, '>X','X') AS LA_CUR_PRI
FROM
	LNXX_INT_RTE_HST LNXX
	INNER JOIN GRXX_RPT_LON_APL GRXX
		ON LNXX.BF_SSN = GRXX.BF_SSN
		AND LNXX.LN_SEQ = GRXX.LN_SEQ
	INNER JOIN PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	COALESCE(LTRIM(RTRIM(GRXX.WC_NDS_SCRA_RPT)),'') = ''
	AND LNXX.LC_INT_RDC_PGM = 'M'
	AND LNXX.LC_STA_LONXX = 'A'
	AND LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X.XX
;