CREATE PROCEDURE [dbo].[AddInvalidLogin]
	@LoginId INT,
	@ScriptId VARCHAR(10),
	@Reason VARCHAR(150)

AS

	INSERT INTO InvalidLoginTracking(LoginId, ScriptId, Reason, InActivated)
	VALUES(@LoginId, @ScriptId, @Reason, 0)
RETURN 0
