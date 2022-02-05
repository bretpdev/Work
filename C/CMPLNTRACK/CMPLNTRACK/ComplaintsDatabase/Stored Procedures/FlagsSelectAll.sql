CREATE PROCEDURE [complaints].[FlagsSelectAll]
AS

	SELECT
		FlagId,
		FlagName,
		EnablesControlMailFields
	FROM
		[complaints].Flags
	WHERE
		DeletedOn IS NULL

RETURN 0