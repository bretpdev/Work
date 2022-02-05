CREATE PROCEDURE [dashcache].[AUTODEMO_UHEAA]
AS

	SELECT
		SUM(UnworkedCount)
	FROM
		OPENQUERY
		(
			DUSTER,
			'
				SELECT
					COUNT(*) "UnworkedCount"
				FROM
					OLWHRM1.WQ20_TSK_QUE WQ20
				WHERE
					WQ20.WF_QUE IN (''1E'', ''DO'', ''LX'')
					AND
					WQ20.WF_SUB_QUE = ''01''
					AND
					WQ20.WC_STA_WQUE20 = ''U''
					AND
					DAYS(WQ20.WD_ACT_REQ) < DAYS(CURRENT DATE) -4

				UNION ALL

				SELECT
					COUNT(*)
				FROM
					OLWHRM1.CT30_CALL_QUE CT30
				WHERE
					CT30.IC_TSK_STA = ''A''
					AND
					COALESCE(TRIM(CT30.IF_LST_USR_CT30), '''') = ''''
					AND
					DAYS(CT30.IF_CRT_DTS_CT30) < DAYS(CURRENT DATE) -4
					AND
					(
						(
							CT30.IF_WRK_GRP IN 
							(				
								''SRMNOADD'',
								''XDEMOE'',
								''XDEMOG'',
								''DEMOUPDT'',
								''PENDPDEM''
							)
							AND
							CT30.IC_REC_TYP = ''ALL''
						)
						OR
						(
							CT30.IF_WRK_GRP IN 
							(				
								''ACURINTR'',
								''DBADPO'',
								''PDEMREVW'',
								''SKIPCHNG''
							)
							AND
							CT30.IC_REC_TYP = ''SKP''
						)
					)
			'
		)


RETURN 0