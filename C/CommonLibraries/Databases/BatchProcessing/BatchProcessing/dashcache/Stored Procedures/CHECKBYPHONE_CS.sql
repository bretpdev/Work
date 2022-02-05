CREATE PROCEDURE [dashcache].[CHECKBYPHONE_CS]
AS

	SELECT
		COUNT(*)
	FROM
		CLS.dbo.CheckByPhone CBP
	WHERE
		CBP.Deleted = 0
		AND
		(
			CAST(CBP.EffectiveDate AS DATE) < CAST(GETDATE() AS DATE)
			OR
			(
				CAST(CBP.EffectiveDate AS DATE) <= CAST(GETDATE() AS DATE)
				AND
				DATEPART(HOUR, GETDATE()) > 15
			)
		)
		AND
		CBP.EffectiveDate >= '2017-3-1' -- exclude any old problem records
		AND
		(
			CBP.ProcessedDate IS NULL
			OR
			CBP.[FileName] IS NULL
		)


RETURN 0