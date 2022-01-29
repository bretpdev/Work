CREATE PROCEDURE [schrpt].[AddSchool]
	@Name VARCHAR(50),
	@SchoolCode CHAR(6),
	@BranchCode CHAR(6),
	@WindowsUserName VARCHAR(50)
AS

	INSERT INTO schrpt.Schools (Name, SchoolCode, BranchCode, AddedBy)
	VALUES (@Name, @SchoolCode, @BranchCode, @WindowsUserName)

	SELECT CAST(SCOPE_IDENTITY() AS INT)

RETURN 0
