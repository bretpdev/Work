DROP TABLE IF EXISTS #POP
DROP TABLE IF EXISTS #FINAL
SELECT
	*
INTO #POP
FROM
	UDW..LT20_LTR_REQ_PRC
WHERE
	CreatedAt BETWEEN '06/07/2021' AND '06/13/2021'
	AND RM_APL_PGM_PRC = 'TSX08'
	AND RN_SEQ_LTR_CRT_PRC = 1
	AND RN_SEQ_REC_PRC = 1
	AND CONVERT(TIME, RT_RUN_SRT_DTS_PRC) = '00:00:00.0000000' 

SELECT
	P.*
INTO #FINAL
FROM
	#POP P
	LEFT JOIN UDW..LT20_LTR_REQ_PRC LT20
		ON LT20.DF_SPE_ACC_ID = P.DF_SPE_ACC_ID 
		AND LT20.RM_DSC_LTR_PRC = P.RM_DSC_LTR_PRC
		AND LT20.RN_ATY_SEQ_PRC = P.RN_ATY_SEQ_PRC
		AND LT20.CreatedAt BETWEEN '06/07/2021' AND '06/13/2021'
		AND CONVERT(TIME, LT20.RT_RUN_SRT_DTS_PRC) != '00:00:00.0000000' 
WHERE
	LT20.DF_SPE_ACC_ID IS NULL

SELECT DISTINCT
	F.DF_SPE_ACC_ID,
	F.RN_ATY_SEQ_PRC,
	F.RM_DSC_LTR_PRC
FROM
	#FINAL F
	INNER JOIN UDW..AY10_BR_LON_ATY AY10
		ON AY10.BF_SSN = F.RF_SBJ_PRC
		AND AY10.LN_ATY_SEQ = F.RN_ATY_SEQ_PRC
		AND LC_STA_ACTY10 = 'A'
		AND PF_RSP_ACT != 'CANCL'