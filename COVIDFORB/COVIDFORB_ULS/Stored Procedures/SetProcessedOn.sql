CREATE PROCEDURE [covidforb].[SetProcessedOn]
(
	@ForbProcessingId BIGINT,
	@ArcComment VARCHAR(300),
	@Failure BIT
)
AS

	UPDATE 
		[covidforb].ForbearanceProcessing 
	SET 
		ForbearanceProcessedOn = GETDATE(),
		ArcComment = @ArcComment,
		Failure = @Failure
	WHERE 
		ForbearanceProcessingId = @ForbProcessingId
		AND ForbearanceProcessedOn IS NULL
		AND DeletedAt IS NULL