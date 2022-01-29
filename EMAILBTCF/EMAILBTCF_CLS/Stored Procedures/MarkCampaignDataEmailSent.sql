CREATE PROCEDURE [emailbtcf].[MarkCampaignDataEmailSent]
	@CampaignDataId INT
AS

	UPDATE
		emailbtcf.CampaignData
	SET
		EmailSentAt = GETDATE()
	WHERE
		CampaignDataId = @CampaignDataId