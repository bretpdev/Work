CREATE PROCEDURE [complaints].[FlagInsert]
	@FlagName NVARCHAR(50),
	@EnablesControlMailFields BIT
AS

	INSERT INTO [complaints].[Flags] (FlagName, EnablesControlMailFields)
	VALUES (@FlagName, @EnablesControlMailFields)

RETURN 0