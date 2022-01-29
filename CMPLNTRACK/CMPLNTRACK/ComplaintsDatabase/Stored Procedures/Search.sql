CREATE PROCEDURE [complaints].[Search]
	@AccountNumber CHAR(10) = NULL,
	@IncludeOpenComplaints BIT,
	@IncludeClosedComplaints BIT,
	@FlagId INT = NULL,
	@PartyId INT = NULL,
	@TypeId INT = NULL,
	@GroupId INT = NULL
AS
	
	SELECT
		C.ComplaintId,
		C.AccountNumber,
		C.BorrowerName,
		CP.PartyName,
		CT.TypeName,
		CP.ComplaintPartyId,
		CT.ComplaintTypeId,
		C.ComplaintDescription, 
	    C.ResolutionComplaintHistoryId,
		CAST(C.ComplaintDate AS DATE) [ComplaintDate],
		C.ControlMailNumber,
		C.DaysToRespond,
		C.NeedHelpTicketNumber,
		C.ComplaintGroupId,
		CG.GroupName,
		C.AddedBy,
		CAST(C.AddedOn AS DATE) [AddedOn]
	FROM
		[complaints].Complaints C
		INNER JOIN [complaints].ComplaintParties CP 
			ON CP.ComplaintPartyId = C.ComplaintPartyId
		INNER JOIN [complaints].ComplaintTypes CT
			ON CT.ComplaintTypeId = C.ComplaintTypeId
		INNER JOIN [complaints].ComplaintGroups CG
			ON CG.ComplaintGroupId = C.ComplaintGroupId
	WHERE
		(
			(@FlagId IS NULL)
			OR
			EXISTS(SELECT * FROM [complaints].ComplaintFlags CF WHERE CF.ComplaintId = C.ComplaintId AND CF.FlagId = @FlagId)
		)
		AND ((@IncludeOpenComplaints = 1 AND C.ResolutionComplaintHistoryId IS NULL) OR (@IncludeClosedComplaints = 1 AND C.ResolutionComplaintHistoryId IS NOT NULL))
		AND (@PartyId IS NULL OR C.ComplaintPartyId = @PartyId)
		AND (@TypeID IS NULL OR C.ComplaintTypeId = @TypeID)
		AND (@GroupId IS NULL OR C.ComplaintGroupId = @GroupId)
		AND (@AccountNumber IS NULL OR C.AccountNumber = @AccountNumber)

RETURN 0