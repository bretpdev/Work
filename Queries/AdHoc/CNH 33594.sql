USE CDW
GO

SELECT DISTINCT
	PDXX.DF_PRS_ID,
	PDXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	PDXX.DM_FGN_CNY
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON lnXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
WHERE
	LNXX.LA_CUR_PRI > X
	AND PDXX.DC_ADR = 'L'
	AND PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DM_FGN_CNY IN 
							(
								'Austria','Belgium','Bulgaria',
								'Croatia','Cyprus','CzechRepublic','Denmark',
								'Estonia','Finland','France','Germany',
								'Greece','Hungary','Ireland','Italy','Latvia',
								'Lithuania','Luxembourg','Malta','Netherlands',
								'Poland','Portugal','Romania','Slovakia','Slovenia',
								'Spain','Sweden','UnitedKingdom'
							)
