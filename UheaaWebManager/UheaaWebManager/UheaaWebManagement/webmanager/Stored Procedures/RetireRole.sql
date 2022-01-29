CREATE PROCEDURE [webmanager].[RetireRole]
	@RoleId INT,
	@WindowsUsername VARCHAR(50)
AS
	
	UPDATE
		webapi.Roles
	SET
		InactivatedAt = GETDATE(),
		InactivatedBy = @WindowsUsername
	WHERE
		RoleId = @RoleId

RETURN 0
