CREATE PROCEDURE [emailbtcf].[MarkCampaignDataArcProcessed]
	@CampaignDataId INT,
	@ArcAddProcessingId BIGINT = NULL
AS

	UPDATE
		emailbtcf.CampaignData
	SET
		ArcProcessedAt = GETDATE(),
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		CampaignDataId = @CampaignDataId
