SELECT
	REPLACE(EC.LetterId, '.HTML','') AS LETTER_ID,
	CAST(MAX(CD.EmailSentAt) AS DATE) AS LAST_SENT
FROM
	CLS.emailbtcf.EmailCampaigns EC
	INNER JOIN CLS.emailbtcf.CampaignData CD
		ON CD.EmailCampaignId = EC.EmailCampaignId
WHERE 
	CD.EmailSentAt BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
GROUP BY
	EC.LetterId

UNION ALL

SELECT
	L.Letter AS LETTER_ID,
	CAST(MAX(DD.AddedAt) AS DATE) AS LAST_SENT
FROM
	ECorrFed..DocumentDetails DD
	INNER JOIN ECorrFed..Letters L
		ON L.LetterId = DD.LetterId
WHERE
	DD.AddedAt BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
GROUP BY
	L.Letter

ORDER BY LAST_SENT DESC