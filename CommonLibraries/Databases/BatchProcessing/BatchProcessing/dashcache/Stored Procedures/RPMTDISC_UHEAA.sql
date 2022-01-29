CREATE PROCEDURE [dashcache].[RPMTDISC_UHEAA]
AS

	SELECT
		COUNT(*)
	FROM
		ULS.[print].[PrintProcessing] PP
		INNER JOIN ULS.[print].[ScriptData] SD ON SD.ScriptDataId = PP.ScriptDataId
	WHERE
		SD.ScriptId = 'REPAYDISCL'
		AND
		PP.DeletedAt IS NULL
		AND
		PP.DeletedBy IS NULL
		AND
		PP.AddedAt < CentralData.dbo.AddBusinessDays(GETDATE(), -2)
		AND
		(
			(
				PP.OnEcorr = 0
				AND
				PP.PrintedAt IS NULL
			)
			OR
			(
				PP.ImagingNeeded = 1
				AND
				PP.ImagedAt IS NULL
			)
			OR
			(
				PP.OnEcorr = 1
				AND
				PP.ECorrDocumentCreatedAt IS NULL
			)
			OR
			(
				PP.ArcNeeded = 1
				AND
				PP.ArcAddProcessingId IS NULL
			)
		)

RETURN 0