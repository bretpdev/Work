DELETE FROM ULS.retrocap.PrintProcessingDataReCap
GO

DELETE FROM ULS.retrocap.PrintProcessingDataCap
GO

DECLARE @NumberOfRows INT = 28

--DECLARE @AccountNumber VARCHAR(10) = '' --**FOR TESTING ONLY, you will need to uncomment other instances of @AccountNumber

DECLARE @Today DATE = CAST(GETDATE() AS DATE)
DECLARE @LastWeek DATE = DATEADD(DAY, -7, @Today)

DECLARE @ScriptId VARCHAR(10) = 'RETROCAP'
DECLARE @CostCenter VARCHAR(10) = 'MA2324'

DECLARE @SystemLetter VARCHAR(10) = 'US06BCAP'
DECLARE @SystemLetterArc VARCHAR(5) = 'F100Q'

DECLARE @CapLetter VARCHAR(10) = 'INTCAPCAS'
DECLARE @CapArc VARCHAR(5) = 'RTCAP'

DECLARE @ReCapLetter VARCHAR(10) = 'INTCAPARS'
DECLARE @ReCapArc VARCHAR(5) = 'RRCAP'

DECLARE @CapScriptDataId INT 
DECLARE @CapDoNotProcessEcorr INT
DECLARE @CapImagingNeeded BIT
DECLARE @CapArcNeeded BIT

--Setting variables to be used to create the letters
SELECT 
	@CapScriptDataId = SD.ScriptDataId,
	@CapDoNotProcessEcorr = SD.DoNotProcessEcorr,
	@CapImagingNeeded = CASE WHEN SD.DocIdId IS NOT NULL THEN 1 ELSE 0 END,
	@CapArcNeeded = CASE WHEN ASD.ScriptDataId IS NOT NULL THEN 1 ELSE 0 END
FROM 
	ULS.[print].ScriptData SD 
	INNER JOIN ULS.[print].Letters L 
		ON SD.LetterId = L.LetterId 
	LEFT JOIN ULS.[print].ArcScriptDataMapping ASD 
		ON ASD.ScriptDataId = SD.ScriptDataId 
WHERE 
	L.Letter = @CapLetter 
	AND SD.ScriptId = @ScriptId

DECLARE @ReCapScriptDataId INT 
DECLARE @ReCapDoNotProcessEcorr INT
DECLARE @ReCapImagingNeeded BIT
DECLARE @ReCapArcNeeded BIT

--Setting variables to be used to create the letters
SELECT 
	@ReCapScriptDataId = SD.ScriptDataId,
	@ReCapDoNotProcessEcorr = SD.DoNotProcessEcorr,
	@ReCapImagingNeeded = CASE WHEN SD.DocIdId IS NOT NULL THEN 1 ELSE 0 END,
	@ReCapArcNeeded = CASE WHEN ASD.ScriptDataId IS NOT NULL THEN 1 ELSE 0 END
FROM 
	ULS.[print].ScriptData SD 
	INNER JOIN ULS.[print].Letters L 
		ON SD.LetterId = L.LetterId 
	LEFT JOIN ULS.[print].ArcScriptDataMapping ASD 
		ON ASD.ScriptDataId = SD.ScriptDataId 
WHERE 
	L.Letter = @ReCapLetter 
	AND SD.ScriptId = @ScriptId

--New Capitalization Letter LETTER-TBD1, ARC-TBD1, Sends when an interest capitalization 
--happens without a notice of interest capitalization in the last week.
--When there are multiple matched reversals, this will still generate record.
INSERT INTO ULS.retrocap.PrintProcessingDataCap(BF_SSN,LN_SEQ,LN_FAT_SEQ,LA_FAT_CUR_PRI,LD_FAT_EFF,TotalCapitalized,[Population])
SELECT
	LN90.BF_SSN,
	LN90.LN_SEQ,
	LN90.LN_FAT_SEQ,
	ABS(LN90.LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI,
	LN90.LD_FAT_EFF,
	LN90_TOTALS.TotalCapitalized,
	1 --Population number just used to tell what query inserted it into the processing table
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN90_FIN_ATY LN90
		ON PD10.DF_PRS_ID = LN90.BF_SSN
	INNER JOIN UDW..LN10_LON LN10
		ON LN90.BF_SSN = LN10.BF_SSN
		AND LN90.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			LN90.BF_SSN,
			LN90.LN_SEQ,
			SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL (18,2)))) AS TotalCapitalized
		FROM
			UDW..LN90_FIN_ATY LN90
		WHERE
			LN90.PC_FAT_TYP = '70'
			AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
			AND LN90.LC_STA_LON90 = 'A'
		GROUP BY
			LN90.BF_SSN,
			LN90.LN_SEQ
	) LN90_TOTALS
		ON LN90_TOTALS.BF_SSN = LN90.BF_SSN
		AND LN90_TOTALS.LN_SEQ = LN90.LN_SEQ
	--We want to find an matched reversals if one exists
	--so we can exclude LN90 records that are included in a matching reversal
	LEFT JOIN
	(
		SELECT
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE) AS LD_STA_LON90,
			LN90.LN_FAT_SEQ
		FROM
			UDW..LN90_FIN_ATY LN90_REV
			--Join on non-reversed transactions to make sure they are matched
			LEFT JOIN UDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = LN90_REV.BF_SSN
				AND LN90.LN_SEQ = LN90_REV.LN_SEQ
				AND LN90.LD_STA_LON90 = LN90_REV.LD_STA_LON90
				AND LN90.PC_FAT_TYP = '70'
				AND LN90.PC_FAT_SUB_TYP = '01'
				AND LN90.LC_STA_LON90 = 'A'
				AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
				AND CAST(LN90.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
				AND LN90.LD_FAT_EFF = LN90_REV.LD_FAT_EFF
		WHERE
			LN90_REV.PC_FAT_TYP = '70'
			AND LN90_REV.PC_FAT_SUB_TYP = '01'
			AND ISNULL(LN90_REV.LC_FAT_REV_REA, '') != ''
			--We don't want to send a letter if there is an active reversal 
			AND LN90_REV.LC_STA_LON90 = 'A'
			AND LN90_REV.LD_STA_LON90 BETWEEN @LastWeek AND @Today
			--Only return results when there is a matched reversal
			AND LN90.BF_SSN IS NOT NULL
		GROUP BY
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE),
			LN90.LN_FAT_SEQ
	) LN90_REV
		ON LN90.BF_SSN = LN90_REV.BF_SSN
		AND LN90.LN_SEQ = LN90_REV.LN_SEQ
		AND CAST(LN90.LD_STA_LON90 AS DATE) = LN90_REV.LD_STA_LON90
		AND LN90.LN_FAT_SEQ = LN90_REV.LN_FAT_SEQ
	--We get all unmatched reversals
	LEFT JOIN
	(
		SELECT
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE) AS LD_STA_LON90
		FROM
			UDW..LN90_FIN_ATY LN90_REV
			--Join on non-reversed transactions to make sure they are matched
			LEFT JOIN UDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = LN90_REV.BF_SSN
				AND LN90.LN_SEQ = LN90_REV.LN_SEQ
				AND LN90.LD_STA_LON90 = LN90_REV.LD_STA_LON90
				AND LN90.PC_FAT_TYP = '70'
				AND LN90.PC_FAT_SUB_TYP = '01'
				AND LN90.LC_STA_LON90 = 'A'
				AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
				AND CAST(LN90.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
				AND LN90.LD_FAT_EFF = LN90_REV.LD_FAT_EFF
		WHERE
			LN90_REV.PC_FAT_TYP = '70'
			AND LN90_REV.PC_FAT_SUB_TYP = '01'
			AND ISNULL(LN90_REV.LC_FAT_REV_REA, '') != ''
			--We don't want to send a letter if there is an active reversal 
			AND LN90_REV.LC_STA_LON90 = 'A'
			AND CAST(LN90_REV.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
			--Only return results when there is a matched reversal
			AND LN90.BF_SSN IS NULL
		GROUP BY
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE)
	) LN90_REV_UNMATCHED
		ON LN90.BF_SSN = LN90_REV_UNMATCHED.BF_SSN
		AND LN90.LN_SEQ = LN90_REV_UNMATCHED.LN_SEQ
		AND CAST(LN90.LD_STA_LON90 AS DATE) = LN90_REV_UNMATCHED.LD_STA_LON90
		--We only want to join this onto records without a matching reversal
		AND LN90_REV.BF_SSN IS NULL
	LEFT JOIN
	(
		SELECT	
			LT20.DF_SPE_ACC_ID,
			MAX(CAST(LT20.CreatedAt AS DATE)) AS MAX_CRT
		FROM
			UDW..LT20_LTR_REQ_PRC LT20
		WHERE
			LT20.RM_DSC_LTR_PRC = @SystemLetter
			AND CAST(LT20.CreatedAt AS DATE) BETWEEN @LastWeek AND @Today
		GROUP BY
			LT20.DF_SPE_ACC_ID
	) LT20
		ON PD10.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID 
	LEFT JOIN
	(
		SELECT
			AY10.BF_SSN,
			MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
			MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM 
			UDW..AY10_BR_LON_ATY AY10
		WHERE
			(
				AY10.PF_REQ_ACT = @SystemLetterArc
				OR AY10.PF_REQ_ACT = @CapArc
			)
			AND AY10.LC_STA_ACTY10 = 'A'
			AND LD_ATY_REQ_RCV BETWEEN @LastWeek AND @Today
		GROUP BY
			AY10.BF_SSN
	) AY10
		ON AY10.BF_SSN = PD10.DF_PRS_ID 
WHERE
	LN90.PC_FAT_TYP = '70'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 IN ('R', 'L')
	AND LN90.PC_FAT_SUB_TYP = '01'
	AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
	AND LN90.LC_STA_LON90 = 'A'
	AND CAST(LN90.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
	--Exclusions
	AND LN90_REV.BF_SSN IS NULL
	AND LN90_REV_UNMATCHED.BF_SSN IS NULL
	AND LT20.DF_SPE_ACC_ID IS NULL
	AND AY10.BF_SSN IS NULL
	--AND PD10.DF_SPE_ACC_ID = @AccountNumber --**FOR TESTING ONLY
ORDER BY
	BF_SSN,
	LN_SEQ,
	LN_FAT_SEQ

--New Re-Capitalization Letter LETTER-TBD2, ARC-TBD2, Sends when an interest capitalization
--is reversed and reapplied within a week.
--Population 1 of Re-Capitalization, when the borrower has a reversal matched on effective date with a different balance
INSERT INTO ULS.retrocap.PrintProcessingDataReCap(BF_SSN,LN_SEQ,LN_FAT_SEQ, LD_FAT_EFF, REV_LD_FAT_EFF, LA_FAT_CUR_PRI,REV_LA_FAT_CUR_PRI,[Population])
SELECT
	LN90.BF_SSN,
	LN90.LN_SEQ,
	LN90.LN_FAT_SEQ,
	LN90.LD_FAT_EFF,
	LN90_REV.LD_FAT_EFF AS REV_LD_FAT_EFF,
	ABS(LN90.LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI,
	ABS(LN90_REV.LA_FAT_CUR_PRI) AS REV_LA_FAT_CUR_PRI,
	2 --Population number just used to tell what query inserted it into the processing table
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN90_FIN_ATY LN90
		ON PD10.DF_PRS_ID = LN90.BF_SSN
	INNER JOIN UDW..LN10_LON LN10
		ON LN90.BF_SSN = LN10.BF_SSN
		AND LN90.LN_SEQ = LN10.LN_SEQ
	--Get the corresponding reversals that happened on the same status date as
	--the max date reversal that fits the criteria
	INNER JOIN 
	(
		SELECT	
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			LN90_REV.LD_FAT_EFF,
			CAST(LN90_REV.LD_STA_LON90 AS DATE) AS LD_STA_LON90,
			MIN(LN90_REV.LN_FAT_SEQ) AS LN_FAT_SEQ
		FROM
			UDW..LN90_FIN_ATY LN90_REV
		WHERE
			LN90_REV.PC_FAT_TYP = '70'
			AND LN90_REV.PC_FAT_SUB_TYP = '01'
			AND LN90_REV.LC_STA_LON90 = 'A'
			AND ISNULL(LN90_REV.LC_FAT_REV_REA, '') != ''
			AND CAST(LN90_REV.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
		GROUP BY
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			LN90_REV.LD_FAT_EFF,
			CAST(LN90_REV.LD_STA_LON90 AS DATE)
	) LN90_REV_MIN
		ON LN90.BF_SSN = LN90_REV_MIN.BF_SSN
		AND LN90.LN_SEQ = LN90_REV_MIN.LN_SEQ
		AND CAST(LN90.LD_STA_LON90 AS DATE) = LN90_REV_MIN.LD_STA_LON90
		AND LN90_REV_MIN.LD_FAT_EFF = LN90.LD_FAT_EFF
	--This is inner joined onto LN90_REV_MIN on the primary key, it should not need to check active flags
	INNER JOIN UDW..LN90_FIN_ATY LN90_REV
		ON LN90_REV.BF_SSN = LN90_REV_MIN.BF_SSN
		AND LN90_REV.LN_SEQ = LN90_REV_MIN.LN_SEQ
		AND LN90_REV.LD_STA_LON90 = LN90_REV_MIN.LD_STA_LON90
		AND LN90_REV.LD_FAT_EFF = LN90_REV_MIN.LD_FAT_EFF
		AND LN90_REV.LN_FAT_SEQ = LN90_REV_MIN.LN_FAT_SEQ
		AND ABS(LN90_REV.LA_FAT_CUR_PRI) != ABS(LN90.LA_FAT_CUR_PRI)
	LEFT JOIN
	(
		SELECT	
			LT20.DF_SPE_ACC_ID,
			MAX(CAST(LT20.CreatedAt AS DATE)) AS MAX_CRT
		FROM
			UDW..LT20_LTR_REQ_PRC LT20
		WHERE
			LT20.RM_DSC_LTR_PRC = @SystemLetter
			AND CAST(LT20.CreatedAt AS DATE) BETWEEN @LastWeek AND @Today
		GROUP BY
			LT20.DF_SPE_ACC_ID
	) LT20
		ON PD10.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID 
	LEFT JOIN
	(
		SELECT
			AY10.BF_SSN,
			MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
			MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM 
			UDW..AY10_BR_LON_ATY AY10
		WHERE
			(
				AY10.PF_REQ_ACT = @SystemLetterArc
				OR AY10.PF_REQ_ACT = @ReCapArc
			)
			AND AY10.LC_STA_ACTY10 = 'A'
			AND LD_ATY_REQ_RCV BETWEEN @LastWeek AND @Today
		GROUP BY
			AY10.BF_SSN
	) AY10
		ON AY10.BF_SSN = PD10.DF_PRS_ID 
WHERE
	LN90.PC_FAT_TYP = '70'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 IN ('R', 'L')
	AND LN90.PC_FAT_SUB_TYP = '01'
	AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
	AND LN90.LC_STA_LON90 = 'A'
	AND CAST(LN90.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
	--Exclusions
	AND LT20.DF_SPE_ACC_ID IS NULL
	AND AY10.BF_SSN IS NULL
	--AND PD10.DF_SPE_ACC_ID = @AccountNumber --**FOR TESTING ONLY
ORDER BY
	BF_SSN,
	LN_SEQ,
	LN_FAT_SEQ
	--REV_LN_FAT_SEQ

--New Re-Capitalization Letter LETTER-TBD2, ARC-TBD2, Sends when an interest capitalization
--is reversed and reapplied within a week.
--Population 2 of Re-Capitalization, when the borrower has an unmatched reversal and a new capitalization with a different effective date on the same status date
INSERT INTO ULS.retrocap.PrintProcessingDataReCap(BF_SSN,LN_SEQ,LN_FAT_SEQ, LD_FAT_EFF, REV_LD_FAT_EFF, LA_FAT_CUR_PRI,REV_LA_FAT_CUR_PRI,[Population])
SELECT
	LN90.BF_SSN,
	LN90.LN_SEQ,
	LN90.LN_FAT_SEQ,
	LN90.LD_FAT_EFF AS LD_FAT_EFF,
	LN90_REV_UNMATCHED.LD_FAT_EFF AS REV_LD_FAT_EFF,
	ABS(LN90.LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI,
	ABS(LN90_REV_UNMATCHED.LA_FAT_CUR_PRI) AS REV_LA_FAT_CUR_PRI,
	3 --Population number just used to tell what query inserted it into the processing table
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN90_FIN_ATY LN90
		ON PD10.DF_PRS_ID = LN90.BF_SSN
	INNER JOIN UDW..LN10_LON LN10
		ON LN90.BF_SSN = LN10.BF_SSN
		AND LN90.LN_SEQ = LN10.LN_SEQ
	--We want to find an matched reversals if one exists
	--so we can exclude LN90 records that are included in a matching reversal
	LEFT JOIN
	(
		SELECT
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE) AS LD_STA_LON90,
			LN90.LN_FAT_SEQ
		FROM
			UDW..LN90_FIN_ATY LN90_REV
			--Join on non-reversed transactions to make sure they are matched
			LEFT JOIN UDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = LN90_REV.BF_SSN
				AND LN90.LN_SEQ = LN90_REV.LN_SEQ
				AND LN90.LD_STA_LON90 = LN90_REV.LD_STA_LON90
				AND LN90.PC_FAT_TYP = '70'
				AND LN90.PC_FAT_SUB_TYP = '01'
				AND LN90.LC_STA_LON90 = 'A'
				AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
				AND CAST(LN90.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
				AND LN90.LD_FAT_EFF = LN90_REV.LD_FAT_EFF
		WHERE
			LN90_REV.PC_FAT_TYP = '70'
			AND LN90_REV.PC_FAT_SUB_TYP = '01'
			AND ISNULL(LN90_REV.LC_FAT_REV_REA, '') != ''
			--We don't want to send a letter if there is an active reversal 
			AND LN90_REV.LC_STA_LON90 = 'A'
			AND CAST(LN90_REV.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
			--Only return results when there is a matched reversal
			AND LN90.BF_SSN IS NOT NULL
		GROUP BY
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE),
			LN90.LN_FAT_SEQ
	) LN90_REV
		ON LN90.BF_SSN = LN90_REV.BF_SSN
		AND LN90.LN_SEQ = LN90_REV.LN_SEQ
		AND CAST(LN90.LD_STA_LON90 AS DATE) = LN90_REV.LD_STA_LON90
		AND LN90.LN_FAT_SEQ = LN90_REV.LN_FAT_SEQ
	--We get all unmatched reversals
	LEFT JOIN
	(
		SELECT
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE) AS LD_STA_LON90,
			LN90_REV.LA_FAT_CUR_PRI,
			LN90_REV.LD_FAT_EFF
		FROM
			UDW..LN90_FIN_ATY LN90_REV
			--Join on non-reversed transactions to make sure they are matched
			LEFT JOIN UDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = LN90_REV.BF_SSN
				AND LN90.LN_SEQ = LN90_REV.LN_SEQ
				AND LN90.LD_STA_LON90 = LN90_REV.LD_STA_LON90
				AND LN90.PC_FAT_TYP = '70'
				AND LN90.PC_FAT_SUB_TYP = '01'
				AND LN90.LC_STA_LON90 = 'A'
				AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
				AND CAST(LN90.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
				AND LN90.LD_FAT_EFF = LN90_REV.LD_FAT_EFF
		WHERE
			LN90_REV.PC_FAT_TYP = '70'
			AND LN90_REV.PC_FAT_SUB_TYP = '01'
			AND ISNULL(LN90_REV.LC_FAT_REV_REA, '') != ''
			--We don't want to send a letter if there is an active reversal 
			AND LN90_REV.LC_STA_LON90 = 'A'
			AND CAST(LN90_REV.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
			--Only return results when there is a matched reversal
			AND LN90.BF_SSN IS NULL
		GROUP BY
			LN90_REV.BF_SSN,
			LN90_REV.LN_SEQ,
			CAST(LN90_REV.LD_STA_LON90 AS DATE),
			LN90_REV.LA_FAT_CUR_PRI,
			LN90_REV.LD_FAT_EFF
	) LN90_REV_UNMATCHED
		ON LN90.BF_SSN = LN90_REV_UNMATCHED.BF_SSN
		AND LN90.LN_SEQ = LN90_REV_UNMATCHED.LN_SEQ
		AND LN90.LD_STA_LON90 = LN90_REV_UNMATCHED.LD_STA_LON90
		--We only want to join this onto records without a matching reversal
		AND LN90_REV.BF_SSN IS NULL
	LEFT JOIN
	(
		SELECT	
			LT20.DF_SPE_ACC_ID,
			MAX(CAST(LT20.CreatedAt AS DATE)) AS MAX_CRT
		FROM
			UDW..LT20_LTR_REQ_PRC LT20
		WHERE
			LT20.RM_DSC_LTR_PRC = @SystemLetter
			AND CAST(LT20.CreatedAt AS DATE) BETWEEN @LastWeek AND @Today
		GROUP BY
			LT20.DF_SPE_ACC_ID
	) LT20
		ON PD10.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID 
	LEFT JOIN
	(
		SELECT
			AY10.BF_SSN,
			MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
			MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM 
			UDW..AY10_BR_LON_ATY AY10
		WHERE
			(
				AY10.PF_REQ_ACT = @SystemLetterArc
				OR AY10.PF_REQ_ACT = @ReCapArc
			)
			AND AY10.LC_STA_ACTY10 = 'A'
			AND LD_ATY_REQ_RCV BETWEEN @LastWeek AND @Today
		GROUP BY
			AY10.BF_SSN
	) AY10
		ON AY10.BF_SSN = PD10.DF_PRS_ID 
WHERE
	LN90.PC_FAT_TYP = '70'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 IN ('R', 'L')
	AND LN90.PC_FAT_SUB_TYP = '01'
	AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
	AND LN90.LC_STA_LON90 = 'A'
	AND CAST(LN90.LD_STA_LON90 AS DATE) BETWEEN @LastWeek AND @Today
	--There is an unmatched reversal
	AND LN90_REV_UNMATCHED.BF_SSN IS NOT NULL
	--Exclusions
	--There is no matching reversal
	AND LN90_REV.BF_SSN IS NULL
	AND LT20.DF_SPE_ACC_ID IS NULL
	AND AY10.BF_SSN IS NULL
	--AND PD10.DF_SPE_ACC_ID = @AccountNumber --**FOR TESTING ONLY
ORDER BY
	BF_SSN,
	LN_SEQ,
	LN_FAT_SEQ

;WITH LoanDetail(NumberOfRow) AS 
(
	SELECT 1
	UNION ALL
	SELECT NumberOfRow + 1
	FROM LoanDetail
	WHERE NumberOfRow < @NumberOfRows
)
--Create ReCap Letters
INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT
	LETTER_INFO.AccountNumber,
	LETTER_INFO.EmailAddress,
	LETTER_INFO.ScriptDataId,
	LETTER_INFO.LetterData,
	LETTER_INFO.CostCenter,
	LETTER_INFO.DoNotProcessEcorr,
	LETTER_INFO.OnEcorr,
	LETTER_INFO.ArcNeeded,
	LETTER_INFO.ImagingNeeded,
	LETTER_INFO.AddedBy,
	LETTER_INFO.AddedAt
FROM
(
	SELECT DISTINCT	
		PPD.BF_SSN AS Ssn,
		ADDR_INFO.AccountNumber AS AccountNumber,
		CASE WHEN @ReCapDoNotProcessEcorr = 1 THEN 'Ecorr@uheaa.org' ELSE COALESCE(EMAIL_INFO.DX_CNC_EML_ADR, 'Ecorr@uheaa.org') END AS EmailAddress,
		@ReCapScriptDataId AS ScriptDataId,
		LetterData = CentralData.dbo.CreateACSKeyLine(ADDR_INFO.AccountNumber, 'B', 'L') + ',' + ADDR_INFO.[Name] + ',' + ADDR_INFO.Address1 + ',' + ADDR_INFO.Address2 + ',' + ADDR_INFO.City +
		',' + ADDR_INFO.[State] + ',' + ADDR_INFO.Zip + ',' + ADDR_INFO.Country + ',' + ADDR_INFO.AccountNumber + 	
		STUFF
		(
			(
				SELECT	
					CASE WHEN x.BF_SSN IS NOT NULL THEN
						',' + COALESCE(FT.Label, LN10.IC_LON_PGM) --IC_LON_PGM
						+ ',' + CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 101) --LD_LON_1_DSB
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN15.LA_DSB, 0.00), 1) --OPAFEL Previously LN10.LA_LON_AMT_GTR
						+ ',' + CONVERT(VARCHAR(10), LN72.LR_ITR,0) + '%' --LR_INT_BIL
						+ ',' + '$' + CONVERT(VARCHAR(15), LN10.LA_CUR_PRI, 1) --LA_CUR_PRN_BIL
						--+ ',' + CASE WHEN DW01.BF_SSN IS NOT NULL THEN CAST(0 AS VARCHAR(15)) ELSE CAST(ISNULL(LN16.LN_DLQ_MAX + 1, CASE WHEN COALESCE(LN80.LA_BIL_PAS_DU,0.00) > 0 THEN 1 ELSE 0 END) AS VARCHAR(15)) END --DAYS_DELQ
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN80.LA_BIL_PAS_DU,0.00), 1) --LA_BIL_PAS_DU
						--+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN10.LA_LTE_FEE_OTS, 0.00), 1) --LN_LTE_FEE
						+ ',' + CONVERT(VARCHAR(10), x.REV_LD_FAT_EFF, 101) --Reversed effective date
						+ ',' + CONVERT(VARCHAR(10), x.LD_FAT_EFF, 101) --Reapplied effective date
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.REV_LA_FAT_CUR_PRI, 0.00)) --Reversed_Amount_Capitalized
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.LA_FAT_CUR_PRI, 0.00)) --Amount_Capitalized
					ELSE
						',,,,,,,,,'
					END
				FROM
					LoanDetail LD
					LEFT JOIN
					(	
						SELECT DISTINCT	
							BF_SSN,
							LN_SEQ,
							LDPPD.LD_FAT_EFF,
							LDPPD.REV_LD_FAT_EFF,
							LDPPD.LA_FAT_CUR_PRI,
							LDPPD.REV_LA_FAT_CUR_PRI,
							ROW_NUMBER() OVER (ORDER BY BF_SSN, LN_SEQ, LD_FAT_EFF, REV_LD_FAT_EFF, LA_FAT_CUR_PRI, REV_LA_FAT_CUR_PRI) AS RowNumber
						FROM
							ULS.retrocap.PrintProcessingDataReCap LDPPD
						WHERE
							PPD.BF_SSN = LDPPD.BF_SSN
					) x
						ON x.RowNumber = LD.NumberOfRow	
						AND x.BF_SSN = PPD.BF_SSN
					LEFT JOIN UDW..LN10_LON LN10
						ON x.BF_SSN = LN10.BF_SSN
						AND x.LN_SEQ = LN10.LN_SEQ
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 IN ('R', 'L')
					LEFT JOIN
					(	--Adding Active Interest Rate
						SELECT
							LN72.BF_SSN, 
							LN72.LN_SEQ,
							LN72.LR_ITR,
							ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
						FROM
							UDW..LN72_INT_RTE_HST LN72
						WHERE
							LN72.LC_STA_LON72 = 'A'
							AND	@Today BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							AND	LN72.BF_SSN = PPD.BF_SSN
					) LN72 
						ON LN10.BF_SSN = LN72.BF_SSN
						AND LN10.LN_SEQ = LN72.LN_SEQ
						AND LN72.SEQ = 1
					LEFT JOIN /*GETS THE CURRENT BILL INFORMATION*/
					(
						SELECT
							LN80.BF_SSN,
							LN80.LN_SEQ,
							SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS LA_BIL_PAS_DU,
							SUM(isnull(LN80.LA_BIL_CUR_DU,0)) AS LA_BIL_CUR_DU,
							SUM(ISNULL(LN80.LA_TOT_BIL_STS, 0)) AS LA_TOT_BIL_STS
						FROM 
							UDW..LN80_LON_BIL_CRF LN80
							INNER JOIN UDW..LN10_LON LN10
								ON LN10.BF_SSN = LN80.BF_SSN
								AND LN10.LN_SEQ = LN80.LN_SEQ
						WHERE 
							LN80.LC_STA_LON80 = 'A'
							AND LN80.LC_LON_STA_BIL = '1'
							AND CAST(LN80.LD_BIL_DU_LON AS DATE) < CAST(getdate() AS DATE)
							AND LN10.LC_STA_LON10 IN ('R','L')
							AND LN10.LA_CUR_PRI > 0
						GROUP BY 
							LN80.BF_SSN,
							LN80.LN_SEQ
					) LN80
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ
					LEFT JOIN
					(
						SELECT
							LN90.BF_SSN,
							LN90.LN_SEQ,
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL (18,2)))) AS TAPTP,
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL (18,2)))) AS TAPTI,
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0)AS DECIMAL (18,2)))) AS TAPTF, /*cumulative late fees paid*/
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0) AS DECIMAL(18,2)))) AS TAGAP
						FROM 
							UDW..LN90_FIN_ATY LN90
						WHERE
							LN90.PC_FAT_TYP = '10'
							AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
							AND LN90.LC_STA_LON90 = 'A'
						GROUP BY
							LN90.BF_SSN,
							LN90.LN_SEQ
					)LN90
						ON LN90.BF_SSN = LN10.BF_SSN
						AND LN90.LN_SEQ = LN10.LN_SEQ
					LEFT JOIN
					(
						SELECT
							LN15.BF_SSN,
							LN15.LN_SEQ,
							SUM(LN15.LA_DSB - COALESCE(LA_DSB_CAN, 0.00)) AS LA_DSB
						FROM
							UDW..LN15_DSB LN15
						WHERE	
							COALESCE(LA_DSB_CAN, 0.00) < LN15.LA_DSB
							AND LC_STA_LON15 IN (1, 3)
						GROUP BY
							LN15.BF_SSN,
							LN15.LN_SEQ		
					) LN15
						ON LN15.BF_SSN = LN10.BF_SSN
						AND LN15.LN_SEQ = LN10.LN_SEQ
					LEFT JOIN UDW..LN16_LON_DLQ_HST LN16
						ON LN16.BF_SSN = LN10.BF_SSN
						AND LN16.LN_SEQ = LN10.LN_SEQ
						AND LN16.LC_STA_LON16 = '1'
					LEFT JOIN UDW..DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN
						AND DW01.LN_SEQ = LN10.LN_SEQ
						AND DW01.WC_DW_LON_STA IN ('18', '19')
						AND DW01.WX_OVR_DW_LON_STA != ''
					LEFT JOIN UDW..FormatTranslation FT
						ON FT.[Start] = LN10.IC_LON_PGM
						AND FT.FmtName = '$LNPROG'
				ORDER BY
					LD.NumberOfRow
				FOR XML PATH(''), TYPE
			).value('.','VARCHAR(MAX)'),
			1,0,''
		),
		@CostCenter AS CostCenter,
		@ReCapDoNotProcessEcorr AS DoNotProcessEcorr,
		CASE WHEN @ReCapDoNotProcessEcorr = 1 
			THEN 0 
			ELSE 
				CASE WHEN EMAIL_INFO.DI_VLD_CNC_EML_ADR = 'Y' AND EMAIL_INFO.DI_CNC_ELT_OPI = 'Y' 
					THEN 1 
					ELSE 0 
				END 
		END AS OnEcorr, --OnEcorr
		@ReCapArcNeeded AS ArcNeeded,
		@ReCapImagingNeeded AS ImagingNeeded,
		GETDATE() AS AddedAt,
		SUSER_NAME() AS AddedBy
	FROM
	(
		SELECT DISTINCT
			PPD.BF_SSN
		FROM
			ULS.retrocap.PrintProcessingDataReCap PPD
		GROUP BY
			PPD.BF_SSN
	) PPD
	INNER JOIN 
	(
		SELECT
			BORR.DF_PRS_ID,
			--borrower info
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','')  AS Name,
			--address info
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_1)),',','')  AS Address1,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_2)),',','')  AS Address2,
			REPLACE(LTRIM(RTRIM(ADDR.DM_FGN_CNY)),',','') AS Country,
			BORR.DF_SPE_ACC_ID AS AccountNumber,
			REPLACE(LTRIM(RTRIM(ADDR.DM_CT)),',','')  AS City,
			CASE
				WHEN LEN(RTRIM(ADDR.DM_FGN_ST)) > 0 THEN  RTRIM(ADDR.DM_FGN_ST)
				ELSE RTRIM(ADDR.DC_DOM_ST)
			END AS [State],
			CASE WHEN LEN(ADDR.DF_ZIP_CDE) = 9 
				THEN LEFT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 4)
				ELSE LTRIM(RTRIM(ADDR.DF_ZIP_CDE))
			END AS Zip,
			CASE WHEN DI_VLD_ADR = 'Y' 
				THEN 1
				ELSE 0
			END AS HasValidAddress
		FROM
			UDW..PD10_PRS_NME BORR
			INNER JOIN UDW..PD30_PRS_ADR ADDR 
				ON ADDR.DF_PRS_ID = BORR.DF_PRS_ID
		WHERE
			--BORR.DF_PRS_ID = @BF_SSN
			ADDR.DC_ADR = 'L'
	) ADDR_INFO
		ON PPD.BF_SSN = ADDR_INFO.DF_PRS_ID 
	LEFT JOIN UDW..PH05_CNC_EML EMAIL_INFO
		ON EMAIL_INFO.DF_SPE_ID = ADDR_INFO.AccountNumber
) LETTER_INFO
--Check for duplicate letters for the day
LEFT JOIN
(
	SELECT
		PP.AccountNumber,
		SUBSTRING(PP.LetterData, CHARINDEX(',',PP.LetterData,0), LEN(PP.LetterData) - CHARINDEX(',',PP.LetterData,0)) AS LetterData
	FROM
		ULS.[print].PrintProcessing PP
		INNER JOIN ULS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		INNER JOIN ULS.[print].Letters L
			ON L.LetterId = SD.LetterId
	WHERE
		--PP.AccountNumber = @AccountNumber --**FOR TESTING ONLY
		SD.ScriptId = @ScriptId
		AND L.Letter = @ReCapLetter
		AND PP.DeletedAt IS NULL
		AND CAST(PP.AddedAt AS DATE) >= @LastWeek

) PP
	ON PP.AccountNumber = LETTER_INFO.AccountNumber
	AND PP.LetterData = SUBSTRING(LETTER_INFO.LetterData, CHARINDEX(',',LETTER_INFO.LetterData,0), LEN(LETTER_INFO.LetterData) - CHARINDEX(',',LETTER_INFO.LetterData,0))
WHERE
	PP.AccountNumber IS NULL

--Create COBORROWER ReCap Letters
;WITH LoanDetail(NumberOfRow) AS 
(
	SELECT 1
	UNION ALL
	SELECT NumberOfRow + 1
	FROM LoanDetail
	WHERE NumberOfRow < @NumberOfRows
)
INSERT INTO ULS.[print].PrintProcessingCoBorrower(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt, BorrowerSsn)
SELECT
	LETTER_INFO.AccountNumber,
	LETTER_INFO.EmailAddress,
	LETTER_INFO.ScriptDataId,
	LETTER_INFO.LetterData,
	LETTER_INFO.CostCenter,
	LETTER_INFO.DoNotProcessEcorr,
	LETTER_INFO.OnEcorr,
	LETTER_INFO.ArcNeeded,
	LETTER_INFO.ImagingNeeded,
	LETTER_INFO.AddedBy,
	LETTER_INFO.AddedAt,
	LETTER_INFO.Ssn --Borrower Ssn
FROM
(
	SELECT DISTINCT	
		PPD.BF_SSN AS Ssn, --Borrower Ssn
		--PPD.LF_EDS AS Ssn, --Coborrower Ssn
		ADDR_INFO.AccountNumber AS AccountNumber, --Coborrower Account Number
		CASE WHEN @ReCapDoNotProcessEcorr = 1 THEN 'Ecorr@uheaa.org' ELSE COALESCE(EMAIL_INFO.DX_CNC_EML_ADR, 'Ecorr@uheaa.org') END AS EmailAddress,
		@ReCapScriptDataId AS ScriptDataId,
		LetterData = CentralData.dbo.CreateACSKeyLine(PD10_BOR.DF_SPE_ACC_ID, 'B', 'L') + ',' + ADDR_INFO.[Name] + ',' + ADDR_INFO.Address1 + ',' + ADDR_INFO.Address2 + ',' + ADDR_INFO.City +
		',' + ADDR_INFO.[State] + ',' + ADDR_INFO.Zip + ',' + ADDR_INFO.Country + ',' + PD10_BOR.DF_SPE_ACC_ID + 	
		STUFF
		(
			(
				SELECT	
					CASE WHEN x.BF_SSN IS NOT NULL THEN
						',' + COALESCE(FT.Label, LN10.IC_LON_PGM) --IC_LON_PGM
						+ ',' + CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 101) --LD_LON_1_DSB
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN15.LA_DSB, 0.00), 1) --OPAFEL Previously LN10.LA_LON_AMT_GTR
						+ ',' + CONVERT(VARCHAR(10), LN72.LR_ITR,0) + '%' --LR_INT_BIL
						+ ',' + '$' + CONVERT(VARCHAR(15), LN10.LA_CUR_PRI, 1) --LA_CUR_PRN_BIL
						--+ ',' + CASE WHEN DW01.BF_SSN IS NOT NULL THEN CAST(0 AS VARCHAR(15)) ELSE CAST(ISNULL(LN16.LN_DLQ_MAX + 1, CASE WHEN COALESCE(LN80.LA_BIL_PAS_DU,0.00) > 0 THEN 1 ELSE 0 END) AS VARCHAR(15)) END --DAYS_DELQ
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN80.LA_BIL_PAS_DU,0.00), 1) --LA_BIL_PAS_DU
						--+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN10.LA_LTE_FEE_OTS, 0.00), 1) --LN_LTE_FEE
						+ ',' + CONVERT(VARCHAR(10), x.REV_LD_FAT_EFF, 101) --Reversed effective date
						+ ',' + CONVERT(VARCHAR(10), x.LD_FAT_EFF, 101) --Reapplied effective date
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.REV_LA_FAT_CUR_PRI, 0.00)) --Reversed_Amount_Capitalized
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.LA_FAT_CUR_PRI, 0.00)) --Amount_Capitalized
					ELSE
						',,,,,,,,,'
					END
				FROM
					LoanDetail LD
					LEFT JOIN
					(	
						SELECT DISTINCT	
							LDPPD.BF_SSN,
							LN20.LF_EDS,
							LDPPD.LN_SEQ,
							LDPPD.LD_FAT_EFF,
							LDPPD.REV_LD_FAT_EFF,
							LDPPD.LA_FAT_CUR_PRI,
							LDPPD.REV_LA_FAT_CUR_PRI,
							ROW_NUMBER() OVER (PARTITION BY LN20.LF_EDS ORDER BY LDPPD.BF_SSN, LDPPD.LN_SEQ, LDPPD.LD_FAT_EFF, LDPPD.REV_LD_FAT_EFF, LDPPD.LA_FAT_CUR_PRI, LDPPD.REV_LA_FAT_CUR_PRI) AS RowNumber
						FROM
							ULS.retrocap.PrintProcessingDataReCap LDPPD
							INNER JOIN UDW..LN20_EDS LN20
								ON LDPPD.BF_SSN = LN20.BF_SSN
								AND LDPPD.LN_SEQ = LN20.LN_SEQ
						WHERE
							LN20.LC_EDS_TYP = 'M'
							AND LN20.LC_STA_LON20 = 'A'
							AND PPD.BF_SSN = LDPPD.BF_SSN
							AND PPD.LF_EDS = LN20.LF_EDS
					) x
						ON x.RowNumber = LD.NumberOfRow	
						AND x.BF_SSN = PPD.BF_SSN
						AND x.LF_EDS = PPD.LF_EDS
					LEFT JOIN UDW..LN10_LON LN10
						ON x.BF_SSN = LN10.BF_SSN
						AND x.LN_SEQ = LN10.LN_SEQ
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 IN ('R', 'L')
					LEFT JOIN
					(	--Adding Active Interest Rate
						SELECT
							LN72.BF_SSN, 
							LN72.LN_SEQ,
							LN72.LR_ITR,
							ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
						FROM
							UDW..LN72_INT_RTE_HST LN72
						WHERE
							LN72.LC_STA_LON72 = 'A'
							AND	@Today BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							AND	LN72.BF_SSN = PPD.BF_SSN
					) LN72 
						ON LN10.BF_SSN = LN72.BF_SSN
						AND LN10.LN_SEQ = LN72.LN_SEQ
						AND LN72.SEQ = 1
					--LEFT JOIN /*GETS THE CURRENT BILL INFORMATION*/
					--(
					--	SELECT
					--		LN80.BF_SSN,
					--		LN80.LN_SEQ,
					--		SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS LA_BIL_PAS_DU,
					--		SUM(isnull(LN80.LA_BIL_CUR_DU,0)) AS LA_BIL_CUR_DU,
					--		SUM(ISNULL(LN80.LA_TOT_BIL_STS, 0)) AS LA_TOT_BIL_STS
					--	FROM 
					--		UDW..LN80_LON_BIL_CRF LN80
					--		INNER JOIN UDW..LN10_LON LN10
					--			ON LN10.BF_SSN = LN80.BF_SSN
					--			AND LN10.LN_SEQ = LN80.LN_SEQ
					--	WHERE 
					--		LN80.LC_STA_LON80 = 'A'
					--		AND LN80.LC_LON_STA_BIL = '1'
					--		AND CAST(LN80.LD_BIL_DU_LON AS DATE) < CAST(getdate() AS DATE)
					--		AND LN10.LC_STA_LON10 IN ('R','L')
					--		AND LN10.LA_CUR_PRI > 0
					--	GROUP BY 
					--		LN80.BF_SSN,
					--		LN80.LN_SEQ
					--) LN80
					--	ON LN10.BF_SSN = LN80.BF_SSN
					--	AND LN10.LN_SEQ = LN80.LN_SEQ
					LEFT JOIN
					(
						SELECT
							LN90.BF_SSN,
							LN90.LN_SEQ,
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL (18,2)))) AS TAPTP,
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL (18,2)))) AS TAPTI,
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0)AS DECIMAL (18,2)))) AS TAPTF, /*cumulative late fees paid*/
							SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0) AS DECIMAL(18,2)))) AS TAGAP
						FROM 
							UDW..LN90_FIN_ATY LN90
						WHERE
							LN90.PC_FAT_TYP = '10'
							AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
							AND LN90.LC_STA_LON90 = 'A'
						GROUP BY
							LN90.BF_SSN,
							LN90.LN_SEQ
					)LN90
						ON LN90.BF_SSN = LN10.BF_SSN
						AND LN90.LN_SEQ = LN10.LN_SEQ
					LEFT JOIN
					(
						SELECT
							LN15.BF_SSN,
							LN15.LN_SEQ,
							SUM(LN15.LA_DSB - COALESCE(LA_DSB_CAN, 0.00)) AS LA_DSB
						FROM
							UDW..LN15_DSB LN15
						WHERE	
							COALESCE(LA_DSB_CAN, 0.00) < LN15.LA_DSB
							AND LC_STA_LON15 IN (1, 3)
						GROUP BY
							LN15.BF_SSN,
							LN15.LN_SEQ		
					) LN15
						ON LN15.BF_SSN = LN10.BF_SSN
						AND LN15.LN_SEQ = LN10.LN_SEQ
					--LEFT JOIN UDW..LN16_LON_DLQ_HST LN16
					--	ON LN16.BF_SSN = LN10.BF_SSN
					--	AND LN16.LN_SEQ = LN10.LN_SEQ
					--	AND LN16.LC_STA_LON16 = '1'
					LEFT JOIN UDW..DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN
						AND DW01.LN_SEQ = LN10.LN_SEQ
						AND DW01.WC_DW_LON_STA IN ('18', '19')
						AND DW01.WX_OVR_DW_LON_STA != ''
					LEFT JOIN UDW..FormatTranslation FT
						ON FT.[Start] = LN10.IC_LON_PGM
						AND FT.FmtName = '$LNPROG'
				ORDER BY
					LD.NumberOfRow
				FOR XML PATH(''), TYPE
			).value('.','VARCHAR(MAX)'),
			1,0,''
		),
		@CostCenter AS CostCenter,
		@ReCapDoNotProcessEcorr AS DoNotProcessEcorr,
		CASE WHEN @ReCapDoNotProcessEcorr = 1 
			THEN 0 
			ELSE 
				CASE WHEN EMAIL_INFO.DI_VLD_CNC_EML_ADR = 'Y' AND EMAIL_INFO.DI_CNC_ELT_OPI = 'Y' 
					THEN 1 
					ELSE 0 
				END 
		END AS OnEcorr, --OnEcorr
		@ReCapArcNeeded AS ArcNeeded,
		@ReCapImagingNeeded AS ImagingNeeded,
		GETDATE() AS AddedAt,
		SUSER_NAME() AS AddedBy
	FROM
	(
		--Get the distinct coborrower
		SELECT DISTINCT
			PPD.BF_SSN,
			LN20.LF_EDS
		FROM
			ULS.retrocap.PrintProcessingDataReCap PPD
			INNER JOIN UDW..LN20_EDS LN20
				ON PPD.BF_SSN = LN20.BF_SSN
				AND PPD.LN_SEQ = LN20.LN_SEQ
		WHERE
			LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		GROUP BY
			PPD.BF_SSN,
			LN20.LF_EDS
	) PPD
	INNER JOIN 
	(
		SELECT
			BORR.DF_PRS_ID,
			--borrower info
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','')  AS Name,
			--address info
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_1)),',','')  AS Address1,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_2)),',','')  AS Address2,
			REPLACE(LTRIM(RTRIM(ADDR.DM_FGN_CNY)),',','') AS Country,
			BORR.DF_SPE_ACC_ID AS AccountNumber,
			REPLACE(LTRIM(RTRIM(ADDR.DM_CT)),',','')  AS City,
			CASE
				WHEN LEN(RTRIM(ADDR.DM_FGN_ST)) > 0 THEN  RTRIM(ADDR.DM_FGN_ST)
				ELSE RTRIM(ADDR.DC_DOM_ST)
			END AS [State],
			CASE WHEN LEN(ADDR.DF_ZIP_CDE) = 9 
				THEN LEFT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 4)
				ELSE LTRIM(RTRIM(ADDR.DF_ZIP_CDE))
			END AS Zip,
			CASE WHEN DI_VLD_ADR = 'Y' 
				THEN 1
				ELSE 0
			END AS HasValidAddress
		FROM
			UDW..PD10_PRS_NME BORR
			INNER JOIN UDW..PD30_PRS_ADR ADDR 
				ON ADDR.DF_PRS_ID = BORR.DF_PRS_ID
		WHERE
			--BORR.DF_PRS_ID = @BF_SSN
			ADDR.DC_ADR = 'L'
	) ADDR_INFO
		ON PPD.LF_EDS = ADDR_INFO.DF_PRS_ID 
	INNER JOIN UDW..PD10_PRS_NME PD10_BOR
		ON PPD.BF_SSN = PD10_BOR.DF_PRS_ID 
	LEFT JOIN UDW..PH05_CNC_EML EMAIL_INFO
		ON EMAIL_INFO.DF_SPE_ID = ADDR_INFO.AccountNumber
) LETTER_INFO
--Check for duplicate letters for the day
LEFT JOIN
(
	SELECT
		PP.BorrowerSsn,
		PP.AccountNumber,
		SUBSTRING(PP.LetterData, CHARINDEX(',',PP.LetterData,0), LEN(PP.LetterData) - CHARINDEX(',',PP.LetterData,0)) AS LetterData
	FROM
		ULS.[print].PrintProcessingCoBorrower PP
		INNER JOIN ULS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		INNER JOIN ULS.[print].Letters L
			ON L.LetterId = SD.LetterId
	WHERE
		--PP.AccountNumber = @AccountNumber --**FOR TESTING ONLY
		SD.ScriptId = @ScriptId
		AND L.Letter = @ReCapLetter
		AND PP.DeletedAt IS NULL
		AND CAST(PP.AddedAt AS DATE) >= @LastWeek

) PP
	ON PP.AccountNumber = LETTER_INFO.AccountNumber
	AND PP.LetterData = SUBSTRING(LETTER_INFO.LetterData, CHARINDEX(',',LETTER_INFO.LetterData,0), LEN(LETTER_INFO.LetterData) - CHARINDEX(',',LETTER_INFO.LetterData,0))
	AND PP.BorrowerSsn = LETTER_INFO.Ssn --Borrower Ssn
WHERE
	PP.AccountNumber IS NULL

;WITH LoanDetail(NumberOfRow) AS 
(
	SELECT 1
	UNION ALL
	SELECT NumberOfRow + 1
	FROM LoanDetail
	WHERE NumberOfRow < @NumberOfRows
)
--Create Cap Letters
INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT
	LETTER_INFO.AccountNumber,
	LETTER_INFO.EmailAddress,
	LETTER_INFO.ScriptDataId,
	LETTER_INFO.LetterData,
	LETTER_INFO.CostCenter,
	LETTER_INFO.DoNotProcessEcorr,
	LETTER_INFO.OnEcorr,
	LETTER_INFO.ArcNeeded,
	LETTER_INFO.ImagingNeeded,
	LETTER_INFO.AddedBy,
	LETTER_INFO.AddedAt
FROM
(
	SELECT DISTINCT	
		PPD.BF_SSN AS Ssn,
		ADDR_INFO.AccountNumber AS AccountNumber,
		CASE WHEN @CapDoNotProcessEcorr = 1 THEN 'Ecorr@uheaa.org' ELSE COALESCE(EMAIL_INFO.DX_CNC_EML_ADR, 'Ecorr@uheaa.org') END AS EmailAddress,
		@CapScriptDataId AS ScriptDataId,
		LetterData = CentralData.dbo.CreateACSKeyLine(ADDR_INFO.AccountNumber, 'B', 'L') + ',' + ADDR_INFO.[Name] + ',' + ADDR_INFO.Address1 + ',' + ADDR_INFO.Address2 + ',' + ADDR_INFO.City +
		',' + ADDR_INFO.[State] + ',' + ADDR_INFO.Zip + ',' + ADDR_INFO.Country + ',' + ADDR_INFO.AccountNumber +	
		STUFF
		(
			(
				SELECT	
					CASE WHEN x.BF_SSN IS NOT NULL THEN
						',' + COALESCE(FT.Label, LN10.IC_LON_PGM) --IC_LON_PGM
						+ ',' + CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 101) --LD_LON_1_DSB
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN15.LA_DSB, 0.00), 1) --OPAFEL Previously LN10.LA_LON_AMT_GTR
						+ ',' + CONVERT(VARCHAR(10), LN72.LR_ITR,0) + '%' --LR_INT_BIL
						+ ',' + '$' + CONVERT(VARCHAR(15), LN10.LA_CUR_PRI, 1) --LR_INT_BIL
						--+ ',' + CASE WHEN DW01.BF_SSN IS NOT NULL THEN CAST(0 AS VARCHAR(15)) ELSE CAST(ISNULL(LN16.LN_DLQ_MAX + 1, CASE WHEN COALESCE(LN80.LA_BIL_PAS_DU,0.00) > 0 THEN 1 ELSE 0 END) AS VARCHAR(15)) END --DAYS_DELQ
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN80.LA_BIL_PAS_DU,0.00), 1) --LA_BIL_PAS_DU
						--+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN10.LA_LTE_FEE_OTS, 0.00), 1) --LN_LTE_FEE
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.TotalCapitalized, 0.00) - COALESCE(x.LA_FAT_CUR_PRI, 0.00)) --Previous Capitalized Interest
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.LA_FAT_CUR_PRI, 0.00)) --Newly Capitalized Interest
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAPTP,0), 1) --TAPTP
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAPTI,0), 1) --TAPTI
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAPTF,0), 1) --TAPTF
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAGAP,0), 1) --TAGAP
						+ ',' + CONVERT(VARCHAR(10), x.LD_FAT_EFF, 101) --Date Capitalized
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.TotalCapitalized, 0.00)) --Total Capitalized Interest
					ELSE
						',,,,,,,,,'
					END
				FROM
					LoanDetail LD
					LEFT JOIN 
					(	
						SELECT DISTINCT	
							BF_SSN,
							LN_SEQ,
							LDPPD.LA_FAT_CUR_PRI,
							LDPPD.LD_FAT_EFF,
							LDPPD.TotalCapitalized,
							ROW_NUMBER() OVER (ORDER BY BF_SSN, LN_SEQ, LA_FAT_CUR_PRI) AS RowNumber
						FROM
							ULS.retrocap.PrintProcessingDataCap LDPPD
						WHERE
							PPD.BF_SSN = LDPPD.BF_SSN
					) x
						ON x.RowNumber = LD.NumberOfRow	
						AND x.BF_SSN = PPD.BF_SSN
					LEFT JOIN UDW..LN10_LON LN10
						ON x.BF_SSN = LN10.BF_SSN
						AND x.LN_SEQ = LN10.LN_SEQ
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 IN ('R', 'L')
					LEFT JOIN
					(	--Adding Active Interest Rate
						SELECT
							LN72.BF_SSN, 
							LN72.LN_SEQ,
							LN72.LR_ITR,
							ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
						FROM
							UDW..LN72_INT_RTE_HST LN72
						WHERE
							LN72.LC_STA_LON72 = 'A'
							AND	@Today BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							AND	LN72.BF_SSN = PPD.BF_SSN
					) LN72 
						ON LN10.BF_SSN = LN72.BF_SSN
						AND LN10.LN_SEQ = LN72.LN_SEQ
						AND LN72.SEQ = 1
					--LEFT JOIN /*GETS THE CURRENT BILL INFORMATION*/
					--(
					--	SELECT
					--		LN80.BF_SSN,
					--		LN80.LN_SEQ,
					--		SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS LA_BIL_PAS_DU,
					--		SUM(isnull(LN80.LA_BIL_CUR_DU,0)) AS LA_BIL_CUR_DU,
					--		SUM(ISNULL(LN80.LA_TOT_BIL_STS, 0)) AS LA_TOT_BIL_STS
					--	FROM 
					--		UDW..LN80_LON_BIL_CRF LN80
					--		INNER JOIN UDW..LN10_LON LN10
					--			ON LN10.BF_SSN = LN80.BF_SSN
					--			AND LN10.LN_SEQ = LN80.LN_SEQ
					--	WHERE 
					--		LN80.LC_STA_LON80 = 'A'
					--		AND LN80.LC_LON_STA_BIL = '1'
					--		AND CAST(LN80.LD_BIL_DU_LON AS DATE) < CAST(getdate() AS DATE)
					--		AND LN10.LC_STA_LON10 IN ('R','L')
					--		AND LN10.LA_CUR_PRI > 0
					--	GROUP BY 
					--		LN80.BF_SSN,
					--		LN80.LN_SEQ
					--) LN80
					--	ON LN10.BF_SSN = LN80.BF_SSN
					--	AND LN10.LN_SEQ = LN80.LN_SEQ
					--LEFT JOIN
					--(
					--	SELECT
					--		LN90.BF_SSN,
					--		LN90.LN_SEQ,
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL (18,2)))) AS TAPTP,
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL (18,2)))) AS TAPTI,
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0)AS DECIMAL (18,2)))) AS TAPTF, /*cumulative late fees paid*/
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0) AS DECIMAL(18,2)))) AS TAGAP
					--	FROM 
					--		UDW..LN90_FIN_ATY LN90
					--	WHERE
					--		LN90.PC_FAT_TYP = '10'
					--		AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
					--		AND LN90.LC_STA_LON90 = 'A'
					--	GROUP BY
					--		LN90.BF_SSN,
					--		LN90.LN_SEQ
					--)LN90
					--	ON LN90.BF_SSN = LN10.BF_SSN
					--	AND LN90.LN_SEQ = LN10.LN_SEQ
					LEFT JOIN
					(
						SELECT
							LN15.BF_SSN,
							LN15.LN_SEQ,
							SUM(LN15.LA_DSB - COALESCE(LA_DSB_CAN, 0.00)) AS LA_DSB
						FROM
							UDW..LN15_DSB LN15
						WHERE	
							COALESCE(LA_DSB_CAN, 0.00) < LN15.LA_DSB
							AND LC_STA_LON15 IN (1, 3)
						GROUP BY
							LN15.BF_SSN,
							LN15.LN_SEQ		
					) LN15
						ON LN15.BF_SSN = LN10.BF_SSN
						AND LN15.LN_SEQ = LN10.LN_SEQ
					--LEFT JOIN UDW..LN16_LON_DLQ_HST LN16
					--	ON LN16.BF_SSN = LN10.BF_SSN
					--	AND LN16.LN_SEQ = LN10.LN_SEQ
					--	AND LN16.LC_STA_LON16 = '1'
					LEFT JOIN UDW..DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN
						AND DW01.LN_SEQ = LN10.LN_SEQ
						AND DW01.WC_DW_LON_STA IN ('18', '19')
						AND DW01.WX_OVR_DW_LON_STA != ''
					LEFT JOIN UDW..FormatTranslation FT
						ON FT.[Start] = LN10.IC_LON_PGM
						AND FT.FmtName = '$LNPROG'
				ORDER BY
					LD.NumberOfRow
				FOR XML PATH(''), TYPE
			).value('.','VARCHAR(MAX)'),
			1,0,''
		),
		@CostCenter AS CostCenter,
		@CapDoNotProcessEcorr AS DoNotProcessEcorr,
		CASE WHEN @CapDoNotProcessEcorr = 1 
			THEN 0 
			ELSE 
				CASE WHEN EMAIL_INFO.DI_VLD_CNC_EML_ADR = 'Y' AND EMAIL_INFO.DI_CNC_ELT_OPI = 'Y' 
					THEN 1 
					ELSE 0 
				END 
		END AS OnEcorr, --OnEcorr
		@CapArcNeeded AS ArcNeeded,
		@CapImagingNeeded AS ImagingNeeded,
		GETDATE() AS AddedAt,
		SUSER_NAME() AS AddedBy
	FROM
	(
		SELECT DISTINCT
			PPD.BF_SSN
		FROM
			ULS.retrocap.PrintProcessingDataCap PPD
		GROUP BY
			PPD.BF_SSN
	) PPD
	INNER JOIN 
	(
		SELECT
			BORR.DF_PRS_ID,
			--borrower info
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','')  AS Name,
			--address info
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_1)),',','')  AS Address1,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_2)),',','')  AS Address2,
			REPLACE(LTRIM(RTRIM(ADDR.DM_FGN_CNY)),',','') AS Country,
			BORR.DF_SPE_ACC_ID AS AccountNumber,
			REPLACE(LTRIM(RTRIM(ADDR.DM_CT)),',','')  AS City,
			CASE
				WHEN LEN(RTRIM(ADDR.DM_FGN_ST)) > 0 THEN  RTRIM(ADDR.DM_FGN_ST)
				ELSE RTRIM(ADDR.DC_DOM_ST)
			END AS [State],
			CASE WHEN LEN(ADDR.DF_ZIP_CDE) = 9 
				THEN LEFT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 4)
				ELSE LTRIM(RTRIM(ADDR.DF_ZIP_CDE))
			END AS Zip,
			CASE WHEN DI_VLD_ADR = 'Y' 
				THEN 1
				ELSE 0
			END AS HasValidAddress
		FROM
			UDW..PD10_PRS_NME BORR
			INNER JOIN UDW..PD30_PRS_ADR ADDR 
				ON ADDR.DF_PRS_ID = BORR.DF_PRS_ID
		WHERE
			--BORR.DF_PRS_ID = @BF_SSN
			ADDR.DC_ADR = 'L'
	) ADDR_INFO
		ON PPD.BF_SSN = ADDR_INFO.DF_PRS_ID 
	LEFT JOIN UDW..PH05_CNC_EML EMAIL_INFO
		ON EMAIL_INFO.DF_SPE_ID = ADDR_INFO.AccountNumber
) LETTER_INFO
--Check for duplicate letters for the day
LEFT JOIN
(
	SELECT
		PP.AccountNumber,
		SUBSTRING(PP.LetterData, CHARINDEX(',',PP.LetterData,0), LEN(PP.LetterData) - CHARINDEX(',',PP.LetterData,0)) AS LetterData
	FROM
		ULS.[print].PrintProcessing PP
		INNER JOIN ULS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		INNER JOIN ULS.[print].Letters L
			ON L.LetterId = SD.LetterId
	WHERE
		--PP.AccountNumber = @AccountNumber --**FOR TESTING ONLY
		SD.ScriptId = @ScriptId
		AND L.Letter = @CapLetter
		AND PP.DeletedAt IS NULL
		AND CAST(PP.AddedAt AS DATE) >= @LastWeek

) PP
	ON PP.AccountNumber = LETTER_INFO.AccountNumber
	AND PP.LetterData = SUBSTRING(LETTER_INFO.LetterData, CHARINDEX(',',LETTER_INFO.LetterData,0), LEN(LETTER_INFO.LetterData) - CHARINDEX(',',LETTER_INFO.LetterData,0))
WHERE
	PP.AccountNumber IS NULL

--Create COBORROWER Cap Letters
;WITH LoanDetail(NumberOfRow) AS 
(
	SELECT 1
	UNION ALL
	SELECT NumberOfRow + 1
	FROM LoanDetail
	WHERE NumberOfRow < @NumberOfRows
)
INSERT INTO ULS.[print].PrintProcessingCoBorrower(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt,BorrowerSsn)
SELECT
	LETTER_INFO.AccountNumber,
	LETTER_INFO.EmailAddress,
	LETTER_INFO.ScriptDataId,
	LETTER_INFO.LetterData,
	LETTER_INFO.CostCenter,
	LETTER_INFO.DoNotProcessEcorr,
	LETTER_INFO.OnEcorr,
	LETTER_INFO.ArcNeeded,
	LETTER_INFO.ImagingNeeded,
	LETTER_INFO.AddedBy,
	LETTER_INFO.AddedAt,
	LETTER_INFO.Ssn --Borrower Ssn
FROM
(
	SELECT DISTINCT	
		PPD.BF_SSN AS Ssn, --Borrower Ssn
		--PPD.LF_EDS AS Ssn, --Coborrower Ssn
		ADDR_INFO.AccountNumber AS AccountNumber,
		CASE WHEN @CapDoNotProcessEcorr = 1 THEN 'Ecorr@uheaa.org' ELSE COALESCE(EMAIL_INFO.DX_CNC_EML_ADR, 'Ecorr@uheaa.org') END AS EmailAddress,
		@CapScriptDataId AS ScriptDataId,
		LetterData = CentralData.dbo.CreateACSKeyLine(PD10_BOR.DF_SPE_ACC_ID, 'B', 'L') + ',' + ADDR_INFO.[Name] + ',' + ADDR_INFO.Address1 + ',' + ADDR_INFO.Address2 + ',' + ADDR_INFO.City +
		',' + ADDR_INFO.[State] + ',' + ADDR_INFO.Zip + ',' + ADDR_INFO.Country + ',' + PD10_BOR.DF_SPE_ACC_ID +	
		STUFF
		(
			(
				SELECT	
					CASE WHEN x.BF_SSN IS NOT NULL THEN
						',' + COALESCE(FT.Label, LN10.IC_LON_PGM) --IC_LON_PGM
						+ ',' + CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 101) --LD_LON_1_DSB
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN15.LA_DSB, 0.00), 1) --OPAFEL Previously LN10.LA_LON_AMT_GTR
						+ ',' + CONVERT(VARCHAR(10), LN72.LR_ITR,0) + '%' --LR_INT_BIL
						+ ',' + '$' + CONVERT(VARCHAR(15), LN10.LA_CUR_PRI, 1) --LR_INT_BIL
						--+ ',' + CASE WHEN DW01.BF_SSN IS NOT NULL THEN CAST(0 AS VARCHAR(15)) ELSE CAST(ISNULL(LN16.LN_DLQ_MAX + 1, CASE WHEN COALESCE(LN80.LA_BIL_PAS_DU,0.00) > 0 THEN 1 ELSE 0 END) AS VARCHAR(15)) END --DAYS_DELQ
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN80.LA_BIL_PAS_DU,0.00), 1) --LA_BIL_PAS_DU
						--+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(LN10.LA_LTE_FEE_OTS, 0.00), 1) --LN_LTE_FEE
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.TotalCapitalized, 0.00) - COALESCE(x.LA_FAT_CUR_PRI, 0.00)) --Previous Capitalized Interest
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.LA_FAT_CUR_PRI, 0.00)) --Newly Capitalized Interest
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAPTP,0), 1) --TAPTP
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAPTI,0), 1) --TAPTI
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAPTF,0), 1) --TAPTF
						--+ ',' + '$' + CONVERT(VARCHAR(15),COALESCE(LN90.TAGAP,0), 1) --TAGAP
						+ ',' + CONVERT(VARCHAR(10), x.LD_FAT_EFF, 101) --Date Capitalized
						+ ',' + '$' + CONVERT(VARCHAR(15), COALESCE(x.TotalCapitalized, 0.00)) --Total Capitalized Interest
					ELSE
						',,,,,,,,,'
					END
				FROM
					LoanDetail LD
					LEFT JOIN 
					(	
						SELECT DISTINCT	
							LDPPD.BF_SSN,
							LN20.LF_EDS,
							LDPPD.LN_SEQ,
							LDPPD.LA_FAT_CUR_PRI,
							LDPPD.TotalCapitalized,
							LDPPD.LD_FAT_EFF,
							ROW_NUMBER() OVER (PARTITION BY LN20.LF_EDS ORDER BY LDPPD.BF_SSN, LDPPD.LN_SEQ, LDPPD.LA_FAT_CUR_PRI) AS RowNumber
						FROM
							ULS.retrocap.PrintProcessingDataCap LDPPD
							INNER JOIN UDW..LN20_EDS LN20
								ON LDPPD.BF_SSN = LN20.BF_SSN
								AND LDPPD.LN_SEQ = LN20.LN_SEQ
							WHERE
								LN20.LC_EDS_TYP = 'M'
								AND LN20.LC_STA_LON20 = 'A'
								AND PPD.BF_SSN = LDPPD.BF_SSN
								AND PPD.LF_EDS = LN20.LF_EDS
					) x
						ON x.RowNumber = LD.NumberOfRow	
						AND x.BF_SSN = PPD.BF_SSN
						AND x.LF_EDS = PPD.LF_EDS
					LEFT JOIN UDW..LN10_LON LN10
						ON x.BF_SSN = LN10.BF_SSN
						AND x.LN_SEQ = LN10.LN_SEQ
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 IN ('R', 'L')
					LEFT JOIN
					(	--Adding Active Interest Rate
						SELECT
							LN72.BF_SSN, 
							LN72.LN_SEQ,
							LN72.LR_ITR,
							ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
						FROM
							UDW..LN72_INT_RTE_HST LN72
						WHERE
							LN72.LC_STA_LON72 = 'A'
							AND	@Today BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							AND	LN72.BF_SSN = PPD.BF_SSN
					) LN72 
						ON LN10.BF_SSN = LN72.BF_SSN
						AND LN10.LN_SEQ = LN72.LN_SEQ
						AND LN72.SEQ = 1
					LEFT JOIN /*GETS THE CURRENT BILL INFORMATION*/
					(
						SELECT
							LN80.BF_SSN,
							LN80.LN_SEQ,
							SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS LA_BIL_PAS_DU,
							SUM(isnull(LN80.LA_BIL_CUR_DU,0)) AS LA_BIL_CUR_DU,
							SUM(ISNULL(LN80.LA_TOT_BIL_STS, 0)) AS LA_TOT_BIL_STS
						FROM 
							UDW..LN80_LON_BIL_CRF LN80
							INNER JOIN UDW..LN10_LON LN10
								ON LN10.BF_SSN = LN80.BF_SSN
								AND LN10.LN_SEQ = LN80.LN_SEQ
						WHERE 
							LN80.LC_STA_LON80 = 'A'
							AND LN80.LC_LON_STA_BIL = '1'
							AND CAST(LN80.LD_BIL_DU_LON AS DATE) < CAST(getdate() AS DATE)
							AND LN10.LC_STA_LON10 IN ('R','L')
							AND LN10.LA_CUR_PRI > 0
						GROUP BY 
							LN80.BF_SSN,
							LN80.LN_SEQ
					) LN80
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ
					--LEFT JOIN
					--(
					--	SELECT
					--		LN90.BF_SSN,
					--		LN90.LN_SEQ,
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL (18,2)))) AS TAPTP,
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL (18,2)))) AS TAPTI,
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0)AS DECIMAL (18,2)))) AS TAPTF, /*cumulative late fees paid*/
					--		SUM(ABS(CAST(COALESCE(LN90.LA_FAT_CUR_PRI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_NSI,0) AS DECIMAL(18,2)) + CAST(COALESCE(LN90.LA_FAT_LTE_FEE,0) AS DECIMAL(18,2)))) AS TAGAP
					--	FROM 
					--		UDW..LN90_FIN_ATY LN90
					--	WHERE
					--		LN90.PC_FAT_TYP = '10'
					--		AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
					--		AND LN90.LC_STA_LON90 = 'A'
					--	GROUP BY
					--		LN90.BF_SSN,
					--		LN90.LN_SEQ
					--)LN90
					--	ON LN90.BF_SSN = LN10.BF_SSN
					--	AND LN90.LN_SEQ = LN10.LN_SEQ
					LEFT JOIN
					(
						SELECT
							LN15.BF_SSN,
							LN15.LN_SEQ,
							SUM(LN15.LA_DSB - COALESCE(LA_DSB_CAN, 0.00)) AS LA_DSB
						FROM
							UDW..LN15_DSB LN15
						WHERE	
							COALESCE(LA_DSB_CAN, 0.00) < LN15.LA_DSB
							AND LC_STA_LON15 IN (1, 3)
						GROUP BY
							LN15.BF_SSN,
							LN15.LN_SEQ		
					) LN15
						ON LN15.BF_SSN = LN10.BF_SSN
						AND LN15.LN_SEQ = LN10.LN_SEQ
					--LEFT JOIN UDW..LN16_LON_DLQ_HST LN16
					--	ON LN16.BF_SSN = LN10.BF_SSN
					--	AND LN16.LN_SEQ = LN10.LN_SEQ
					--	AND LN16.LC_STA_LON16 = '1'
					LEFT JOIN UDW..DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN
						AND DW01.LN_SEQ = LN10.LN_SEQ
						AND DW01.WC_DW_LON_STA IN ('18', '19')
						AND DW01.WX_OVR_DW_LON_STA != ''
					LEFT JOIN UDW..FormatTranslation FT
						ON FT.[Start] = LN10.IC_LON_PGM
						AND FT.FmtName = '$LNPROG'
				ORDER BY
					LD.NumberOfRow
				FOR XML PATH(''), TYPE
			).value('.','VARCHAR(MAX)'),
			1,0,''
		),
		@CostCenter AS CostCenter,
		@CapDoNotProcessEcorr AS DoNotProcessEcorr,
		CASE WHEN @CapDoNotProcessEcorr = 1 
			THEN 0 
			ELSE 
				CASE WHEN EMAIL_INFO.DI_VLD_CNC_EML_ADR = 'Y' AND EMAIL_INFO.DI_CNC_ELT_OPI = 'Y' 
					THEN 1 
					ELSE 0 
				END 
		END AS OnEcorr, --OnEcorr
		@CapArcNeeded AS ArcNeeded,
		@CapImagingNeeded AS ImagingNeeded,
		GETDATE() AS AddedAt,
		SUSER_NAME() AS AddedBy
	FROM
	(
		--Get the distinct coborrower
		SELECT DISTINCT
			PPD.BF_SSN,
			LN20.LF_EDS
		FROM
			ULS.retrocap.PrintProcessingDataCap PPD
			INNER JOIN UDW..LN20_EDS LN20
				ON PPD.BF_SSN = LN20.BF_SSN
				AND PPD.LN_SEQ = LN20.LN_SEQ
		WHERE
			LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		GROUP BY
			PPD.BF_SSN,
			LN20.LF_EDS
	) PPD
	INNER JOIN 
	(
		SELECT
			BORR.DF_PRS_ID,
			--borrower info
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','')  AS Name,
			--address info
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_1)),',','')  AS Address1,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_2)),',','')  AS Address2,
			REPLACE(LTRIM(RTRIM(ADDR.DM_FGN_CNY)),',','') AS Country,
			BORR.DF_SPE_ACC_ID AS AccountNumber,
			REPLACE(LTRIM(RTRIM(ADDR.DM_CT)),',','')  AS City,
			CASE
				WHEN LEN(RTRIM(ADDR.DM_FGN_ST)) > 0 THEN  RTRIM(ADDR.DM_FGN_ST)
				ELSE RTRIM(ADDR.DC_DOM_ST)
			END AS [State],
			CASE WHEN LEN(ADDR.DF_ZIP_CDE) = 9 
				THEN LEFT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 4)
				ELSE LTRIM(RTRIM(ADDR.DF_ZIP_CDE))
			END AS Zip,
			CASE WHEN DI_VLD_ADR = 'Y' 
				THEN 1
				ELSE 0
			END AS HasValidAddress
		FROM
			UDW..PD10_PRS_NME BORR
			INNER JOIN UDW..PD30_PRS_ADR ADDR 
				ON ADDR.DF_PRS_ID = BORR.DF_PRS_ID
		WHERE
			--BORR.DF_PRS_ID = @BF_SSN
			ADDR.DC_ADR = 'L'
	) ADDR_INFO
		ON PPD.LF_EDS = ADDR_INFO.DF_PRS_ID 
	INNER JOIN UDW..PD10_PRS_NME PD10_BOR
		ON PPD.BF_SSN = PD10_BOR.DF_PRS_ID 
	LEFT JOIN UDW..PH05_CNC_EML EMAIL_INFO
		ON EMAIL_INFO.DF_SPE_ID = ADDR_INFO.AccountNumber
) LETTER_INFO
--Check for duplicate letters for the day
LEFT JOIN
(
	SELECT
		PP.BorrowerSsn,
		PP.AccountNumber,
		SUBSTRING(PP.LetterData, CHARINDEX(',',PP.LetterData,0), LEN(PP.LetterData) - CHARINDEX(',',PP.LetterData,0)) AS LetterData
	FROM
		ULS.[print].PrintProcessingCoBorrower PP
		INNER JOIN ULS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		INNER JOIN ULS.[print].Letters L
			ON L.LetterId = SD.LetterId
	WHERE
		--PP.AccountNumber = @AccountNumber --**FOR TESTING ONLY
		SD.ScriptId = @ScriptId
		AND L.Letter = @CapLetter
		AND PP.DeletedAt IS NULL
		AND CAST(PP.AddedAt AS DATE) >= @LastWeek

) PP
	ON PP.AccountNumber = LETTER_INFO.AccountNumber
	AND PP.LetterData = SUBSTRING(LETTER_INFO.LetterData, CHARINDEX(',',LETTER_INFO.LetterData,0), LEN(LETTER_INFO.LetterData) - CHARINDEX(',',LETTER_INFO.LetterData,0))
	AND PP.BorrowerSsn = LETTER_INFO.Ssn
WHERE
	PP.AccountNumber IS NULL




