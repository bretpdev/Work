CREATE PROCEDURE [schrpt].[UpdateSchool]
	@SchoolId INT,
	@Name VARCHAR(50),
	@SchoolCode CHAR(6),
	@BranchCode CHAR(6)
AS

	UPDATE s
	SET
		s.Name = @Name, s.SchoolCode = @SchoolCode, s.BranchCode = @BranchCode
	FROM
		schrpt.Schools s
	WHERE
		s.SchoolId = @SchoolId

RETURN 0
