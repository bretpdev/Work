CREATE PROCEDURE [dashcache].[CHECKBYPHONE_UHEAA]
AS

	SELECT
		COUNT(ID)
	FROM
		NORAD.[dbo].[CKPH_DAT_OPSCheckByPhone] CBP
	WHERE
		CBP.Deleted = 0
		AND
		CAST(CBP.EffectiveDate AS DATETIME) < master.dbo.AddBusinessDays(GETDATE(), -1)
		AND
		CAST(CBP.CreatedAt AS DATETIME) < master.dbo.AddBusinessDays(GETDATE(), -1)
		AND
		CAST(CBP.EffectiveDate AS DATETIME) >= '2017-3-1' -- exclude any old problem records
		AND
		(
			CBP.ProcessedDate IS NULL
			OR
			CBP.[FileName] IS NULL
		)

RETURN 0