SELECT
	PDXX.DF_SPE_ACC_ID,--Account Number
	LNXX.LN_SEQ,--Loan Seq
	LNXX.LA_CUR_PRI,--Current Balance
	LNXX.LF_DOE_SCL_ORG --OPE ID
	,LNXX.LC_STA_LONXX -- not requested but included to see if it should be
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
WHERE
	LNXX.LF_DOE_SCL_ORG IN ('XXXXXXXX','XXXXXXXX')--ORIGINAL OPE ID (school code) is XXXXXXXX or XXXXXXXX.
	AND LNXX.LA_CUR_PRI > X.XX
ORDER BY
	LNXX.LC_STA_LONXX -- not requested but included to see if it should be