CREATE PROCEDURE [dbo].[FlagsGetByComplaintId]
	@ComplaintId int
AS

	select f.FlagId, f.FlagName, f.EnablesControlMailFields
	  from [complaints].Flags f
	  join [complaints].ComplaintFlags cf on f.FlagId = cf.FlagId
	 where cf.ComplaintId = @ComplaintId

RETURN 0