CREATE PROCEDURE [complaints].[FlagDelete]
	@FlagId INT
AS

	UPDATE
		[complaints].[Flags]
	SET
		DeletedBy = SYSTEM_USER,
		DeletedOn = GETDATE()
	WHERE
		FlagId = @FlagId

RETURN 0