CREATE PROCEDURE [complaints].[ComplaintTypeDelete]
	@ComplaintTypeId int
AS

	update [complaints].ComplaintTypes
	   set DeletedOn = getdate(), DeletedBy = SYSTEM_USER
	 where ComplaintTypeId = @ComplaintTypeId


RETURN 0