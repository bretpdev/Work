SELECT * FROM OPENQUERY (DUSTER, '

	SELECT
		*
	FROM
		OLWHRM1.LK10_LS_CDE_LKP
	WHERE
		PM_ATR = ''WC-CRD-BUR-CUST-ID''

');