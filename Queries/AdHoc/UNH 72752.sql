--Population 1 includes outstanding loans on which a default claim was paid prior to March 13, 2020, 
--that are not subject to an active bankruptcy filing,and are still in default as of 05/24/2021.


DROP TABLE IF EXISTS #POP2
SELECT DISTINCT
	DC01.AF_APL_ID,
	DC01.AF_APL_ID_SFX,
	DC01.LF_CRT_DTS_DC10,
	BF_SSN
INTO #POP2
FROM
	ODW..DC01_LON_CLM_INF DC01
WHERE
    DC01.LC_STA_DC10 = '03'
    AND DC01.LD_LDR_POF >= '03/13/2020'
    AND DC01.LC_PCL_REA IN ('DB','DF','IN')



SELECT DISTINCT
	BF_SSN,
	AF_APL_ID,
	AF_APL_ID_SFX,
	LC_INI_CLM_ELG_RIN,
	LD_LDR_POF AS LENDER_PAYOFF,
	HAS_PMT AS BR_PAYMENT,
	LR_CUR_INT CURRENT_INTEREST,
	
	ISNULL(LA_CLM_BAL,0.00) - ISNULL(LA_CLM_PRJ_COL_CST,0.00) AS PRINCIPAL_INTEREST
FROM
(
SELECT 
	CASE WHEN DC02.LR_CUR_INT = 0 THEN 1 ELSE 0 END AS CUR0,
	CASE WHEN DC02.LR_CUR_INT != 0 THEN 1 ELSE 0 END AS CUR1,
	DC02.*,
	dc01.LD_LDR_POF,
	DC01.LC_INI_CLM_ELG_RIN,
	CASE WHEN PMT.AF_APL_ID IS NULL THEN 'N' ELSE 'Y' END AS HAS_PMT
FROM 
	#POP2 P
	INNER JOIN 
	(
		SELECT
			DC01.*
		FROM
			ODW..DC01_LON_CLM_INF DC01
			INNER JOIN 
			(
				SELECT DISTINCT
					DC01.BF_SSN,
					DC01.AF_APL_ID,
					DC01.AF_APL_ID_SFX,
					MAX(DC01.LF_CRT_DTS_DC10) AS MaxDate
				FROM
					ODW..DC01_LON_CLM_INF DC01
				GROUP BY
					DC01.BF_SSN,
					DC01.AF_APL_ID,
					DC01.AF_APL_ID_SFX
				) DC01M
					ON DC01.AF_APL_ID = DC01M.AF_APL_ID
					AND DC01.AF_APL_ID_SFX = DC01M.AF_APL_ID_SFX
					AND DC01.LF_CRT_DTS_DC10 = DC01M.MaxDate
			WHERE
					DC01.LC_PCL_REA = 'DF'
					AND DC01.LC_STA_DC10 = '03'
					AND DC01.LD_CLM_ASN_DOE IS NULL
		
					AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''
	) DC01
		ON DC01.BF_SSN = P.BF_SSN
		AND DC01.AF_APL_ID = P.AF_APL_ID
		AND DC01.AF_APL_ID_SFX = P.AF_APL_ID_SFX
	INNER JOIN ODW..DC02_BAL_INT DC02 --STILL CURRENTLY IN DEFAULT
		ON DC02.AF_APL_ID = P.AF_APL_ID
		AND DC02.AF_APL_ID_SFX = P.AF_APL_ID_SFX
		AND DC02.LF_CRT_DTS_DC10 = P.LF_CRT_DTS_DC10
		AND DC02.LA_CLM_BAL > 0.00
	LEFT JOIN 
	(
			SELECT 
			AF_APL_ID, 
			AF_APL_ID_SFX, 
			SUM(ISNULL(LA_PRI_AT_PST,0.00)) AS PRIN ,
			SUM(ISNULL(LA_INT_ACR_THS_PRD,0.00)) AS INTEREST 
		FROM 
			ODW..DC11_LON_FAT 
		WHERE 
			LC_TRX_TYP IN ('BR') 
			AND LD_TRX_EFF >= '03/13/2020' 
			AND LC_REV_IND_TYP = ''
		GROUP BY
			AF_APL_ID, 
			AF_APL_ID_SFX
	) PMT
		ON PMT.AF_APL_ID = DC01.AF_APL_ID
		AND PMT.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX

) P



--SELECT
--*
--FROM
--(
--SELECT DISTINCT
--	DC01.BF_SSN,
--	DC01.AF_APL_ID,
--	DC01.AF_APL_ID_SFX,
--	CASE WHEN LR_CUR_INT = 0 THEN 1 ELSE 0 END AS CUR_0,
--	CASE WHEN LR_CUR_INT != 0 THEN 1 ELSE 0 END AS CUR_NO_0
--FROM 
--	ODW..DC01_LON_CLM_INF DC01
--	INNER JOIN 
--	(
--		SELECT DISTINCT
--			DC01.BF_SSN,
--			DC01.AF_APL_ID,
--			DC01.AF_APL_ID_SFX,
--			MAX(DC01.LF_CRT_DTS_DC10) AS MaxDate
--		FROM
--			ODW..DC01_LON_CLM_INF DC01
--		GROUP BY
--			DC01.BF_SSN,
--			DC01.AF_APL_ID,
--			DC01.AF_APL_ID_SFX
--		) DC01M
--			ON DC01.AF_APL_ID = DC01M.AF_APL_ID
--			AND DC01.AF_APL_ID_SFX = DC01M.AF_APL_ID_SFX
--			AND DC01.LF_CRT_DTS_DC10 = DC01M.MaxDate
--	INNER JOIN ODW..DC02_BAL_INT DC02 --STILL CURRENTLY IN DEFAULT
--		ON DC02.AF_APL_ID = DC01.AF_APL_ID
--		AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
--		AND DC02.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
--		AND DC02.LA_CLM_BAL > 0.00
--WHERE
--	DC01.LC_PCL_REA = 'DF'
--	AND DC01.LC_STA_DC10 = '03'
--	AND DC01.LD_CLM_ASN_DOE IS NULL

--	AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''
--) POP