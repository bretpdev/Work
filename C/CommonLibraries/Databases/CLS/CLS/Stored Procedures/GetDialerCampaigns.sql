CREATE PROCEDURE [dbo].[GetDialerCampaigns]

AS
	SELECT 
		DialerCampaignId,
		DialerCampaign
	FROM
		DialerCampaigns
RETURN 0
