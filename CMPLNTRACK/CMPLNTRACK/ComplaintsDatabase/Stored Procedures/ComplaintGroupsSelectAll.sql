CREATE PROCEDURE [complaints].[ComplaintGroupsSelectAll]
AS

	SELECT
		ComplaintGroupId,
		GroupName
	FROM
		[complaints].ComplaintGroups
	WHERE
		DeletedOn IS NULL

RETURN 0