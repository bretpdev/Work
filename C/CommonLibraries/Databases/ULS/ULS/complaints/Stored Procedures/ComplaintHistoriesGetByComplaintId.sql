CREATE PROCEDURE [complaints].[ComplaintHistoriesGetByComplaintId]
	@ComplaintId int
AS
	
	select ComplaintHistoryId, ComplaintId, HistoryDetail, AddedOn, AddedBy
	  from [complaints].[ComplaintHistory]
	 where ComplaintId = @ComplaintId

RETURN 0