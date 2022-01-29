--hours per ticket
SELECT 
	CONCAT(SDU.FirstName, ' ', SDU.LastName) AS Name
	,TT.TicketID
	,CAST(SUM(CAST(DATEDIFF(SECOND, TT.StartTime, TT.EndTime) AS FLOAT) / 60 / 60) AS FLOAT) AS Hours
FROM 
	[Reporting].[dbo].[TimeTracking] TT
	INNER JOIN CSYS.[dbo].[SYSA_DAT_Users] SDU
		ON TT.SqlUserID = SDU.SqlUserId
WHERE
	TicketID IN (9538, 9582, 9447)
	AND Region = 'cornerstone'
GROUP BY
	CONCAT(SDU.FirstName, ' ', SDU.LastName)
	,TT.TicketID


--hours per staff
SELECT 
	CONCAT(SDU.FirstName, ' ', SDU.LastName) AS Name
	,CAST(SUM(CAST(DATEDIFF(SECOND, TT.StartTime, TT.EndTime) AS FLOAT) / 60 / 60) AS FLOAT) AS Hours
FROM 
	[Reporting].[dbo].[TimeTracking] TT
	INNER JOIN CSYS.[dbo].[SYSA_DAT_Users] SDU
		ON TT.SqlUserID = SDU.SqlUserId
WHERE
	TicketID IN (9538, 9582, 9447)
	AND Region = 'cornerstone'
GROUP BY
	CONCAT(SDU.FirstName, ' ', SDU.LastName)

