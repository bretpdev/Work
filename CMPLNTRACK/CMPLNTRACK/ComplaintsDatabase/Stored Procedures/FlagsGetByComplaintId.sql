CREATE PROCEDURE [complaints].[FlagsGetByComplaintId]
	@ComplaintId INT
AS

	SELECT DISTINCT
		F.FlagId,
		F.FlagName,
		F.EnablesControlMailFields
	FROM
		[complaints].Flags F
		INNER JOIN [complaints].ComplaintFlags CF
			ON F.FlagId = CF.FlagId
	WHERE
		CF.ComplaintId = @ComplaintId

RETURN 0