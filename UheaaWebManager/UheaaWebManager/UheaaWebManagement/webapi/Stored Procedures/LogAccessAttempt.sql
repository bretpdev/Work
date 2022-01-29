CREATE PROCEDURE [webapi].[LogAccessAttempt]
	@RelativeUrl VARCHAR(2048),
	@ApiTokenId INT = NULL,
	@UserTokenId INT = NULL,
	@SuccessfulAccess BIT
AS
	
	INSERT INTO webapi.AccessLog (RelativeUrl, ApiTokenId, UserTokenId, SuccessfulAccess)
	VALUES (@RelativeUrl, @ApiTokenId, @UserTokenId, @SuccessfulAccess)

	IF @SuccessfulAccess = 1 AND @UserTokenId IS NOT NULL
	BEGIN
		UPDATE
			webapi.UserTokens
		SET
			TokenExpiresAt = DATEADD(minute, 15, GETDATE())
		WHERE
			UserTokenId = @UserTokenId
	END

RETURN 0
