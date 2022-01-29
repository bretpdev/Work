CREATE PROCEDURE [dbo].[GetAllTimesForUser] 
	@SqlUserID varchar(50),
	@UnstoppedTime bit,
	@TicketId int = null
AS
	SELECT
		TT.TimeTrackingId,
		TT.TicketID,
		ST.SystemType,
		CC.CostCenter,
		TT.StartTime,
		TT.EndTime,
		TT.Region,
		TT.BatchProcessing,
		TT.GenericMeeting
	FROM
		Reporting.dbo.TimeTracking TT
		LEFT JOIN Reporting.dbo.SystemType ST ON TT.SystemTypeId = ST.SystemTypeId
		LEFT JOIN Reporting.dbo.CostCenter CC ON TT.CostCenterId = CC.CostCenterId
	WHERE
		((@SqlUserID > 0 AND TT.SqlUserID = @SqlUserID) OR @SqlUserID = 0)
		AND (TT.EndTime IS NULL OR @UnstoppedTime = 0)
		AND (@TicketId IS NULL OR TT.TicketID = @TicketId)