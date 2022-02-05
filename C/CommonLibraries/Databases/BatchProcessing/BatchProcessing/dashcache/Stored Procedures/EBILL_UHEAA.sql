CREATE PROCEDURE [dashcache].[EBILL_UHEAA]
AS

	SELECT
		COUNT(*)
	FROM
		ULS..EBill EB
	WHERE
		(
			EB.ArcAddedAt IS NULL
			OR
			EB.UpdatedAt IS NULL
		)
		AND
		EB.AddedAt < CAST(CONVERT(VARCHAR(15), [CentralData].dbo.AddBusinessDays(GETDATE(), -1), 110) + ' ' + CONVERT(VARCHAR(12), GETDATE(), 114) as DATETIME)

RETURN 0