CREATE PROCEDURE [dbo].[SetInactive]
	@LoginId INT,
	@ScriptId VARCHAR(10),
	@Reason VARCHAR(150)
AS
	
DECLARE
	@UserName VARCHAR(128) = (SELECT UserName FROM [Login] WHERE LoginId = @LoginId)
	UPDATE
		[Login]
	SET
		Active = 0
	WHERE
		UserName = @UserName

	INSERT INTO InvalidLoginTracking(LoginId, ScriptId, Reason, InActivated)
	VALUES(@LoginId, @ScriptId, @Reason, 1)

RETURN 0