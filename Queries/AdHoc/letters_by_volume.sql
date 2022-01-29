DECLARE @DATE1 DATE = GETDATE() - 90
--SELECT @DATE1

;WITH TOTAL AS
(
	SELECT
		DD.LetterId
		,COUNT(DD.DocumentDetailsId) AS VOLUME_90_DAYS
	FROM 
		[ECorrFed].[dbo].[DocumentDetails] DD
	WHERE
		DD.DocDate >= @DATE1
	GROUP BY
		DD.LetterId
)
SELECT
	T.LetterId
	,L.Letter
	,T.VOLUME_90_DAYS
FROM 
	TOTAL T
	LEFT JOIN Letters L
		ON T.LetterId = L.LetterId
ORDER BY
	VOLUME_90_DAYS DESC
