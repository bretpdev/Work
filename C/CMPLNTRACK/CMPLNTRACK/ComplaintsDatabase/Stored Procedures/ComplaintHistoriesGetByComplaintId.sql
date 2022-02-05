CREATE PROCEDURE [complaints].[ComplaintHistoriesGetByComplaintId]
	@ComplaintId INT
AS
	
	SELECT
		ComplaintHistoryId,
		ComplaintId,
		HistoryDetail,
		AddedOn, 
		AddedBy
	FROM
		[complaints].[ComplaintHistory]
	WHERE
		ComplaintId = @ComplaintId

RETURN 0