CREATE PROCEDURE [rtnemlinvf].[SetInvalidatedAt]
	@InvalidEmailId INT
AS
	UPDATE
		InvalidEmail
	SET
		InvalidatedAt = GETDATE()
	WHERE
		InvalidEmailId = @InvalidEmailId
RETURN 0