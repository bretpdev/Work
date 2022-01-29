CREATE PROCEDURE [tcpapns].[OneLinkLoadData]
( 
	@SourceFile VARCHAR(100)
)
AS
DECLARE 
	@AccountNumberIndex INT = 0,
	@PhoneIndex INT = 1,
	@DateIndex INT = 2,
	@WIRIndex INT = 3
BEGIN
--Add records into FileProcessing if they haven't already loaded from the file,

INSERT INTO [tcpapns].[OneLinkFileProcessing]
(
	GroupKey,
	SourceFile,
	LineData,
	HasConsentArcOneLink,
	AccountNumber,
	Phone,
	RecordDate,
	MobileIndicator,
	AddedBy,
	AddedAt,
	DeletedAt,
	DeletedBy
)
SELECT
	BL.GroupKey AS GroupKey, --Key Identifier
	@SourceFile,
	BL.LineData,
	CASE WHEN AY01.DF_PRS_ID IS NOT NULL THEN 1
		ELSE 0
	END,
	BL.AccountNumber,
	BL.Phone,
	BL.RecordDate,
	CASE WHEN BL.WIR = 'WIR' THEN 1 ELSE 0 END,
	SUSER_NAME(),
	GETDATE(),
	CASE WHEN BL.LineData IS NULL OR BL.LineData = '' THEN GETDATE()
		WHEN BL.GroupKey = '' THEN GETDATE()
		WHEN BL.RecordDate = '' THEN GETDATE()
		WHEN PD01.DF_PRS_ID IS NULL AND BR03.DF_PRS_ID_RFR IS NULL THEN GETDATE()
		ELSE NULL
	END,
	CASE WHEN BL.LineData IS NULL OR BL.LineData = '' THEN SUSER_NAME()
		WHEN BL.GroupKey = '' THEN SUSER_NAME()
		WHEN BL.RecordDate = '' THEN SUSER_NAME()
		WHEN PD01.DF_PRS_ID IS NULL AND BR03.DF_PRS_ID_RFR IS NULL THEN SUSER_NAME()
		ELSE NULL
	END
FROM
	(
		SELECT
			CAST(dbo.SplitAndRemoveQuotes(REPLACE(BL.LineData, '"', ''), ',', @AccountNumberIndex, 1) AS VARCHAR(50)) AS GroupKey,
			CAST(tcpapns.JulianDateConversion(dbo.SplitAndRemoveQuotes(BL.LineData, ',', @AccountNumberIndex, 1), dbo.SplitAndRemoveQuotes(BL.LineData, ',', @DateIndex, 1)) AS VARCHAR(10)) AS AccountNumber,
			CAST(dbo.SplitAndRemoveQuotes(BL.LineData, ',', @PhoneIndex, 1) AS VARCHAR(20)) AS Phone,
			CAST(dbo.SplitAndRemoveQuotes(BL.LineData, ',', @DateIndex, 1) AS VARCHAR(10)) AS RecordDate,
			CAST(UPPER(dbo.SplitAndRemoveQuotes(BL.LineData, ',', @WIRIndex, 1)) AS VARCHAR(3)) AS WIR,
			BL.LineData AS LineData
		FROM
			[tcpapns].[_OneLinkBulkLoad] BL
	) BL
	LEFT JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_SPE_ACC_ID = BL.AccountNumber
	LEFT JOIN ODW..BR03_BR_REF BR03
		ON BR03.DF_PRS_ID_RFR = BL.AccountNumber
		AND BR03.BC_STA_BR03 = 'A'
	LEFT JOIN
	(
		SELECT
			MAX(AY01.BD_ATY_PRF) AS BD_ATY_PRF,
			DF_PRS_ID
		FROM
			ODW..AY01_BR_ATY AY01
			INNER JOIN Phone_Consent_Arcs PCA
				ON AY01.PF_ACT = Arc
				AND PCA.Compass = 0
		GROUP BY 
			DF_PRS_ID
	) AY01
		ON PD01.DF_PRS_ID = AY01.DF_PRS_ID
		OR BR03.DF_PRS_ID_RFR = AY01.DF_PRS_ID
	LEFT JOIN [tcpapns].[OneLinkFileProcessing] FP
		ON BL.LineData = FP.LineData
		AND @SourceFile = FP.SourceFile
		AND 
		(
			(
				FP.DeletedAt IS NULL
				AND FP.DeletedBy IS NULL
			)
			OR
			(
				-- Exclude duplicates of records that are supposed to be deleted
				FP.LineData IS NULL 
				OR FP.LineData = ''
				OR dbo.SplitAndRemoveQuotes(FP.LineData, ',', @AccountNumberIndex, 1) = ''
				OR dbo.SplitAndRemoveQuotes(FP.LineData, ',', @DateIndex, 1) = ''
				OR PD01.DF_PRS_ID IS NULL
			)
		)
WHERE
	FP.FileProcessingId IS NULL

DELETE FROM [tcpapns]._OneLinkBulkLoad

END