
DROP TABLE IF EXISTS #COMPASS
DROP TABLE IF EXISTS #ONELINK

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [NAME],
	PD24.DD_BKR_VER AS APPLIED_DATE,
	PD24.DF_RSU_REA_LST_USR AS PROCESSOR,
	DD_BKR_DCH_RCV AS DISCHARGE_DATE,
	FORB.LD_FOR_BEG AS FORB_BEGIN,
	FORB.LD_FOR_END AS FORB_END,
	FORB.PX_DSC_LNG AS CAP_TYPE
INTO #COMPASS
FROM 
	[UDW].[DBO].[PD24_PRS_BKR] pd24
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD24.DF_PRS_ID
	INNER JOIN
			(
				SELECT DISTINCT
					FB10.LC_FOR_TYP,
					LN60.LD_FOR_BEG,
					LN60.LD_FOR_END,
					LN60.BF_SSN,
					LK10.PX_DSC_LNG
				FROM
					FB10_BR_FOR_REQ FB10
					INNER JOIN LN60_BR_FOR_APV LN60
						ON LN60.BF_SSN = FB10.BF_SSN
						AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					LEFT JOIN UDW..LK10_LS_CDE_LKP LK10
						ON LK10.PM_ATR = 'LC-LON-LEV-FOR-CAP'
						AND LK10.PX_ATR_VAL = LC_LON_LEV_FOR_CAP
					INNER JOIN 
					(
						SELECT
							MAX(LN60.LF_FOR_CTL_NUM) AS LF_FOR_CTL_NUM,
							LN60.BF_SSN
						FROM
							LN60_BR_FOR_APV LN60
							INNER JOIN UDW..FB10_BR_FOR_REQ FB10
								ON FB10.BF_SSN = LN60.BF_SSN
								AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
						GROUP BY
							LN60.BF_SSN
					) LN60Max
						ON LN60.BF_SSN = LN60Max.BF_SSN
						AND LN60Max.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					--AND FB10.LC_FOR_STA = 'A' 
			) Forb
				ON Forb.BF_SSN = PD24.DF_PRS_ID
				
WHERE
	--DC_BKR_STA IN ('04','06')
	--OR
	--(
	--	DC_BKR_STA = '05'
		 DD_BKR_FIL > '03/01/2021'
	--)

--union

SELECT 
	PD01.DF_SPE_ACC_ID,
	RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) AS [NAME],
	DC01.LD_DCO AS APPLIED_DATE,
	DC01.BF_USR_CRT_DC01 AS PROCESSOR,
	CASE WHEN LC_STA_DC10 = '04' THEN LF_LST_DTS_DC10 ELSE NULL END AS DISCHARGE_DATE,
	NULL AS FORB_BEGIN,
	NULL AS FORB_END,
	'' AS CAP_TYPE
INTO #ONELINK
FROM 
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_PRS_ID = DC01.BF_SSN
WHERE
	--(
	--	LC_PCL_REA IN ('BC','BH','BO')
	--	AND 
	--	LC_STA_DC10 = '03'
	--)
	--OR
	--(
		LC_PCL_REA IN ('BC','BH','BO')
		--AND 
		--LC_STA_DC10 = '04'
		AND 
		LD_PCL_RCV >= '03/01/2021'
	--)
ORDER BY PD01.DF_SPE_ACC_ID
--bf_ssn = '529176434'


SELECT
	*
FROM
	#ONELINK

UNION

SELECT
	C.*
FROM
	#COMPASS C
	LEFT JOIN #ONELINK O
		ON O.DF_SPE_ACC_ID = C.DF_SPE_ACC_ID
WHERE
	O.DF_SPE_ACC_ID IS NULL