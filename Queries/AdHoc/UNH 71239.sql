
SELECT DISTINCT
	 [DOC_ID]
      ,[TITLE]
      ,[LETTER_TYPE]
      ,[Description]
      ,ISNULL(CONVERT(VARCHAR, L.LAST_TRIGGERED, 101), 'N/A') AS [Last_Triggered]
      ,[MOST_RECENT_PROMOTION]
  FROM [UDW].[dbo].[UNH 71239] UNH
  LEFT JOIN
  (
	SELECT
	 P.LETTER,
	 MAX(LAST_TRIGGERED) AS LAST_TRIGGERED
	FROM
	(
	SELECT
		L.Letter,
		MAX(PP.AddedAt) AS LAST_TRIGGERED
	FROM
		ULS.[print].PrintProcessing pp
		inner join uls.[print].ScriptData sd
			on pp.ScriptDataId = pp.ScriptDataId
		inner join uls.[print].Letters l
			on l.LetterId = sd.LetterId
		WHERE PP.DeletedAt IS NULL
	GROUP BY L.Letter

	UNION
		SELECT
		L.Letter,
		MAX(PP.AddedAt) AS LAST_TRIGGERED
	FROM
		ULS.[print].PrintProcessingCoBorrower pp
		inner join uls.[print].ScriptData sd
			on pp.ScriptDataId = pp.ScriptDataId
		inner join uls.[print].Letters l
			on l.LetterId = sd.LetterId
		WHERE PP.DeletedAt IS NULL
	GROUP BY L.Letter

	UNION

		SELECT
			RM_DSC_LTR_PRC,
			MAX(CREATEDAT)
		FROM
			UDW..LT20_LTR_REQ_PRC
		WHERE
			InactivatedAt IS NULL
		GROUP BY RM_DSC_LTR_PRC

	UNION
		SELECT
			REPLACE(H.HTMLFile, '.HTML','') AS LETTER,
			MAX(EP.EmailSentAt)
		FROM
			ULS.emailbatch.EmailProcessing EP
			INNER JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = EP.EmailCampaignId
			INNER JOIN ULS.emailbatch.HTMLFiles H
				ON H.HTMLFileId = EC.HTMLFileId
		GROUP BY
			REPLACE(H.HTMLFile, '.HTML','') 
	) P
	GROUP BY P.Letter

  ) L
	ON L.Letter = UNH.DOC_ID