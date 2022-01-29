CREATE PROCEDURE [textcoord].[GetCampaigns]
AS
	SELECT
		CampaignId,
		Campaign,
		ISNULL(CampaignCode, Campaign) AS CampaignCode,
		Sproc
	FROM
		textcoord.Campaigns
RETURN 0
