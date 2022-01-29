


SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT DISTINCT 
				LNXX.BF_SSN,
				LNXX.LN_SEQ
			FROM     
				PKUB.LNXX_LON LNXX
			WHERE
				LNXX.LF_DOE_SCL_ORG = ''XXXXXXXX''
		'
	)
	