SELECT
	AccountNumber,
	ISNULL(PP.PrintedAt, PP.EcorrDocumentCreatedAt) AS DATE_SENT,
	'LETTER' AS CORRESPONDENCE_TYPE,
	L.Letter AS LETTER_ID,
	ISNULL(A.Arc,'') AS ARC,
	SD.ScriptID AS [SOURCE]
FROM
	ULS.[print].PrintProcessing PP
	INNER JOIN ULS.[print].ScriptData SD
		ON SD.ScriptDataId = PP.ScriptDataId
	INNER JOIN ULS.[print].Letters L
		ON L.LetterId = SD.LetterId
	LEFT JOIN ULS.[print].ArcScriptDataMapping ASD
		ON ASD.ScriptDataId = SD.ScriptDataId
	LEFT JOIN ULS.[print].Arcs A
		ON A.ArcId = ASD.ArcId
	LEFT JOIN ULS.[print].Comments C
		ON C.CommentId = ASD.CommentId
WHERE
	AccountNumber = '9979464425'
	AND PP.AddedAt BETWEEN '11/01/2018' AND '01/15/2020'

UNION ALL

SELECT
	LT20.DF_SPE_ACC_ID AS AccountNumber,
	ISNULL(LT20.PrintedAt, LT20.EcorrDocumentCreatedAt) AS DATE_SENT,
	'LETTER' AS CORRESPONDENCE_TYPE,
	LT20.RM_DSC_LTR_PRC AS LETTER_ID,
	AC11.PF_REQ_ACT AS ARC,
	'SYSTEM LETTERS' AS [SOURCE]
FROM
	UDW..LT20_LTR_REQ_PRC LT20
	LEFT JOIN UDW..AC11_ACT_REQ_LTR AC11
		ON AC11.PF_LTR = LT20.RM_DSC_LTR_PRC
WHERE
	LT20.CreatedAt BETWEEN '11/01/2018' AND '01/15/2020' 
	AND LT20.InactivatedAt IS NULL
	AND LT20.DF_SPE_ACC_ID = '9979464425'

UNION ALL

SELECT DISTINCT
	EP.AccountNumber,
	EmailSentAt AS DATE_SENT,
	'EMAIL' AS CORRESPONDENCE_TYPE,
	REPLACE(H.HTMLFile,'.html', '') AS LETTER_ID,
	ISNULL(A.ARC,'') AS ARC,
	ISNULL(NULLIF(EC.SourceFile,''),'N/A') AS [SOURCE]
FROM
	ULS.emailbatch.EmailProcessing EP
	INNER JOIN ULS.emailbatch.EmailCampaigns EC
		ON EP.EmailCampaignId = EP.EmailCampaignId
	INNER JOIN ULS.emailbatch.HTMLFiles H
		ON H.HTMLFileId = EC.HTMLFileId
	LEFT JOIN ULS.emailbatch.Arcs A
		ON EC.ArcId = A.ArcId
	LEFT JOIN ULS.emailbatch.Comments C
		ON C.CommentId = EC.CommentId
WHERE
	AccountNumber = '9979464425'
	AND EP.AddedAt BETWEEN '11/01/2018' AND '01/15/2020' 
ORDER BY 
	DATE_SENT