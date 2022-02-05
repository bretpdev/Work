CREATE PROCEDURE [webmanager].[SaveRole]
	@RoleId INT = NULL,
	@ActiveDirectoryRoleName VARCHAR(50),
	@Notes VARCHAR(1000) = NULL,
	@WindowsUsername VARCHAR(50),
	@RoleAccess webmanager.ControllerAccessType READONLY
AS
	BEGIN TRANSACTION

	IF (@RoleId IS NULL)
	BEGIN
		INSERT INTO webapi.Roles (ActiveDirectoryRoleName, Notes, AddedBy)
		VALUES (@ActiveDirectoryRoleName, @Notes, @WindowsUsername)

		SET @RoleId = CAST(SCOPE_IDENTITY() AS INT)
	END

	UPDATE
		webapi.Roles
	SET
		ActiveDirectoryRoleName = @ActiveDirectoryRoleName,
		Notes = @Notes
	WHERE
		RoleId = @RoleId

	UPDATE
		RCA
	SET
		InactivatedAt = GETDATE(), InactivatedBy = @WindowsUsername
	FROM
		webapi.RoleControllerActions RCA
		LEFT JOIN @RoleAccess RA ON 
			RA.ControllerActionId = RCA.ControllerActionId AND 
			RCA.InactivatedAt IS NOT NULL
	WHERE
		RCA.RoleId = @RoleId
		AND
		RA.ControllerActionId IS NULL

	INSERT INTO webapi.RoleControllerActions (RoleId, ControllerActionId, AddedBy)
	SELECT
		@RoleId, RA.ControllerActionId, @WindowsUsername
	FROM
		@RoleAccess RA
		LEFT JOIN webapi.RoleControllerActions RCA ON 
			RCA.RoleId = @RoleId AND
			RCA.ControllerActionId = RA.ControllerActionId AND
			RCA.InactivatedAt IS NULL
	WHERE
		RCA.RoleId IS NULL

	SELECT @RoleId [RoleId]

	IF @@ERROR = 0
		COMMIT TRANSACTION

RETURN 0
