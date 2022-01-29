USE UDW;
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID
	,AY10.PF_REQ_ACT
	,AY10.LD_ATY_REQ_RCV
	,AY20.LX_ATY
FROM
	AY10_BR_LON_ATY AY10
	INNER JOIN AY20_ATY_TXT AY20
		ON AY10.BF_SSN = AY20.BF_SSN
		AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
	INNER JOIN PD10_PRS_NME PD10
		ON AY10.BF_SSN = PD10.DF_PRS_ID
WHERE
	AY10.PF_REQ_ACT = 'P200C'
	AND AY10.LD_ATY_REQ_RCV >= CONVERT(DATE,'20180618') 
	AND AY10.LD_ATY_REQ_RCV <= CONVERT(DATE,'20181231')
	AND (
			AY20.LX_ATY LIKE '%LOWER PAYMENT%'
			OR AY20.LX_ATY LIKE '%CANT MAKE PAYMENT%'
			OR AY20.LX_ATY LIKE '%PAST DUE%'
			OR AY20.LX_ATY LIKE '%DELQ%'
			OR AY20.LX_ATY LIKE '%UNABLE TO MAKE PAYMENT%'
			OR AY20.LX_ATY LIKE '%FIN DIFF%'
		)
ORDER BY
	AY10.LD_ATY_REQ_RCV
	,DF_SPE_ACC_ID;