--Letters
SELECT
	COUNT(DD.LetterId) AS CountLetter,
	L.Letter
FROM
	ECorrFed..DocumentDetails DD
	INNER JOIN ECorrFed..Letters L
		ON L.LetterId = DD.LetterId
WHERE
	(CAST(DD.Printed AS DATE) >= CAST(DATEADD(DAY,-XX,GETDATE()) AS DATE)
	OR CAST(DD.EmailSent AS DATE) >= CAST(DATEADD(DAY,-XX,GETDATE()) AS DATE))
	AND L.Active = X
GROUP BY
	L.Letter


SELECT
	COUNT(CD.CampaignDataId) AS CountEmail,
	EC.LetterId
FROM
	CLS.emailbtcf.CampaignData CD
	INNER JOIN CLS.emailbtcf.EmailCampaigns EC
		ON EC.EmailCampaignId = CD.EmailCampaignId
WHERE
	CAST(CD.EmailSentAt AS DATE) >= CAST(DATEADD(DAY,-XX,GETDATE()) AS DATE)
	AND CD.InactivatedAt IS NULL
	AND EC.InactivatedAt IS NULL
GROUP BY
	EC.LetterId