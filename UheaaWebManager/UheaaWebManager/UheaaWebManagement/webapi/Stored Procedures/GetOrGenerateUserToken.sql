CREATE PROCEDURE [webapi].[GetOrGenerateUserToken]
	@AssociatedWindowsUsername VARCHAR(50)
AS

	DECLARE @GeneratedToken UNIQUEIDENTIFIER

	SELECT
		@GeneratedToken = UT.GeneratedToken
	FROM
		webapi.UserTokens UT
	WHERE
		AssociatedWindowsUsername = @AssociatedWindowsUsername
		AND
		GETDATE() BETWEEN UT.AddedAt AND UT.TokenExpiresAt

	IF @GeneratedToken IS NULL
	BEGIN
		INSERT INTO
			webapi.UserTokens (AssociatedWindowsUsername)
		VALUES
			(@AssociatedWindowsUsername)

		EXEC webapi.GetOrGenerateUserToken @AssociatedWindowsUsername
	END
	ELSE
	BEGIN
		UPDATE webapi.UserTokens
			SET TokenExpiresAt = DATEADD(minute, 15, GETDATE())
		WHERE
			GeneratedToken = @GeneratedToken
		SELECT @GeneratedToken
	END
RETURN 0
