USE CDW;
GO

SELECT DISTINCT
	LNXX.BF_SSN AS SSN
	,LNXX.LN_SEQ AS LoanSeq
	,LNXX.LD_FAT_EFF AS PaymentDate
	,LNXX.LC_RMT_BCH_SRC_IPT AS PaymentSource
	,LNXX.LF_DOE_SCL_ORG AS SchoolCode
FROM
	LNXX_LON LNXX
	INNER JOIN LNXX_FIN_ATY LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN LNXX_LON_PAY_FAT LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LN_FAT_SEQ = LNXX.LN_FAT_SEQ
WHERE
	LNXX.LF_DOE_SCL_ORG IN ('XXXXXXXX', 'XXXXXXXX', 'XXXXXXXX')
	AND LNXX.LD_FAT_EFF >= CONVERT(DATE,'XXXXXXXX')
	AND LNXX.LD_FAT_EFF <= CONVERT(DATE,'XXXXXXXX')
	AND LNXX.LC_RMT_BCH_SRC_IPT = 'S'
;