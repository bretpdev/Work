SELECT
	*
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				*
			FROM 
				OLWHRM1.RS05_IBR_RPS RS05
			WHERE
				BF_SSN IN 
				(
					SELECT DISTINCT
						PD10.DF_PRS_ID
					FROM
						OLWHRM1.PD10_PRS_NME PD10
					WHERE
						PD10.DF_SPE_ACC_ID IN
						(
							 ''4910002870''
							,''7151466494''
							,''3641438242''
							,''4890486824''
							,''5797077353''
							,''4447784421''
							,''4614796672''
							,''8701519170''
						)
				)

		'
	) ;

SELECT
	*
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				*
			FROM 
				OLWHRM1.RS10_BR_RPD RS10
			WHERE
				BF_SSN IN 
				(
					SELECT DISTINCT
						PD10.DF_PRS_ID
					FROM
						OLWHRM1.PD10_PRS_NME PD10
					WHERE
						PD10.DF_SPE_ACC_ID IN
						(
							 ''4910002870''
							,''7151466494''
							,''3641438242''
							,''4890486824''
							,''5797077353''
							,''4447784421''
							,''4614796672''
							,''8701519170''
						)
				)

		'
	) ;


SELECT
	*
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				*
			FROM 
				OLWHRM1.LN65_LON_RPS LN65
			WHERE
				BF_SSN IN 
				(
					SELECT DISTINCT
						PD10.DF_PRS_ID
					FROM
						OLWHRM1.PD10_PRS_NME PD10
					WHERE
						PD10.DF_SPE_ACC_ID IN
						(
							 ''4910002870''
							,''7151466494''
							,''3641438242''
							,''4890486824''
							,''5797077353''
							,''4447784421''
							,''4614796672''
							,''8701519170''
						)
				)

		'
	) ;
