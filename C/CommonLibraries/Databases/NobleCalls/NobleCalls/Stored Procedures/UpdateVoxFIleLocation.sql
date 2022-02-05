CREATE PROCEDURE [dbo].[UpdateVoxFIleLocation]
	@CallId int,
	@VoxFileLocation varchar(100)
AS
	UPDATE
		NobleCallHistory
	SET
		VoxFileLocation = CASE WHEN @VoxFileLocation = '' THEN NULL ELSE @VoxFileLocation END,
		VoxVerifiedAt = CASE WHEN @VoxFileLocation = '' THEN NULL ELSE GETDATE() END
	WHERE
		NobleCallHistoryId = @CallId
