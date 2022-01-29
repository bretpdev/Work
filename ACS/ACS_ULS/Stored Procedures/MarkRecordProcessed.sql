CREATE PROCEDURE [acs].[MarkRecordProcessed]
	@UheaaDemographicsId BIGINT,
	@ArcId INT

AS

BEGIN
	UPDATE 
		[ULS].[acs].[UheaaDemographics]
	SET
		ProcessedAt = GETDATE(), 
		ArcAddProcessingId = @ArcId
	WHERE
		UheaaDemographicsId = @UheaaDemographicsId
END