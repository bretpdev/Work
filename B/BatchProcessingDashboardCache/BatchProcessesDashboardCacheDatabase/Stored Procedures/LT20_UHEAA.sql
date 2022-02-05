CREATE PROCEDURE [dashcache].[LT20_UHEAA]
AS

	SELECT
		COUNT(*)
	FROM
		UDW..LT20_LTR_REQ_PRC LT20
	WHERE
		LT20.InactivatedAt IS NULL
		AND
		(
			CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE) < CentralData.dbo.AddBusinessDays(GETDATE(), -2)
			OR
			LT20.CreatedAt < CentralData.dbo.AddBusinessDays(GETDATE(), -2)
		)
		AND
		(
			(
				LT20.EcorrDocumentCreatedAt IS NULL
			)
			OR
			(
				LT20.OnEcorr = 0
				AND
				LT20.PrintedAt IS NULL
			)
		)

RETURN 0
