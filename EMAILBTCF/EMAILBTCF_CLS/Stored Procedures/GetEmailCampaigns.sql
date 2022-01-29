CREATE PROCEDURE [emailbtcf].[GetEmailCampaigns]
AS

	SELECT 
		EC.EmailCampaignId,
		EC.SasFile,
		EC.LetterId,
		EC.SendingAddress,
		EC.SubjectLine,
		EC.Arc,
		EC.CommentText,
		MAX(CD.AddedAt) [WorkLastLoaded]
	FROM 
		emailbtcf.EmailCampaigns EC
		LEFT JOIN emailbtcf.CampaignData CD 
			ON EC.EmailCampaignId = CD.EmailCampaignId
	WHERE
		EC.InactivatedAt IS NULL
	GROUP BY
		EC.EmailCampaignId,
		EC.SasFile,
		EC.LetterId,
		EC.SendingAddress,
		EC.SubjectLine,
		EC.Arc,
		EC.CommentText,
		EC.InactivatedAt