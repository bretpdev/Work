CREATE PROCEDURE [activedirectory].[GetUserTokenId]
	@AssociatedWindowsUsername VARCHAR(50)
AS
	
	IF NOT EXISTS(SELECT * FROM webapi.UserTokens WHERE AssociatedWindowsUsername = @AssociatedWindowsUsername)
		INSERT INTO webapi.UserTokens (AssociatedWindowsUsername) VALUES (@AssociatedWindowsUsername)

	SELECT
		UserTokenId
	FROM
		webapi.UserTokens
	WHERE
		AssociatedWindowsUsername = @AssociatedWindowsUsername

RETURN 0
