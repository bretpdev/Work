SELECT DISTINCT
	LN90.BF_SSN,
	CONVERT(VARCHAR, LN90.LD_FAT_EFF, 101) as INTEREST_CAP_EFF_DATE,
	CASE WHEN LT20.DF_SPE_ACC_ID IS NOT NULL OR PP.AccountNumber IS NOT NULL THEN 'Y' ELSE 'N' END AS HAS_LETTER,
	COALESCE(LT20.RM_DSC_LTR_PRC, L.LETTER, '') AS letterIdGenerated
FROM
	UDW..LN90_FIN_ATY LN90
	INNER JOIN uDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN90.BF_SSN
	LEFT JOIN UDW..LT20_LTR_REQ_PRC LT20
		ON LT20.RF_SBJ_PRC = LN90.BF_SSN
		AND LT20.RM_DSC_LTR_PRC IN 
		(
			'INTCAPCAS',
			'INTCAPARS',
			'US06BCAP'
		)
		--= 'US06BCAP'
		AND LN90.LD_FAT_APL <= CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
	LEFT JOIN ULS.[print].PrintProcessing PP
		ON PP.AccountNumber = PD10.DF_SPE_ACC_ID
		and pp.ScriptDataId in (199)
		and pp.AddedAt >= LN90.LD_FAT_APL
	LEFT JOIN ULS.[print].ScriptData SD
		ON SD.ScriptDataId = PP.ScriptDataId
	LEFT JOIN ULS.[print].Letters L
		ON L.LetterId = SD.LetterId
WHERE
	PC_FAT_TYP = '70'
	AND PC_FAT_SUB_TYP = '01'
	AND LC_STA_LON90 = 'A'
	AND ISNULL(LC_FAT_REV_REA,'') = ''
	AND LN90.LD_FAT_EFF >= '06/01/2021'


SELECT DISTINCT 
	RM_DSC_LTR_PRC, 
	COUNT(RM_DSC_LTR_PRC) 
FROM 
	UDW..LT20_LTR_REQ_PRC 
WHERE
	RM_DSC_LTR_PRC IN 
	(
		'INTCAPCAS',
		'INTCAPARS',
		'US06BCAP'
	)
GROUP BY 
	RM_DSC_LTR_PRC