CREATE PROCEDURE [emailbatch].[GetCampaignsToLoad]
	
AS
	SELECT 
		EC.EmailCampaignId,
		EC.SourceFile,
		EC.ProcessAllFiles,
		EC.OKIfMissing,
		EC.OKIfEmpty
	FROM
		emailbatch.EmailCampaigns EC
	WHERE
		EC.DeletedAt IS NULL
RETURN 0
