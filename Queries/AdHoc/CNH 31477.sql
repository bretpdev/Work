SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				*
			FROM
				PKUB.LPXX_RPY_PAR LPXX
			WHERE
				LPXX.PC_STA_LPDXX = ''A''
				
		'
	)