USE CDW
GO

SELECT DISTINCT
	PDXX.DM_PRS_X AS [First Name],
	PDXX.DM_PRS_LST AS [Last Name],
	LNXX.BF_SSN AS SSN,
	AYXX.PF_REQ_ACT AS ARC
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN AYXX_BR_LON_ATY AYXX
		ON LNXX.BF_SSN = AYXX.BF_SSN
WHERE
	AYXX.PF_REQ_ACT IN ('DICSK','DIFCR')
	AND AYXX.LD_ATY_REQ_RCV BETWEEN  'XXXX-XX-XX' AND 'XXXX-XX-XX'
	AND 
	LNXX.LF_DOE_SCL_ORG IN 
	(
		'XXXXXXXX',
		'XXXXXXXX',
		'XXXXXXXX'
	)
