﻿CREATE PROCEDURE [complaints].[FlagsGetByComplaintId]
	@ComplaintId int
AS

	select distinct f.FlagId, f.FlagName, f.EnablesControlMailFields
	  from [complaints].Flags f
	  join [complaints].ComplaintFlags cf on f.FlagId = cf.FlagId
	 where cf.ComplaintId = @ComplaintId

RETURN 0