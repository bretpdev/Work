CREATE PROCEDURE [emailbtcf].[GetCampaignData]
	@EmailCampaignId INT
AS
	
SELECT
	CD.[CampaignDataId],
	CD.[EmailCampaignId],
	CD.[Recipient], 
	CD.[AccountNumber], 
	CD.[FirstName], 
	CD.[LastName],
	CD.[EmailSentAt],
	CD.[ArcProcessedAt],
	CD.[ArcAddProcessingId],
	COALESCE(LD.LineData,'') AS LineData
FROM
	emailbtcf.CampaignData CD
	LEFT JOIN emailbtcf.LineData LD
		ON LD.LineDataId = CD.LineDataId
WHERE
	(
		CD.EmailSentAt IS NULL 
		OR CD.ArcProcessedAt IS NULL
	)
	AND CD.InactivatedAt IS NULL
	AND CD.EmailCampaignId = @EmailCampaignId
