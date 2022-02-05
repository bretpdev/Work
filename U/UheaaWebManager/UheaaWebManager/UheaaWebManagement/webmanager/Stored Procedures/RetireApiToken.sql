CREATE PROCEDURE [webmanager].[RetireApiToken]
	@ApiTokenId INT,
	@WindowsUsername VARCHAR(50)
AS
	
	UPDATE
		webapi.ApiTokens
	SET
		InactivatedAt = GETDATE(),
		InactivatedBy = @WindowsUsername
	WHERE
		ApiTokenId = @ApiTokenId

RETURN 0
