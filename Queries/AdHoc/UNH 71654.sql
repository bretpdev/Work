USE UDW
GO

SELECT DISTINCT
	RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [NAME],
	LN10.BF_SSN,
	AY10.LD_ATY_REQ_RCV,
	ay10.LF_USR_REQ_ATY
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..AY10_BR_LON_ATY AY10
		ON AY10.BF_SSN = LN10.BF_SSN
		AND AY10.LC_STA_ACTY10 = 'A'
		AND AY10.LD_ATY_REQ_RCV BETWEEN '01/01/2021' AND '05/01/2021'
	INNER JOIN UDW..AY15_ATY_CMT AY15
		ON AY15.BF_SSN = AY10.BF_SSN
		AND AY15.LN_ATY_SEQ = AY10.LN_ATY_SEQ
		AND AY15.LC_STA_AY15 = 'A'
	INNER JOIN UDW..AY20_ATY_TXT AY20
		ON AY20.BF_SSN = AY10.BF_SSN
		AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
WHERE
	AY20.LX_ATY LIKE '%credit%'
	OR AY20.LX_ATY LIKE '%negative%'
	OR AY20.LX_ATY LIKE '%negative reporting%' 
	