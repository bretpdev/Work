CREATE PROCEDURE [acs].[MarkRecordProcessed]
	@OneLinkDemographicsId BIGINT,
	@ArcId INT

AS

BEGIN
	UPDATE 
		[OLS].[acs].[OneLinkDemographics]
	SET
		ProcessedAt = GETDATE(), 
		ArcAddProcessingId = @ArcId
	WHERE
		OneLinkDemographicsId = @OneLinkDemographicsId
END