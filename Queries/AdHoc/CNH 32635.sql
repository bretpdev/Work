SELECT * FROM OPENQUERY(LEGEND, 
'
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID
FROM 
	PKUB.LNXX_LON LNXX 
	INNER JOIN PKUB.PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
WHERE 
	LNXX.LF_DOE_SCL_ORG = ''XXXXXXXX''')