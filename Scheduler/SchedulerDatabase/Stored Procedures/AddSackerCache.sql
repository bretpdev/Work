CREATE PROCEDURE [dbo].[AddSackerCache]
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

	INSERT INTO
		[SackerCache] ([RequestTypeId], [Name], [Id], [Status], [Priority], [Court], [AssignedProgrammer],	[AssignedTester], [DevEstimate], [TestEstimate])
	VALUES
		(@RequestTypeId, @Name, @Id, @Status, @Priority, @Court, @AssignedProgrammer, @AssignedTester, @DevEstimate, @TestEstimate)

RETURN 0
