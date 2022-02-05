CREATE PROCEDURE [complaints].[FlagUpdate]
	@FlagId INT,
	@EnablesControlMailFields BIT
AS

	UPDATE
		[complaints].[Flags]
	SET
		EnablesControlMailFields = @EnablesControlMailFields
	WHERE
		FlagId = @FlagId

RETURN 0