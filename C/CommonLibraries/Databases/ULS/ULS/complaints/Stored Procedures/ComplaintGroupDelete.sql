CREATE PROCEDURE [complaints].[ComplaintGroupDelete]
	@ComplaintGroupId int
AS

	update [complaints].ComplaintGroups
	   set DeletedOn = getdate(), DeletedBy = SYSTEM_USER
	 where ComplaintGroupId = @ComplaintGroupId


RETURN 0