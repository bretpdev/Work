SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	LN60.LN_SEQ,
	ISNULL(LK10.PX_DSC_LNG, FB10.LC_FOR_TYP) AS FORB_TYPE,
	CONVERT(VARCHAR(10), LN60.LD_FOR_BEG, 101) AS FORB_BEGIN,
	CONVERT(VARCHAR(10), LN60.LD_FOR_END, 101) AS FORB_END
FROM
	UDW..FB10_BR_FOR_REQ FB10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = FB10.BF_SSN
	INNER JOIN UDW..LN60_BR_FOR_APV LN60
		ON LN60.BF_SSN = FB10.BF_SSN
		AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
	INNER JOIN UDW..LK10_LS_CDE_LKP LK10
		ON LK10.PM_ATR = 'LC-FOR-TYP'
		AND LK10.PX_ATR_VAL = FB10.LC_FOR_TYP
WHERE
	FB10.LC_FOR_TYP IN ('05','31','34')
	AND LN60.LD_FOR_APL BETWEEN '06/01/2021' AND '07/31/2021'
	AND FB10.LC_FOR_STA = 'A'
	AND FB10.LC_STA_FOR10 = 'A'
	AND LN60.LC_STA_LON60 = 'A'
	AND LN60.LC_FOR_RSP != '003'