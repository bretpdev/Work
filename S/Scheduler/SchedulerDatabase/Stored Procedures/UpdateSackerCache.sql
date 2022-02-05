CREATE PROCEDURE [dbo].[UpdateSackerCache]
	@SackerCacheId INT,
    @RequestTypeId INT, 
    @Name VARCHAR(100) = NULL, 
	@Id INT,
    @Status VARCHAR(100), 
    @Priority TINYINT = NULL, 
	@Court VARCHAR(50) = NULL,
	@AssignedProgrammer VARCHAR(50) = NULL,
	@AssignedTester VARCHAR(50) = NULL,
    @DevEstimate DECIMAL(6, 2) = NULL, 
    @TestEstimate DECIMAL(6, 2) = NULL
AS
	
	UPDATE
		SackerCache
	SET
		RequestTypeId = @RequestTypeId,
		Name = @Name,
		Id = @Id,
		[Status] = @Status,
		[Priority] = @Priority,
		Court = @Court,
		AssignedProgrammer = @AssignedProgrammer,
		AssignedTester = @AssignedTester,
		DevEstimate = @DevEstimate,
		TestEstimate = @TestEstimate
	WHERE
		SackerCacheId = @SackerCacheId

RETURN 0
