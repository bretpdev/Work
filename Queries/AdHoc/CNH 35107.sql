SELECT * FROM OPENQUERY(LEGEND,
'
SELECT
	LNXX.*
FROM
	PKUB.LNXX_LON_BIL_CRF LNXX
	INNER JOIN PKUB.PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID = ''XXXXXXXXXX'' 
	AND LNXX.LN_SEQ = X
	AND LNXX.LD_BIL_CRT < ''XX/XX/XXXX''

')