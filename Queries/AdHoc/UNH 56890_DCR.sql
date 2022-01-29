--run on UHEAASQLDB
USE BatchProcessing
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 70

	UPDATE BatchProcessing..Login SET Active = 1 WHERE LoginId != 1124 AND UserName != 'UT00240' AND Active = 0

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END

GO
ALTER PROCEDURE [dbo].[SetInactive]
	@LoginId INT,
	@ScriptId VARCHAR(10),
	@Reason VARCHAR(150)
AS
	
--DECLARE
--	@UserName VARCHAR(128) = (SELECT UserName FROM [Login] WHERE LoginId = @LoginId)
--	UPDATE
--		[Login]
--	SET
--		Active = 0
--	WHERE
--		UserName = @UserName

	INSERT INTO InvalidLoginTracking(LoginId, ScriptId, Reason, InActivated)
	VALUES(@LoginId, @ScriptId, @Reason, 1)

RETURN 0