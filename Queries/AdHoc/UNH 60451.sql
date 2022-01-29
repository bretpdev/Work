SELECT
	RS05.*
FROM
	UDW..RS05_IBR_RPS RS05
	INNER JOIN PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = RS05.BF_SSN
WHERE
	PD10.DF_SPE_ACC_ID = '4828086141'
;

SELECT
	RS10.*
FROM
	UDW..RS10_BR_RPD RS10
	INNER JOIN PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = RS10.BF_SSN
WHERE
	PD10.DF_SPE_ACC_ID = '4828086141'
;

SELECT
	LN65.*
FROM
	UDW..LN65_LON_RPS LN65
	INNER JOIN PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN65.BF_SSN
WHERE
	PD10.DF_SPE_ACC_ID = '4828086141'
;