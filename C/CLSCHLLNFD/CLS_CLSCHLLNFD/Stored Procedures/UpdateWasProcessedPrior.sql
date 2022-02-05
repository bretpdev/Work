CREATE PROCEDURE [clschllnfd].[UpdateWasProcessedPrior]
	@SchoolClosureDataId INT
AS
	UPDATE
		clschllnfd.SchoolClosureData
	SET
		WasProcessedPrior = 1
	WHERE
		SchoolClosureDataId = @SchoolClosureDataId
RETURN 0
