CREATE PROCEDURE [complaints].[FlagDelete]
	@FlagId int
AS

	update [complaints].[Flags]
	   set DeletedBy = SYSTEM_USER, DeletedOn = GETDATE()
	 where FlagId = @FlagId

RETURN 0