DECLARE @LastRunDate DATE
SET @LastRunDate = '2016-10-01'


--Still running (Open tracking records)
SELECT 
	TT.SqlUserID
	,TT.Region
	,TT.TicketID
	,CASE
		WHEN TT.Region = 'uheaa'
		THEN CONCAT('UNH ',TT.TicketID)
		WHEN TT.Region = 'cornerstone'
		THEN CONCAT('CNH ',TT.TicketID)
		ELSE ''
	END AS Ticket
	,TT.StartTime
	,TT.EndTime
    ,SDU.FirstName + ' ' + SDU.LastName AS Name
	,SDU.EMail
    ,DATEDIFF(MINUTE,TT.StartTime, GETDATE())/60 AS Hours
    ,(DATEDIFF(MINUTE,TT.StartTime, GETDATE())/60.0 - DATEDIFF(MINUTE,TT.StartTime, GETDATE())/60)*60 AS Minutes
FROM 
	[Reporting].[dbo].[TimeTracking] TT
	INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] SDU
		ON TT.SqlUserID = SDU.SqlUserId
WHERE
	TT.EndTime IS NULL
	AND CONVERT(DATE, TT.StartTime) < CONVERT(DATE,GETDATE())
	--AND SDU.EMail IN (@EMail)
ORDER BY
	TT.SqlUserID
	,TT.TicketID


--Long running
SELECT 
	TT.SqlUserID
	,TT.TicketID
	,CASE
		WHEN TT.Region = 'uheaa'
		THEN CONCAT('UNH ',TT.TicketID)
		WHEN TT.Region = 'cornerstone'
		THEN CONCAT('CNH ',TT.TicketID)
		ELSE ''
	END AS Ticket
	,TT.StartTime
	,TT.EndTime
    ,SDU.FirstName + ' ' + SDU.LastName AS Name
	,SDU.EMail
    ,DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60 AS Hours
    ,(DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60.0 - DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60)*60 AS Minutes
FROM 
	[Reporting].[dbo].[TimeTracking] TT
	INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] SDU
		ON TT.SqlUserID = SDU.SqlUserId
WHERE
	DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60 > 4
	AND CONVERT(DATE, TT.StartTime) > @LastRunDate
	--AND CONVERT(DATE, TT.StartTime) > '2016-10-01'
	--AND SDU.EMail IN (@EMail)
ORDER BY
	TT.SqlUserID
	,TT.StartTime
	

--Cumulative time per ticket
SELECT 
	TT.Region
	,TT.TicketID
	,SUM(DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/360.0) AS Hours
FROM 
	[Reporting].[dbo].[TimeTracking] TT
	INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] SDU
		ON TT.SqlUserID = SDU.SqlUserId
WHERE
	TT.EndTime IS NOT NULL
GROUP BY 
	TT.Region
	,TT.TicketID
ORDER BY
	TT.Region
	,TT.TicketID


--Overlapping NH hours:
;WITH CTE1 AS (
	SELECT 
		TT.SqlUserID
		,CONVERT(DATE,TT.StartTime) AS _Date
	FROM 
		[Reporting].[dbo].[TimeTracking] TT
		INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] SDU
			ON TT.SqlUserID = SDU.SqlUserId
	WHERE
		DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60 > 4
		AND CONVERT(DATE, TT.StartTime) > @LastRunDate
		--AND CONVERT(DATE, TT.StartTime) > '2016-10-01'
	GROUP BY
		TT.SqlUserID
		,TT.StartTime
),
CTE2 AS (
	SELECT 
		TT.SqlUserID
		,TT.TicketID
		,CASE
			WHEN TT.Region = 'uheaa'
			THEN CONCAT('UNH ',TT.TicketID)
			WHEN TT.Region = 'cornerstone'
			THEN CONCAT('CNH ',TT.TicketID)
			ELSE ''
		END AS Ticket
		,CONVERT(DATE,TT.StartTime) AS _Date
		,TT.StartTime
		,TT.EndTime
		,SDU.FirstName + ' ' + SDU.LastName AS Name
		,SDU.EMail
		,DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60 AS Hours
		,(DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60.0 - DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60)*60 AS Minutes
	FROM 
		[Reporting].[dbo].[TimeTracking] TT
		INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] SDU
			ON TT.SqlUserID = SDU.SqlUserId
	WHERE
		DATEDIFF(MINUTE,TT.StartTime, TT.EndTime)/60 > 4
		AND CONVERT(DATE, TT.StartTime) > @LastRunDate
		--AND CONVERT(DATE, TT.StartTime) > '2016-10-01'
)
SELECT
	CTE2.SqlUserID
	,CTE2.Name
	,CTE2.EMail
	,CTE2.Ticket
	,CTE2.StartTime
	,CTE2.EndTime
	,CTE2.Hours
	,CTE2.Minutes
	,COUNT(*) AS _Count
FROM 
	CTE1
	INNER JOIN CTE2
		ON CTE1.SqlUserID = CTE2.SqlUserID
		AND CTE1._Date = CTE2._Date
--WHERE
	--CTE2.EMail IN (@EMail)
GROUP BY 
	CTE2.SqlUserID
	,CTE2.Name
	,CTE2.EMail
	,CTE2.Ticket
	,CTE2.StartTime	
	,CTE2.EndTime
	,CTE2.Hours
	,CTE2.Minutes
HAVING 
	Count(CTE1._Date) > 1
ORDER BY 
	StartTime




/****************************************************************************************/
--DEPLOY TO QUARTERLY TIME TRACKING FOLDER, THEN MOVE TO SUPPORT SERVICES>KPI FOLDER
--daily NH ticket hours
--DECLARE @reportDate DATE = DATEADD(MONTH, -12, GETDATE())--comment out for SSRS
SELECT
	TT.SqlUserID
	,SDU.FirstName + ' ' +  SDU.LastName [Name]
	,ROUND(CAST(SUM(CAST(DATEDIFF(SECOND, TT.StartTime, TT.EndTime) AS FLOAT) / 60 / 60) AS FLOAT),6) [TimeSheetHours]
	,SUM(DATEDIFF(MINUTE, TT.StartTime, TT.EndTime))/60 [Hours]
	,SUM(DATEDIFF(MINUTE, TT.StartTime, TT.EndTime)) % 60 [Minutes]
FROM
	Reporting.dbo.TimeTracking TT
	INNER JOIN CSYS.dbo.SYSA_DAT_Users SDU
		ON TT.SqlUserID = SDU.SqlUserId
WHERE
	@reportDate = CAST(TT.EndTime AS DATE)
	AND SDU.WindowsUserName = REPLACE(@USER,'UHEAA\', '')--comment in for SSRS
GROUP BY
	TT.SqlUserID
	,SDU.FirstName
	,SDU.LastName
ORDER BY
	SDU.LastName



/****************************************************************************************/
--DEPLOY TO QUARTERLY TIME TRACKING FOLDER, THEN MOVE TO SUPPORT SERVICES>KPI FOLDER
--total NH ticket hours
--DECLARE @reportDate_begin DATE = DATEADD(WEEK, -1, GETDATE())--comment out for SSRS
--DECLARE @reportDate_end DATE = GETDATE()--comment out for SSRS
SELECT
	TT.SqlUserID
	,SDU.FirstName + ' ' +  SDU.LastName [Name]
	,ROUND(CAST(SUM(CAST(DATEDIFF(SECOND, TT.StartTime, TT.EndTime) AS FLOAT) / 60 / 60) AS FLOAT),6) [TimeSheetHours]
	,SUM(DATEDIFF(MINUTE, TT.StartTime, TT.EndTime))/60 [Hours]
	,SUM(DATEDIFF(MINUTE, TT.StartTime, TT.EndTime)) % 60 [Minutes]
FROM
	Reporting.dbo.TimeTracking TT
	INNER JOIN CSYS.dbo.SYSA_DAT_Users SDU
		ON TT.SqlUserID = SDU.SqlUserId
WHERE
	CAST(TT.EndTime AS DATE) BETWEEN @reportDate_begin AND @reportDate_end
	AND SDU.WindowsUserName = REPLACE(@USER,'UHEAA\', '')--comment in for SSRS
GROUP BY
	TT.SqlUserID
	,SDU.FirstName
	,SDU.LastName
ORDER BY
	SDU.LastName
