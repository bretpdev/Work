CREATE PROCEDURE [dbo].[UpdateBatchPassword]
	@UserName varchar(10),
	@Password VARCHAR(128)
AS
	UPDATE
		[Login]
	SET
		EncrypedPassword = dbo.Encrypt(@Password),
		Active = 1,
		LastUpdated = GETDATE()
	WHERE
		UserName = @UserName
RETURN 0
