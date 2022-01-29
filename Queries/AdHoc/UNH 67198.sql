USE UDW
GO

SELECT 
	PD10.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	FB10.LF_USR_CRT_REQ_FOR AS [USER],
	FB10.LD_CRT_REQ_FOR AS CREATE_DATE,
	RTRIM(LK10.PX_DSC_LNG) + ' FORBEARANCE' AS [TYPE]
FROM
	UDW..FB10_BR_FOR_REQ FB10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = FB10.BF_SSN	
	INNER JOIN UDW..LK10_LS_CDE_LKP LK10
		ON LK10.PM_ATR = 'LC-FOR-TYP'
		AND LK10.PX_ATR_VAL = FB10.LC_FOR_TYP
WHERE
	FB10.LF_USR_CRT_REQ_FOR IN
	(
		 'UT02952',
		 'UT02945',
		 'UT02947',
		 'UT02946'
	)
	AND LD_CRT_REQ_FOR BETWEEN '05-04-2020' AND '05-08-2020'

UNION


SELECT 
	PD10.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	DF10.LF_USR_CRT_REQ_DFR AS [USER],
	DF10.LD_CRT_REQ_DFR AS CREATE_DATE,
	RTRIM(LK10.PX_DSC_LNG) + ' DEFERMENT' AS [TYPE]
FROM
	UDW..DF10_BR_DFR_REQ DF10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = DF10.BF_SSN	
	INNER JOIN UDW..LK10_LS_CDE_LKP LK10
		ON LK10.PM_ATR = 'LC-DFR-TYP'
		AND LK10.PX_ATR_VAL = DF10.LC_DFR_TYP
WHERE
	DF10.LF_USR_CRT_REQ_DFR IN
	(
		 'UT02952',
		 'UT02945',
		 'UT02947',
		 'UT02946'
	)
	AND LD_CRT_REQ_DFR BETWEEN '05-04-2020' AND '05-08-2020'

ORDER BY
	CREATE_DATE