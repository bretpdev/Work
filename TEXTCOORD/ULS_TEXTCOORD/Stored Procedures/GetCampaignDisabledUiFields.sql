CREATE PROCEDURE [textcoord].[GetCampaignDisabledUiFields]
	@CampaignId INT
AS
	SELECT
		CampaignDisabledUiFieldId, 
		CampaignId, 
		UiFieldId
	FROM
		textcoord.CampaignDisabledUiFields
	WHERE
		CampaignId = @CampaignId
RETURN 0