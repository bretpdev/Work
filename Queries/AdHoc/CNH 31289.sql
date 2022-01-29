SELECT
	*
FROM
	OPENQUERY
	(LEGEND,
		'
			SELECT
				*
			FROM
				PKUB.LKXX_LS_CDE_LKP
			WHERE
				PM_ATR IN (''LC-DFR-DNL-REA'',''LC-FOR-DNL-REA'')
		'
	)