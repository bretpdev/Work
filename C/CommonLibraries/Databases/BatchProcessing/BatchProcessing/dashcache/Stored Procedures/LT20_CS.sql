CREATE PROCEDURE [dashcache].[LT20_CS]
AS

	SELECT
		COUNT(*)
	FROM 
		CDW..LT20_LetterRequests LT20
	WHERE 
		LT20.PrintedAt IS NULL 
		AND 
		LT20.InactivatedAt IS NULL 
		AND
		CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE) <= [CentralData].dbo.AddBusinessDays(GETDATE(), -1)