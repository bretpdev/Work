CREATE PROCEDURE [covidforb].[SetArcAddProcessingId]
(
	@ForbProcessingId BIGINT,
	@ArcAddProcessingId INT,
	@PrintProcessingId INT = NULL
)
AS

BEGIN TRANSACTION
BEGIN TRY
	
	UPDATE 
		[covidforb].ForbearanceProcessing 
	SET 
		ArcAddProcessingId = @ArcAddProcessingId,
		PrintProcessingId = @PrintProcessingId
	WHERE 
		ForbearanceProcessingId = @ForbProcessingId
		AND ArcAddProcessingId IS NULL
		AND DeletedAt IS NULL

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	THROW
END CATCH