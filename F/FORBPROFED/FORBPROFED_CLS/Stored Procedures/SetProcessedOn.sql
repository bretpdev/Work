CREATE PROCEDURE [forb].[SetProcessedOn]
(
	@ForbProcessingId BIGINT,
	@ArcAddProcessingId INT
)
AS

	UPDATE 
		[forb].ForbearanceProcessing 
	SET 
		ForbearanceProcessedOn = GETDATE(),
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE 
		ForbearanceProcessingId = @ForbProcessingId
		AND ForbearanceProcessedOn IS NULL
		AND DeletedAt IS NULL