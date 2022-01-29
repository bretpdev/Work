CREATE PROCEDURE [complaints].[ComplaintSetFlags]
	@ComplaintId int,
	@FlagIds complaints.IntTable readonly
AS
	
	delete 
	  from complaints.ComplaintFlags
	 where ComplaintId = @ComplaintId 
	   and FlagId not in (select Id from @FlagIds)

	insert into complaints.ComplaintFlags (ComplaintId, FlagId)
	select @ComplaintId, FlagId
	  from complaints.Flags f
	  join @FlagIds fi on fi.Id = f.FlagId
	 where FlagId not in (select FlagId from complaints.ComplaintFlags where ComplaintId = @ComplaintId)

RETURN 0