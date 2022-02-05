CREATE PROCEDURE [complaints].[ComplaintTypesSelectAll]
AS

	SELECT
		ComplaintTypeId,
		TypeName
	FROM
		[complaints].ComplaintTypes
	WHERE
		DeletedOn is null

RETURN 0