SELECT
	*
FROM
	OPENQUERY
	(LEGEND,
	'
		SELECT
			*
		FROM
			PKUB.LPXX_RPY_PAR
		WHERE
			PC_STA_LPDXX = ''A''
	'
);
			
