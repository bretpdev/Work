CREATE PROCEDURE [schrpt].[RetireSchool]
	@SchoolId INT,
	@WindowsUserName VARCHAR(50)
AS

	UPDATE
		schrpt.Schools
	SET
		DeletedAt = GETDATE(), DeletedBy = @WindowsUserName
	WHERE
		SchoolId = @SchoolId


RETURN 0
