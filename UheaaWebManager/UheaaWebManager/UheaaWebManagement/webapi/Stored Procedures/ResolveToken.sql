CREATE PROCEDURE [webapi].[ResolveToken]
	@Token VARCHAR(36) = NULL
AS
	
	SELECT
		API.ApiTokenId [TokenId], 0 [IsUserToken]
	FROM
		webapi.ApiTokens API
	WHERE
		API.GeneratedToken = @Token

	SELECT
		UT.UserTokenId [TokenId], 1 [IsUserToken]
	FROM
		webapi.UserTokens UT
	WHERE
		UT.GeneratedToken = @Token

RETURN 0
