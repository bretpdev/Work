CREATE PROCEDURE [dbo].[GetCallCampaigns]
AS
	SELECT
		CC.CallCampaignId,
		CC.CampaignCode,
		CC.CampaignName
	FROM
		CallCampaigns CC
	WHERE
		CC.active = 1
RETURN 0
