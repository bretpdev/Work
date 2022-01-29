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
				PKUB.RSXX_IBR_RPS RSXX
			WHERE
				BF_SSN IN 
				(
					SELECT DISTINCT
						PDXX.DF_PRS_ID
					FROM
						PKUB.PDXX_PRS_NME PDXX
					WHERE
						PDXX.DF_SPE_ACC_ID IN
						(
							 ''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
						)
				)

		'
	) ;


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
				PKUB.RSXX_BR_RPD RSXX
			WHERE
				BF_SSN IN 
				(
					SELECT DISTINCT
						PDXX.DF_PRS_ID
					FROM
						PKUB.PDXX_PRS_NME PDXX
					WHERE
						PDXX.DF_SPE_ACC_ID IN
						(
							 ''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
						)
				)

		'
	) ;



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
				PKUB.RSXX_IBR_IRL_LON RSXX
			WHERE
				BF_SSN IN 
				(
					SELECT DISTINCT
						PDXX.DF_PRS_ID
					FROM
						PKUB.PDXX_PRS_NME PDXX
					WHERE
						PDXX.DF_SPE_ACC_ID IN
						(
							 ''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
						)
				)

		'
	) ;

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
				PKUB.LNXX_LON_RPS LNXX
			WHERE
				BF_SSN IN 
				(
					SELECT DISTINCT
						PDXX.DF_PRS_ID
					FROM
						PKUB.PDXX_PRS_NME PDXX
					WHERE
						PDXX.DF_SPE_ACC_ID IN
						(
							 ''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
							,''XXXXXXXXXX''
						)
				)

		'
	) ;