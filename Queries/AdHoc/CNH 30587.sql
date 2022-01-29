SELECT 
	*
FROM OPENQUERY
(
	LEGEND,
	'
		SELECT 
			*
		FROM
			PKUB.TXXX_FAT_RUL_CRF
	'
)


SELECT 
	*
FROM OPENQUERY
(
	LEGEND,
	'
		SELECT 
			*
		FROM
			PKUB.MRXX_TAX_YR_FRM
	'
)