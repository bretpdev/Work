CREATE PROCEDURE [complaints].[FlagInsert]
	@FlagName nvarchar(50),
	@EnablesControlMailFields bit
AS

	insert into [complaints].[Flags] (FlagName, EnablesControlMailFields)
	values (@FlagName, @EnablesControlMailFields)

RETURN 0