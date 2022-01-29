USE CDW
GO

SELECT 
	*
FROM
	OPENQUERY(LEGEND,
	'
		SELECT
			count(distinct df_prs_id)
		FROM
			PKUB.PDXX_PRS_ADR
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