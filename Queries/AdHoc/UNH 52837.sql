SELECT
	TOP 1000 *
FROM
	OPENQUERY
	(DUSTER,
		'
			SELECT
				*
			FROM
				OLWHRM1.WQ21_TSK_QUE_HST
			WHERE
				WD_INI_TSK = ''08/15/2017''
		'
	)