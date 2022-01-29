CREATE PROCEDURE [tcpapns].[LoadData]
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

INSERT INTO [tcpapns].[FileProcessing]
(
	GroupKey,
	SourceFile,
	LineData,
	HasConsentArc,
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
	CASE WHEN AY10.BF_SSN IS NOT NULL OR AY10_Endorser.LF_ATY_RGD_TO IS NOT NULL THEN 1
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
		WHEN PD10.DF_PRS_ID IS NULL THEN GETDATE()
		ELSE NULL
	END,
	CASE WHEN BL.LineData IS NULL OR BL.LineData = '' THEN SUSER_NAME()
		WHEN BL.GroupKey = '' THEN SUSER_NAME()
		WHEN BL.RecordDate = '' THEN SUSER_NAME()
		WHEN PD10.DF_PRS_ID IS NULL THEN SUSER_NAME()
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
			[tcpapns].[_BulkLoad] BL
	) BL
	LEFT JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = BL.AccountNumber
	--May need to not be treated as an endorser in some cases
	LEFT JOIN 
	(
		SELECT
			LN20.LF_EDS
		FROM
			UDW..LN20_EDS LN20	
		WHERE
			LN20.LC_STA_LON20 = 'A'
		GROUP BY
			LN20.LF_EDS
	) LN20
		ON PD10.DF_PRS_ID = LN20.LF_EDS
	--AY10 Endorser
	LEFT JOIN
	(
		SELECT
			MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV,
			LF_ATY_RGD_TO
		FROM
			UDW..AY10_BR_LON_ATY AY10
			INNER JOIN Phone_Consent_Arcs PCA
				ON AY10.PF_REQ_ACT = Arc
				AND PCA.Compass = 1
		WHERE
			LC_STA_ACTY10 = 'A'
			AND LC_ATY_RGD_TO = 'E'
		GROUP BY
			LF_ATY_RGD_TO
	) AY10_Endorser
		ON LN20.LF_EDS = AY10_Endorser.LF_ATY_RGD_TO 
	--AY10 
	LEFT JOIN 
	(
		SELECT
			MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV,
			BF_SSN
		FROM
			UDW..AY10_BR_LON_ATY AY10
			INNER JOIN Phone_Consent_Arcs PCA
				ON AY10.PF_REQ_ACT = Arc
				AND PCA.Endorser = 0
				AND PCA.Compass = 1
		WHERE
			LC_STA_ACTY10 = 'A'
		GROUP BY
			BF_SSN
	) AY10
		ON PD10.DF_PRS_ID = AY10.BF_SSN
	LEFT JOIN [tcpapns].[FileProcessing] FP
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
				OR PD10.DF_PRS_ID IS NULL
			)
		)
WHERE
	FP.FileProcessingId IS NULL

DELETE FROM [tcpapns]._BulkLoad

END