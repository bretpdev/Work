CREATE PROCEDURE [clschllnfd].[UpdatePrintProcessingId]
	@SchoolClosureDataId INT,
	@PrintProcessingId INT
AS
	UPDATE
		SchoolClosureData
	SET
		PrintProcessingId = @PrintProcessingId
	WHERE
		SchoolClosureDataId = @SchoolClosureDataId
RETURN 0