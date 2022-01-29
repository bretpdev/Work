CREATE PROCEDURE [complaints].[ComplaintGroupInsert]
	@ComplaintGroupName nvarchar(100)
AS

	insert into [complaints].[ComplaintGroups] (GroupName)
	values (@ComplaintGroupName)

RETURN 0