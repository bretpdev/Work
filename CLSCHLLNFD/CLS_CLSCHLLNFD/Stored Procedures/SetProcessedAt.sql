CREATE PROCEDURE [clschllnfd].[SetProcessedAt]
	@SchoolClosureDataId INT
AS
	UPDATE
		clschllnfd.SchoolClosureData
	SET
		ProcessedAt = GETDATE()
	WHERE
		SchoolClosureDataId = @SchoolClosureDataId
RETURN 0