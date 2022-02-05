CREATE PROCEDURE [complaints].[ComplaintGroupInsert]
	@ComplaintGroupName NVARCHAR(100)
AS

	INSERT INTO [complaints].[ComplaintGroups] (GroupName)
	VALUES (@ComplaintGroupName)

RETURN 0