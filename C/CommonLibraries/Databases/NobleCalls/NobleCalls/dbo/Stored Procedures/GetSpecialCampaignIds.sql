CREATE PROCEDURE [dbo].[GetSpecialCampaignIds]

AS
	SELECT 
		CallCampaign
	FROM	
		CallCampaigns 
	WHERE
		IsSpecialCampaign = 1