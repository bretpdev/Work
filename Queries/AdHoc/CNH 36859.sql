SELECT DISTINCT
	PDXX.DM_PRS_X AS FirstName,
	PDXX.DM_PRS_LST AS LastName,
	PDXX.DF_SPE_ACC_ID AS AccountNumber,
	CASE WHEN AYXX.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS HasDIFCR
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	LEFT JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.PF_REQ_ACT = 'DIFCR'
		AND AYXX.LD_ATY_REQ_RCV >= 'XXXX-XX-XX'
WHERE
	LNXX.LF_DOE_SCL_ORG IN('XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')