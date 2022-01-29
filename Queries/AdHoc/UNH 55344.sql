USE UDW
GO

SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DM_PRS_1,
	PD10.DM_PRS_LST,
	PD30.DM_FGN_CNY
FROM
	PD10_PRS_NME PD10
	INNER JOIN LN10_LON ln10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
WHERE
	LN10.LA_CUR_PRI > 0
	AND PD30.DC_ADR = 'L'
	AND PD30.DI_VLD_ADR = 'Y'
	AND PD30.DM_FGN_CNY IN 
							(
								'Austria','Belgium','Bulgaria',
								'Croatia','Cyprus','CzechRepublic','Denmark',
								'Estonia','Finland','France','Germany',
								'Greece','Hungary','Ireland','Italy','Latvia',
								'Lithuania','Luxembourg','Malta','Netherlands',
								'Poland','Portugal','Romania','Slovakia','Slovenia',
								'Spain','Sweden','UnitedKingdom'
							)
