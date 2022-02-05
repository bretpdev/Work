CREATE PROCEDURE [complaints].[Search]
	@AccountNumber char(10) = NULL,
	@IncludeOpenComplaints bit,
	@IncludeClosedComplaints bit,
	@IncludeNoFlags bit,
	@Flags complaints.IntTable readonly,
	@Parties complaints.IntTable readonly,
	@Types complaints.IntTable readonly,
	@Groups complaints.IntTable readonly
AS
	

	select c.ComplaintId, c.AccountNumber, c.BorrowerName, cp.PartyName, ct.TypeName, cp.ComplaintPartyId, ct.ComplaintTypeId, c.ComplaintDescription, 
	       c.ResolutionComplaintHistoryId, c.ComplaintDate, c.ControlMailNumber, c.DaysToRespond, c.NeedHelpTicketNumber, c.ComplaintGroupId, cg.GroupName
	  from [complaints].Complaints c
      join [complaints].ComplaintParties cp on cp.ComplaintPartyId = c.ComplaintPartyId
	  join @Parties p on p.Id = c.ComplaintPartyId --filter out unspecified parties
      join [complaints].ComplaintTypes ct on ct.ComplaintTypeId = c.ComplaintTypeId
	  join @Types t on t.Id = c.ComplaintTypeId --filter out unspecified types
	  join [complaints].ComplaintGroups cg on cg.ComplaintGroupId = c.ComplaintGroupId
	  join @Groups g on g.Id = c.ComplaintGroupId
	 where 
	       ((@IncludeNoFlags = 1 and not exists(select * from [complaints].ComplaintFlags cf join @Flags f on f.Id = cf.FlagId where cf.ComplaintId = c.ComplaintId) ) 
		   or 
		   (@IncludeNoFlags = 0 and exists (select * from [complaints].ComplaintFlags cf join @Flags f on f.Id = cf.FlagId where cf.ComplaintId = c.ComplaintId)))
	   and ((@IncludeOpenComplaints = 1 and c.ResolutionComplaintHistoryId IS NULL) or (@IncludeClosedComplaints = 1 and c.ResolutionComplaintHistoryId IS NOT NULL))
	   and (@AccountNumber IS NULL or c.AccountNumber = @AccountNumber)

RETURN 0