CREATE PROCEDURE [rtnemlinvf].[SetDeletedAt]
	@InvalidEmailId INT
AS
	UPDATE
		rtnemlinvf.InvalidEmail
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_SNAME()
	WHERE
		InvalidEmailId = @InvalidEmailId
RETURN 0