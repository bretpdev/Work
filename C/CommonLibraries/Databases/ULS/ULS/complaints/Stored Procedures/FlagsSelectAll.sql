CREATE PROCEDURE [complaints].[FlagsSelectAll]
AS

	select FlagId, FlagName, EnablesControlMailFields
	  from [complaints].Flags
	 where DeletedOn is null

RETURN 0