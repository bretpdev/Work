CREATE PROCEDURE [activedirectorycache].[RemoveOldUsers]
	@ThresholdDate DATETIME
AS
	
	UPDATE
		activedirectorycache.Users
	SET
		DeletedAt = GETDATE(),
		UpdatedAt = GETDATE()
	WHERE
		UpdatedAt < @ThresholdDate
		AND
		DeletedAt IS NULL

RETURN 0
