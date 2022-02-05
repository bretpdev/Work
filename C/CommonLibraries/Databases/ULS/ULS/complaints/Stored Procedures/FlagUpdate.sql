CREATE PROCEDURE [complaints].[FlagUpdate]
	@FlagId int,
	@EnablesControlMailFields bit
AS

	update [complaints].[Flags]
	   set EnablesControlMailFields = @EnablesControlMailFields
	 where FlagId = @FlagId

RETURN 0