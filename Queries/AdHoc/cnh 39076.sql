USE CDW
GO

SELECT
	PD10.DM_PRS_1,
	PD10.DM_PRS_LST,
	PD10.DF_PRS_ID, 
	AY10.PF_REQ_ACT
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN CDW..AY10_BR_LON_ATY AY10
		ON AY10.BF_SSN = LN10.BF_SSN
		and AY10.PF_REQ_ACT = 'DIFCR'
WHERE
	LN10.LF_DOE_SCL_ORG IN ('02243000')
