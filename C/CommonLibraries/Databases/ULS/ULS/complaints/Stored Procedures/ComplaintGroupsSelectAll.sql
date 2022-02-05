CREATE PROCEDURE [complaints].[ComplaintGroupsSelectAll]
AS

	select ComplaintGroupId, GroupName
	  from [complaints].ComplaintGroups
	 where DeletedOn is null

RETURN 0