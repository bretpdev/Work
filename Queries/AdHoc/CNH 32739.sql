SELECT * FROM OPENQUERY(LEGEND,
'
	SELECT DISTINCT
		PDXX.DF_SPE_ACC_ID
	FROM 
		PKUB.PDXX_PRS_NME PDXX
		INNER JOIN PKUB.LNXX_LON LNXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		LEFT JOIN PKUB.AYXX_BR_LON_ATY AYXX
			ON AYXX.BF_SSN = PDXX.DF_PRS_ID
		LEFT JOIN PKUR.MRXX_MGT_RPT_LON MRXX
			ON MRXX.BF_SSN = PDXX.DF_PRS_ID
	WHERE
		AYXX.PF_REQ_ACT = ''DICSK''
		AND
		(
			LNXX.LF_DOE_SCL_ORG = ''XXXXXXXX''
			OR
			MRXX.LF_DOE_SCL_ENR_CUR = ''XXXXXXXX''
		)
')