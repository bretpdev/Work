CREATE PROCEDURE [skpemlbrw].[GetCampaignId]
AS
	SELECT 
		EC.EmailCampaignId
	FROM
		emailbatch.EmailCampaigns EC
		INNER JOIN emailbatch.HTMLFiles HF
			ON EC.HTMLFileId = HF.HTMLFileId
	WHERE
		HF.HTMLFile = 'SKIPEML.html'
		AND EC.DeletedAt IS NULL
