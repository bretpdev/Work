USE Reporting
GO

DECLARE 
	@UNIT varchar(2) = 30
	,@DATE1 DATETIME = '01/01/2016 00:00:01'
	,@DATE2 DATETIME = '03/20/2016 23:59:59'
	,@DATE3 DATETIME = '02/28/2016 00:00:01'
	,@DATE4 DATETIME = '03/05/2016 23:59:59';

--check of week 12 hours to match other query
SELECT
	SqlUserID,
	CAST(SUM(CAST(DATEDIFF(second, StartTime, EndTime) AS FLOAT) / 60 / 60) AS FLOAT) AS [Hours]
FROM
	TimeTracking
WHERE
	SqlUserID IN 
	(
		1276 --Bret
		,1280 --Jarom
		,1449 --Scott
		,1451 --Evan
		,1492 --Eric
		,1573 --Josh
		,1734 --JR
	 )
	AND StartTime > @DATE3 AND StartTime < @DATE4
GROUP BY SqlUserID
ORDER BY SqlUserID



--gets NOCHOUSE hours for Developers
;WITH CTE AS 
	(
	SELECT
		TT.SqlUserID
		,SYSA.LastName
		,FirstName
		,DATEPART(WEEK,StartTime) AS Week_No
		,CAST(SUM(CAST(DATEDIFF(second, StartTime, EndTime) AS FLOAT) / 60 / 60) AS FLOAT) AS Hours_JM16
	FROM
		Reporting.dbo.TimeTracking TT
		INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] SYSA
		 ON TT.SqlUserID = SYSA.SqlUserId
	WHERE

		TT.SqlUserID IN 
		(
			1276 --Bret
			,1280 --Jarom
			,1449 --Scott
			,1451 --Evan
			,1492 --Eric
			,1573 --Josh
			,1734 --JR
		)
		AND StartTime > @DATE1 AND StartTime < @DATE2
	GROUP BY 
		TT.SqlUserID 	
		,LastName
		,FirstName
		,StartTime
		,EndTime
	)
SELECT
	SqlUserID	
	,LastName
	,FirstName
	,Week_No
	,ROUND(SUM(Hours_JM16),2) AS Weekly_Hrs
FROM 
	CTE
GROUP BY
	SqlUserID	
	,LastName
	,FirstName
	,Week_No
