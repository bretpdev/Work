USE CDW
GO

SELECT 
	*
FROM
	OPENQUERY(DUSTER,
	'
		SELECT
			*
		FROM
			OLWHRM1.PD30_PRS_ADR
		WHERE
			DI_VLD_ADR = ''Y''
			AND 
			DM_FGN_CNY IN (''AUSTRIA'',
							''BELGIUM'',
							''BULGARIA'',
							''CROATIA'',
							''CYPRUS'',
							''CZECHREPUBLIC'',
							''DENMARK'',
							''ESTONIA'',
							''FINLAND'',
							''FRANCE'',
							''GERMANY'',
							''GREECE'',
							''HUNGARY'',
							''IRELAND'',
							''ITALY'',
							''LATVIA'',
							''LITHUANIA'',
							''LUXEMBOURG'',
							''MALTA'',
							''NETHERLANDS'',
							''POLAND'',
							''PORTUGAL'',
							''ROMANIA'',
							''SLOVAKIA'',
							''SLOVENIA'',
							''SPAIN'',
							''SWEDEN'',
							''UNITED KINGDOM''
							)

						AND DF_PRS_ID NOT LIKE ''P%''




	')