SELECT * FROM OPENQUERY (LEGEND,'
	SELECT
		PDXX.DF_SPE_ACC_ID,
		LNXX.*
	FROM
		PKUB.LNXX_FIN_ATY LNXX
		INNER JOIN PKUB.PDXX_PRS_NME PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	WHERE
		PDXX.DF_SPE_ACC_ID = ''XXXXXXXXXX''
		AND LNXX.LN_SEQ = ''X''
');

SELECT * FROM OPENQUERY (LEGEND,'
	SELECT
		PDXX.DF_SPE_ACC_ID,
		LNXX.*
	FROM
		PKUB.LNXX_DSB_FIN_TRX LNXX
		INNER JOIN PKUB.PDXX_PRS_NME PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	WHERE
		PDXX.DF_SPE_ACC_ID = ''XXXXXXXXXX''
		AND LNXX.LN_SEQ = ''X''
');