USE Reporting

UPDATE
	TimeTracking
SET
	TimeTracking.EndTime = CorrectEndTime.CorrectEndTime
FROM
	TimeTracking 
	INNER JOIN CorrectEndTime 
		ON TimeTracking.SqlUserID = CorrectEndTime.SqlUserID 
		AND TimeTracking.TicketID = CorrectEndTime.TicketId 
		AND TimeTracking.StartTime = CorrectEndTime.StartTime
