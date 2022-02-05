CREATE PROCEDURE [payhistlpp].[StartRun]
	@UserAccessId INT,
	@Tilp BIT
AS
	INSERT INTO ULS.payhistlpp.Run(UserAccessId, Tilp)
	VALUES(@UserAccessId, @Tilp)

	SELECT SCOPE_IDENTITY()