SELECT
	*
FROM
	RSXX_IBR_RPS RSXX
	JOIN PDXX_PRS_NME PDXX ON RSXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'

SELECT
	*
FROM
	RSXX_BR_RPD RSXX
	JOIN PDXX_PRS_NME PDXX ON RSXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'

SELECT
	*
FROM
	OPENQUERY(LEGEND,
		'
			SELECT
				*
			FROM
				PKUB.RSXX_IBR_IRL_LON RSXX
				JOIN PKUB.PDXX_PRS_NME PDXX ON RSXX.BF_SSN = PDXX.DF_PRS_ID
			WHERE
				PDXX.DF_SPE_ACC_ID = ''XXXXXXXXXX''
		'
	)

SELECT
	*
FROM
	LNXX_LON_RPS LNXX
	JOIN PDXX_PRS_NME PDXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'