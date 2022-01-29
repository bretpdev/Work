CREATE PROCEDURE [dashcache].[CHECKBYPHONE_CS]
AS

	SELECT
		COUNT(*)
	FROM
		CLS.dbo.CheckByPhone CBP
	WHERE
		CBP.Deleted = 0
		AND
		CBP.EffectiveDate < CentralData.dbo.AddBusinessDays(GETDATE(), -1)
		AND
		CBP.CreatedAt < CentralData.dbo.AddBusinessDays(GETDATE(), -1)
		AND
		CBP.EffectiveDate >= '2017-3-1' -- exclude any old problem records
		AND
		(
			CBP.ProcessedDate IS NULL
			OR
			CBP.[FileName] IS NULL
	)

RETURN 0
