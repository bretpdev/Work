SELECT
	PD10.DF_SPE_ACC_ID,
	LN50.LN_SEQ,
	DF10.LC_DFR_TYP,
	LN50.LD_DFR_BEG,
	LN50.LD_DFR_END,
	DF10.LF_DFR_CTL_NUM
FROM 
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..DF10_BR_DFR_REQ DF10 
		ON DF10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..LN50_BR_DFR_APV LN50
		ON LN50.BF_SSN = DF10.BF_SSN 
		AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
WHERE
	DF10.LC_DFR_STA = 'A'
	AND DF10.LC_STA_DFR10 = 'A'
	AND LN50.LC_STA_LON50 = 'A'
	AND DF10.LC_DFR_TYP IN('38','40')
	AND 
	(
		(LN50.LD_DFR_BEG <= '2013-10-01' AND LN50.LD_DFR_END >= '2013-10-01')  --begins before, ends during or after
		OR (LN50.LD_DFR_BEG BETWEEN '2013-10-01' AND '2015-09-30' )  --begins during, ends sometime
	)
ORDER BY
	PD10.DF_SPE_ACC_ID,
	LN50.LN_SEQ,
	DF10.LC_DFR_TYP,
	LF_DFR_CTL_NUM