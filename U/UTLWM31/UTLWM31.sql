USE ODW
GO
DROP TABLE IF EXISTS #DEMOS;
DROP TABLE IF EXISTS #DEMOSEndorser;

DECLARE @R2 INT = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = 'DANOPHND1'),
		@R3 INT = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = 'DANOPHND2'),
		@R4 INT = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = 'DANOPHND3'),
		@R5 INT = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = 'DANOPHND4'),
		@R6 INT = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = 'DANOPHND5'),
		@R7 INT = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = 'DANOPHND6');
DECLARE @ScriptId VARCHAR(10) = 'SCLATESTG';
DECLARE @ScriptDataIdBorrowerR2 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R2 AND IsEndorser = 0),
		@ScriptDataIdEndorserR2 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R2 AND IsEndorser = 1),
		@ScriptDataIdBorrowerR3 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R3 AND IsEndorser = 0),
		@ScriptDataIdEndorserR3 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R3 AND IsEndorser = 1),
		@ScriptDataIdBorrowerR4 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R4 AND IsEndorser = 0),
		@ScriptDataIdEndorserR4 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R4 AND IsEndorser = 1),
		@ScriptDataIdBorrowerR5 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R5 AND IsEndorser = 0),
		@ScriptDataIdEndorserR5 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R5 AND IsEndorser = 1),
		@ScriptDataIdBorrowerR6 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R6 AND IsEndorser = 0),
		@ScriptDataIdEndorserR6 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R6 AND IsEndorser = 1),
		@ScriptDataIdBorrowerR7 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R7 AND IsEndorser = 0),
		@ScriptDataIdEndorserR7 INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND LetterId = @R7 AND IsEndorser = 1);

--SELECT 
--	@R2, @R3, @R4, @R5, @R6, @R7, 
--	@ScriptDataIdBorrowerR2, @ScriptDataIdBorrowerR3, @ScriptDataIdBorrowerR4, @ScriptDataIdBorrowerR5, @ScriptDataIdBorrowerR6, @ScriptDataIdBorrowerR7,
--	@ScriptDataIdEndorserR2, @ScriptDataIdEndorserR3, @ScriptDataIdEndorserR4, @ScriptDataIdEndorserR5, @ScriptDataIdEndorserR6, @ScriptDataIdEndorserR7

SELECT DISTINCT 
	DC01.BF_SSN,
	PD01.DF_SPE_ACC_ID,
	PD01.DF_PRS_ID,
	RTRIM(PD01.DM_PRS_1) AS DM_PRS_1,
	RTRIM(PD01.DM_PRS_LST) AS DM_PRS_LST,
	RTRIM(PD01.DX_STR_ADR_1) AS DX_STR_ADR_1,
	RTRIM(PD01.DX_STR_ADR_2) AS DX_STR_ADR_2,
	RTRIM(PD01.DM_CT) AS DM_CT,
	RTRIM(PD01.DC_DOM_ST) AS DC_DOM_ST,
	RTRIM(PD01.DF_ZIP) AS DF_ZIP,
	RTRIM(PD01.DM_FGN_CNY) AS DM_FGN_CNY,
	MAX(DATEDIFF(DAY,DC01Delq.LD_DCO,GETDATE())) OVER(PARTITION BY DC01.BF_SSN) AS DAYS_DLQ,
	CASE WHEN AY01.ALTS1 != 0 THEN 'X' ELSE NULL END AS ALTS1,
	CASE WHEN AY01.ALTS2 != 0 THEN 'X' ELSE NULL END AS ALTS2,
	CASE WHEN AY01.ALTT1 != 0 THEN 'X' ELSE NULL END AS ALTT1,
	CASE WHEN AY01.ALTT2 != 0 THEN 'X' ELSE NULL END AS ALTT2,
	CASE WHEN AY01.ALTV1 != 0 THEN 'X' ELSE NULL END AS ALTV1,
	CASE WHEN AY01.ALTV2 != 0 THEN 'X' ELSE NULL END AS ALTV2,
	PD01.DC_ADR,
	CASE WHEN ISNULL(PD01.DM_FGN_CNY,'') != '' THEN ''
		 WHEN ISNULL(PD01.DM_FGN_CNY,'') = '' THEN RTRIM(PD01.DC_DOM_ST)
	END AS STATE_IND,
	'MA2329' AS CCC,
	'00' AS Rfile,
	0 AS LetterId,
	0 AS ScriptDataId
INTO
	#DEMOS
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..GA15_NDS_ID GA15
		ON GA15.DF_PRS_ID_STU_NDS = DC01.BF_SSN
		AND GA15.AF_APL_ID = DC01.AF_APL_ID
		AND GA15.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND GA15.AD_NDS_CLC_ENT_RPD < CAST(DATEADD(YEAR,-2,GETDATE()) AS DATE)
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_PRS_ID = DC01.BF_SSN
		AND PD01.DI_PHN_VLD = 'N'
		AND PD01.DI_VLD_ADR = 'Y'
		AND PD01.DC_ADR = 'L'
	INNER JOIN
	(
		SELECT
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			MAX(DC01.LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
		FROM
			ODW..DC01_LON_CLM_INF DC01
		GROUP BY
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX
	) MaxDC01
		ON MaxDC01.BF_SSN = DC01.BF_SSN
		AND MaxDC01.AF_APL_ID = DC01.AF_APL_ID
		AND MaxDC01.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND MaxDC01.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
	INNER JOIN
	(
		SELECT
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			DC01.LF_CRT_DTS_DC10,
			MIN(DC01.LD_DCO) AS LD_DCO
		FROM
			ODW..DC01_LON_CLM_INF DC01
		GROUP BY
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			DC01.LF_CRT_DTS_DC10
	) DC01Delq
		ON DC01Delq.BF_SSN = DC01.BF_SSN
		AND DC01Delq.AF_APL_ID = DC01.AF_APL_ID
		AND DC01Delq.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC01Delq.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
		AND DC01Delq.LD_DCO = DC01.LD_DCO
	LEFT JOIN
	(
		SELECT DISTINCT
			AY01.DF_PRS_ID,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTS1' THEN 1 ELSE 0 END) AS ALTS1,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTS2' THEN 1 ELSE 0 END) AS ALTS2,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTT1' THEN 1 ELSE 0 END) AS ALTT1,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTT2' THEN 1 ELSE 0 END) AS ALTT2,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTV1' THEN 1 ELSE 0 END) AS ALTV1,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTV2' THEN 1 ELSE 0 END) AS ALTV2
		FROM 
			ODW..AY01_BR_ATY AY01 
		WHERE 
			AY01.PF_ACT IN('ALTS1','ALTS2','ALTT1','ALTT2','ALTV1','ALTV2')
			AND AY01.BD_ATY_PRF >= CAST(DATEADD(DAY,-14,GETDATE()) AS DATE)
		GROUP BY
			AY01.DF_PRS_ID
	) AY01
		ON AY01.DF_PRS_ID = DC01.BF_SSN
WHERE 
	DC01.LC_STA_DC10 = '01'
	AND DC01.LC_PCL_REA IN ('DF','RS','DB','DQ')


SELECT DISTINCT 
	DC01.BF_SSN,
	PD01Borrower.DF_SPE_ACC_ID,
	PD01Borrower.DF_PRS_ID,
	RTRIM(PD01.DM_PRS_1) AS DM_PRS_1,
	RTRIM(PD01.DM_PRS_LST) AS DM_PRS_LST,
	RTRIM(PD01.DX_STR_ADR_1) AS DX_STR_ADR_1,
	RTRIM(PD01.DX_STR_ADR_2) AS DX_STR_ADR_2,
	RTRIM(PD01.DM_CT) AS DM_CT,
	RTRIM(PD01.DC_DOM_ST) AS DC_DOM_ST,
	RTRIM(PD01.DF_ZIP) AS DF_ZIP,
	RTRIM(PD01.DM_FGN_CNY) AS DM_FGN_CNY,
	MAX(DATEDIFF(DAY,DC01Delq.LD_DCO,GETDATE())) OVER(PARTITION BY DC01.BF_SSN) AS DAYS_DLQ,
	CASE WHEN AY01.ALTS1 != 0 THEN 'X' ELSE NULL END AS ALTS1,
	CASE WHEN AY01.ALTS2 != 0 THEN 'X' ELSE NULL END AS ALTS2,
	CASE WHEN AY01.ALTT1 != 0 THEN 'X' ELSE NULL END AS ALTT1,
	CASE WHEN AY01.ALTT2 != 0 THEN 'X' ELSE NULL END AS ALTT2,
	CASE WHEN AY01.ALTV1 != 0 THEN 'X' ELSE NULL END AS ALTV1,
	CASE WHEN AY01.ALTV2 != 0 THEN 'X' ELSE NULL END AS ALTV2,
	PD01.DC_ADR,
	CASE WHEN ISNULL(PD01.DM_FGN_CNY,'') != '' THEN ''
		 WHEN ISNULL(PD01.DM_FGN_CNY,'') = '' THEN RTRIM(PD01.DC_DOM_ST)
	END AS STATE_IND,
	'MA2329' AS CCC,
	'00' AS Rfile,
	0 AS LetterId,
	0 AS ScriptDataId,
	PD01.DF_SPE_ACC_ID AS CoBorrowerAccount
INTO
	#DEMOSEndorser
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..GA15_NDS_ID GA15
		ON GA15.DF_PRS_ID_STU_NDS = DC01.BF_SSN
		AND GA15.AF_APL_ID = DC01.AF_APL_ID
		AND GA15.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND GA15.AD_NDS_CLC_ENT_RPD < CAST(DATEADD(YEAR,-2,GETDATE()) AS DATE)
	INNER JOIN ODW..GA01_APP GA01
		ON GA01.AF_APL_ID = GA15.AF_APL_ID
		AND GA01.AC_EDS_TYP = 'C'
		AND GA01.DF_PRS_ID_BR = DC01.BF_SSN
	INNER JOIN ODW..PD01_PDM_INF PD01Borrower
		ON PD01Borrower.DF_PRS_ID = DC01.BF_SSN
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_PRS_ID = GA01.DF_PRS_ID_EDS
		AND PD01.DI_PHN_VLD = 'N'
		AND PD01.DI_VLD_ADR = 'Y'
		AND PD01.DC_ADR = 'L'
	INNER JOIN
	(
		SELECT
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			MAX(DC01.LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
		FROM
			ODW..DC01_LON_CLM_INF DC01
		GROUP BY
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX
	) MaxDC01
		ON MaxDC01.BF_SSN = DC01.BF_SSN
		AND MaxDC01.AF_APL_ID = DC01.AF_APL_ID
		AND MaxDC01.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND MaxDC01.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
	INNER JOIN
	(
		SELECT
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			DC01.LF_CRT_DTS_DC10,
			MIN(DC01.LD_DCO) AS LD_DCO
		FROM
			ODW..DC01_LON_CLM_INF DC01
		GROUP BY
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			DC01.LF_CRT_DTS_DC10
	) DC01Delq
		ON DC01Delq.BF_SSN = DC01.BF_SSN
		AND DC01Delq.AF_APL_ID = DC01.AF_APL_ID
		AND DC01Delq.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC01Delq.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
		AND DC01Delq.LD_DCO = DC01.LD_DCO
	LEFT JOIN
	(
		SELECT DISTINCT
			AY01.DF_PRS_ID,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTS1' THEN 1 ELSE 0 END) AS ALTS1,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTS2' THEN 1 ELSE 0 END) AS ALTS2,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTT1' THEN 1 ELSE 0 END) AS ALTT1,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTT2' THEN 1 ELSE 0 END) AS ALTT2,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTV1' THEN 1 ELSE 0 END) AS ALTV1,
			SUM(CASE WHEN AY01.PF_ACT = 'ALTV2' THEN 1 ELSE 0 END) AS ALTV2
		FROM 
			ODW..AY01_BR_ATY AY01 
		WHERE 
			AY01.PF_ACT IN('ALTS1','ALTS2','ALTT1','ALTT2','ALTV1','ALTV2')
			AND AY01.BD_ATY_PRF >= DATEADD(DAY,-14,GETDATE())
		GROUP BY
			AY01.DF_PRS_ID
	) AY01
		ON AY01.DF_PRS_ID = DC01.BF_SSN
WHERE 
	DC01.LC_STA_DC10 = '01'
	AND DC01.LC_PCL_REA IN ('DF','RS','DB','DQ')
;

--Set R file based on delinquency days
UPDATE
	D
SET
	D.Rfile = 
		( -- < 157 wont be late stage, > 330 is no longer late stage, but delq
			CASE WHEN D.DAYS_DLQ BETWEEN 157 AND 171 AND D.ALTS1 IS NULL THEN 'R2'
				 WHEN D.DAYS_DLQ BETWEEN 172 AND 186 AND D.ALTS2 IS NULL THEN 'R3'
				 WHEN D.DAYS_DLQ BETWEEN 187 AND 201 AND D.ALTT1 IS NULL THEN 'R4'
				 WHEN D.DAYS_DLQ BETWEEN 202 AND 216 AND D.ALTT2 IS NULL THEN 'R5'
				 WHEN D.DAYS_DLQ BETWEEN 217 AND 231 AND D.ALTV1 IS NULL THEN 'R6'
				 WHEN D.DAYS_DLQ BETWEEN 232 AND 330 AND D.ALTV2 IS NULL THEN 'R7'
				 ELSE ''
			END
		)
FROM
	#DEMOS D;

UPDATE
	D
SET
	D.LetterId = 
		(
			CASE WHEN D.Rfile = 'R2' THEN @R2
				 WHEN D.Rfile = 'R3' THEN @R3
				 WHEN D.Rfile = 'R4' THEN @R4
				 WHEN D.Rfile = 'R5' THEN @R5
				 WHEN D.Rfile = 'R6' THEN @R6
				 WHEN D.Rfile = 'R7' THEN @R7
				 ELSE ''
			END
		),
	D.ScriptDataId =
		(
			CASE WHEN D.Rfile = 'R2' THEN @ScriptDataIdBorrowerR2
				 WHEN D.Rfile = 'R3' THEN @ScriptDataIdBorrowerR3
				 WHEN D.Rfile = 'R4' THEN @ScriptDataIdBorrowerR4
				 WHEN D.Rfile = 'R5' THEN @ScriptDataIdBorrowerR5
				 WHEN D.Rfile = 'R6' THEN @ScriptDataIdBorrowerR6
				 WHEN D.Rfile = 'R7' THEN @ScriptDataIdBorrowerR7
				 ELSE ''
			END
		)
FROM
	#DEMOS D;

UPDATE
	D
SET
	D.Rfile = 
		( -- < 157 wont be late stage, > 330 is no longer late stage, but delq
			CASE WHEN D.DAYS_DLQ BETWEEN 157 AND 171 AND D.ALTS1 IS NULL THEN 'R2'
				 WHEN D.DAYS_DLQ BETWEEN 172 AND 186 AND D.ALTS2 IS NULL THEN 'R3'
				 WHEN D.DAYS_DLQ BETWEEN 187 AND 201 AND D.ALTT1 IS NULL THEN 'R4'
				 WHEN D.DAYS_DLQ BETWEEN 202 AND 216 AND D.ALTT2 IS NULL THEN 'R5'
				 WHEN D.DAYS_DLQ BETWEEN 217 AND 231 AND D.ALTV1 IS NULL THEN 'R6'
				 WHEN D.DAYS_DLQ BETWEEN 232 AND 330 AND D.ALTV2 IS NULL THEN 'R7'
				 ELSE ''
			END
		)
FROM
	#DEMOSEndorser D;

UPDATE
	D
SET
	D.LetterId = 
		(
			CASE WHEN D.Rfile = 'R2' THEN @R2
				 WHEN D.Rfile = 'R3' THEN @R3
				 WHEN D.Rfile = 'R4' THEN @R4
				 WHEN D.Rfile = 'R5' THEN @R5
				 WHEN D.Rfile = 'R6' THEN @R6
				 WHEN D.Rfile = 'R7' THEN @R7
				 ELSE ''
			END
		),
	D.ScriptDataId =
		(
			CASE WHEN D.Rfile = 'R2' THEN @ScriptDataIdEndorserR2
				 WHEN D.Rfile = 'R3' THEN @ScriptDataIdEndorserR3
				 WHEN D.Rfile = 'R4' THEN @ScriptDataIdEndorserR4
				 WHEN D.Rfile = 'R5' THEN @ScriptDataIdEndorserR5
				 WHEN D.Rfile = 'R6' THEN @ScriptDataIdEndorserR6
				 WHEN D.Rfile = 'R7' THEN @ScriptDataIdEndorserR7
				 ELSE ''
			END
		)
FROM
	#DEMOSEndorser D;

SELECT * FROM #DEMOS
SELECT * FROM #DEMOSEndorser

INSERT INTO ULS.[print].PrintProcessing
(
	AccountNumber,
	EmailAddress, 
	ScriptDataId, 
	SourceFile, 
	LetterData, 
	CostCenter, 
	InValidAddress, 
	DoNotProcessEcorr, 
	OnEcorr, 
	ArcNeeded, 
	ImagingNeeded, 
	AddedAt, 
	AddedBy
)
SELECT
	D.DF_SPE_ACC_ID AS AccountNumber,
	'Ecorr@UHEAA.org' AS EmailAddress, --Default Services does not do Ecorr.
	D.ScriptDataId AS ScriptDataId,
	NULL AS SourceFile,
	D.BF_SSN + ','
		+ D.DF_SPE_ACC_ID + ',"'
		+ RTRIM(D.DM_PRS_1) + '","'
		+ RTRIM(D.DM_PRS_LST) + '","'
		+ RTRIM(D.DX_STR_ADR_1) + '","'
		+ RTRIM(D.DX_STR_ADR_2) + '","'
		+ RTRIM(D.DM_CT) + '",'
		+ RTRIM(D.DC_DOM_ST) + ','
		+ RTRIM(D.DF_ZIP) + ',"'
		+ RTRIM(D.DM_FGN_CNY) + '",'
		+ CAST(D.DAYS_DLQ AS VARCHAR(4)) + ','
		+ CentralData.dbo.CreateACSKeyline(D.BF_SSN,'B','L') + ','
		+ RTRIM(D.STATE_IND) + ','
		+ D.CCC 
	AS LetterData,
	D.CCC AS CostCenter,
	IIF(PD03.DI_VLD_ADR = 'Y', 0, 1) AS InValidAddress,
	SD.DoNotProcessEcorr,
	0 AS OnEcorr,
	IIF(ASDM.ArcId IS NOT NULL, 1, 0) AS ArcNeeded,
	IIF(SD.DocIdId IS NOT NULL, 1, 0) AS ImagingNeeded,
	GETDATE() AS AddedAt, 
	'UTLWM31' AS AddedBy
FROM
	#DEMOS D
	INNER JOIN ULS.[print].ScriptData SD
		ON SD.ScriptDataId = D.ScriptDataId 
		AND SD.LetterId = D.LetterId
		AND SD.Active = 1 --active flag
	INNER JOIN ULS.[print].ArcScriptDataMapping ASDM
		ON ASDM.ScriptDataId = D.ScriptDataId
	INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
		ON PD03.DC_DOM_ST = D.DC_DOM_ST
		AND PD03.DF_PRS_ID = D.DF_PRS_ID
		AND PD03.DC_ADR = 'L' --legal
	LEFT JOIN ULS.[print].PrintProcessing PP
		ON PP.AccountNumber = D.DF_SPE_ACC_ID
		AND PP.ScriptDataId = D.ScriptDataId
		AND PP.DeletedAt IS NULL
		AND 
		(
			CONVERT(DATE,PP.AddedAt) = CONVERT(DATE,GETDATE())
			OR
			(
				PP.EcorrDocumentCreatedAt IS NULL
				AND PP.PrintedAt IS NULL
			)
		)
WHERE
	Rfile != ''
	AND PP.AccountNumber IS NULL;

--Endorser
INSERT INTO ULS.[print].PrintProcessing
(
	AccountNumber,
	EmailAddress, 
	ScriptDataId, 
	SourceFile, 
	LetterData, 
	CostCenter, 
	InValidAddress, 
	DoNotProcessEcorr, 
	OnEcorr, 
	ArcNeeded, 
	ImagingNeeded, 
	AddedAt, 
	AddedBy
)
SELECT
	D.DF_SPE_ACC_ID AS AccountNumber,
	'Ecorr@UHEAA.org' AS EmailAddress, --Default Services does not do Ecorr.
	D.ScriptDataId AS ScriptDataId,
	NULL AS SourceFile,
	D.BF_SSN + ','
		+ D.DF_SPE_ACC_ID + ',"'
		+ RTRIM(D.DM_PRS_1) + '","'
		+ RTRIM(D.DM_PRS_LST) + '","'
		+ RTRIM(D.DX_STR_ADR_1) + '","'
		+ RTRIM(D.DX_STR_ADR_2) + '","'
		+ RTRIM(D.DM_CT) + '",'
		+ RTRIM(D.DC_DOM_ST) + ','
		+ RTRIM(D.DF_ZIP) + ',"'
		+ RTRIM(D.DM_FGN_CNY) + '",'
		+ CAST(D.DAYS_DLQ AS VARCHAR(4)) + ','
		+ CentralData.dbo.CreateACSKeyline(D.BF_SSN,'B','L') + ','
		+ RTRIM(D.STATE_IND) + ','
		+ D.CCC + ','
		+ D.CoBorrowerAccount 
	AS LetterData,
	D.CCC AS CostCenter,
	IIF(PD03.DI_VLD_ADR = 'Y', 0, 1) AS InValidAddress,
	SD.DoNotProcessEcorr,
	0 AS OnEcorr,
	IIF(ASDM.ArcId IS NOT NULL, 1, 0) AS ArcNeeded,
	IIF(SD.DocIdId IS NOT NULL, 1, 0) AS ImagingNeeded,
	GETDATE() AS AddedAt, 
	'UTLWM31' AS AddedBy
FROM
	#DEMOSEndorser D
	INNER JOIN ULS.[print].ScriptData SD
		ON SD.ScriptDataId = D.ScriptDataId 
		AND SD.LetterId = D.LetterId
		AND SD.Active = 1 --active flag
	INNER JOIN ULS.[print].ArcScriptDataMapping ASDM
		ON ASDM.ScriptDataId = D.ScriptDataId
	INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
		ON PD03.DC_DOM_ST = D.DC_DOM_ST
		AND PD03.DF_PRS_ID = D.DF_PRS_ID
		AND PD03.DC_ADR = 'L' --legal
	LEFT JOIN ULS.[print].PrintProcessing PP
		ON PP.AccountNumber = D.DF_SPE_ACC_ID
		AND PP.ScriptDataId = D.ScriptDataId
		AND PP.DeletedAt IS NULL
		AND 
		(
			CONVERT(DATE,PP.AddedAt) = CONVERT(DATE,GETDATE())
			OR
			(
				PP.EcorrDocumentCreatedAt IS NULL
				AND PP.PrintedAt IS NULL
			)
		)
WHERE
	Rfile != ''
	AND PP.AccountNumber IS NULL;
