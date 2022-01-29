CREATE PROCEDURE [webmanager].[SaveApiToken]
	@ApiTokenId INT = NULL,
	@GeneratedToken UNIQUEIDENTIFIER,
	@StartDate DATE,
	@EndDate DATE = NULL,
	@Notes VARCHAR(1000),
	@WindowsUsername VARCHAR(50),
	@TokenAccess webmanager.ControllerAccessType READONLY
AS
	BEGIN TRANSACTION

	IF (@ApiTokenId IS NULL)
	BEGIN
		INSERT INTO webapi.ApiTokens (GeneratedToken, StartDate, EndDate, Notes, AddedBy)
		VALUES (@GeneratedToken, @StartDate, @EndDate, @Notes, @WindowsUsername)

		SET @ApiTokenId = CAST(SCOPE_IDENTITY() AS INT)
	END

	UPDATE
		webapi.ApiTokens
	SET
		GeneratedToken = @GeneratedToken,
		StartDate = @StartDate,
		EndDate = @EndDate,
		Notes = @Notes
	WHERE
		ApiTokenId = @ApiTokenId

	UPDATE
		ATCA
	SET
		InactivatedAt = GETDATE(), InactivatedBy = @WindowsUsername
	FROM
		webapi.ApiTokenControllerActions ATCA
		LEFT JOIN @TokenAccess TA ON 
			TA.ControllerActionId = ATCA.ControllerActionId AND 
			ATCA.InactivatedAt IS NOT NULL
	WHERE
		ATCA.ApiTokenId = @ApiTokenId
		AND
		TA.ControllerActionId IS NULL

	INSERT INTO webapi.ApiTokenControllerActions (ApiTokenId, ControllerActionId, AddedBy)
	SELECT
		@ApiTokenId, TA.ControllerActionId, @WindowsUsername
	FROM
		@TokenAccess TA
		LEFT JOIN webapi.ApiTokenControllerActions ATCA ON 
			ATCA.ApiTokenId = @ApiTokenId AND
			ATCA.ControllerActionId = TA.ControllerActionId AND
			ATCA.InactivatedAt IS NULL
	WHERE
		ATCA.ApiTokenId IS NULL

	SELECT @ApiTokenID [ApiTokenId]

	IF @@ERROR = 0
		COMMIT TRANSACTION

RETURN 0
