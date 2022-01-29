/*  TEST DATA */
	--DECLARE @reportDate DATETIME = GETDATE()
/* END TEST DATA */
DECLARE	@MonthYears TABLE (MonthNumber INT, YearNumber INT,	CalendarDate DATETIME, [MonthName] VARCHAR(XX),	MonthName_Year VARCHAR(XX))

DECLARE @Years TABLE ([YearNumber] smallint)
INSERT INTO @Years(YearNumber)
	SELECT YEAR(@reportDate)

;WITH Months(MonthNumber) AS
(
    SELECT X

    UNION ALL

    SELECT 
		MonthNumber+X 
    FROM 
		months
    WHERE 
		MonthNumber < XX
)
INSERT INTO @MonthYears(MonthNumber, YearNumber,CalendarDate, [MonthName], MonthName_Year)
	SELECT
		M.MonthNumber,
		Y.YearNumber,
		CAST(CAST(Y.YearNumber AS CHAR(X)) + '-' + CAST(MonthNumber AS VARCHAR(X)) + '-X' AS DATE) [CalendarDate],
		DATENAME(MONTH, CAST(CAST(MonthNumber AS VARCHAR(X)) + '-X-XXXX' AS DATE)) [MonthName],
		DATENAME(MONTH, CAST(CAST(MonthNumber AS VARCHAR(X)) + '-X-XXXX' AS DATE)) + ' ' + CAST(Y.YearNumber AS CHAR(X)) [MonthName_Year]
	FROM 
		Months M
		CROSS JOIN @Years Y
	WHERE
		CAST(CAST(YearNumber AS CHAR(X)) + '-' + CAST(MonthNumber AS VARCHAR(X)) + '-X' AS DATE) < CAST(GETDATE() AS DATE)
	ORDER BY
		Y.YearNumber DESC,
		M.MonthNumber DESC

--SELECT * FROM @MonthYears
--Get entry for all dates for the year by complaint group
DECLARE @Complaints TABLE(ComplaintGroupId INT, CalendarMonth INT, CalendarYear INT, TotalComplaints INT DEFAULT(X), Ombudsman INT DEFAULT(X), ControlMail INT DEFAULT(X), CumulativeTotal INT, CumulativeOmbudsman INT, CumulativeControlMail INT)
INSERT INTO @Complaints(ComplaintGroupId, CalendarMonth, CalendarYear)
	SELECT DISTINCT
		CG.ComplaintGroupId,
		M.MonthNumber,
		M.YearNumber
	FROM
		CLS.complaints.ComplaintGroups CG
		CROSS JOIN @MonthYears M
	WHERE
		CG.DeletedOn IS NULL

--SELECT * FROM @Complaints

UPDATE 
	T
SET
	T.TotalComplaints = MonthlyComplaints.TotalComplaints,
	T.Ombudsman = MonthlyComplaints.Ombudsman,
	T.ControlMail = MonthlyComplaints.ControlMail
FROM
	@Complaints T 
	INNER JOIN 
	(
		SELECT	
			SUM(CASE WHEN F.FlagName IN ('Ombudsman','Control Mail') THEN X ELSE X END) AS TotalComplaints,
			MONTH(C.ComplaintDate) AS CalendarMonth,
			YEAR(C.ComplaintDate) AS CalendarYear,
			SUM(CASE WHEN F.FlagName = 'Ombudsman' THEN X ELSE X END) AS Ombudsman,
			SUM(CASE WHEN F.FlagName = 'Control Mail' THEN X ELSE X END) AS ControlMail,
			C.ComplaintGroupId
		FROM
			CLS.complaints.Complaints C
			LEFT JOIN CLS.complaints.ComplaintFlags CF 
				ON CF.ComplaintId = C.ComplaintId
			LEFT JOIN CLS.complaints.Flags F 
				ON F.FlagId = CF.FlagId 
				AND F.DeletedOn IS NULL
		WHERE
			C.ComplaintDate BETWEEN DATEADD(yy, DATEDIFF(yy,X,@reportDate), X) AND DATEADD(s,-X,DATEADD(mm, DATEDIFF(m,X,@reportDate)+X,X))
		GROUP BY
			MONTH(C.ComplaintDate),
			YEAR(C.ComplaintDate),
			C.ComplaintGroupId
	) MonthlyComplaints 
		ON MonthlyComplaints.ComplaintGroupId = T.ComplaintGroupId 
		AND MonthlyComplaints.CalendarMonth = T.CalendarMonth 
		AND MonthlyComplaints.CalendarYear = T.CalendarYear

--SELECT * FROM @Complaints

UPDATE
	T
SET 
	T.CumulativeTotal = C.CumulativeTotal,
	T.CumulativeOmbudsman = C.CumulativeOmbudsman,
	T.CumulativeControlMail = C.CumulativeControlMail
FROM 
	@Complaints T 
	INNER JOIN
	(
		SELECT
			C.CalendarMonth,
			C.CalendarYear,
			C.ComplaintGroupId,
			SUM(CX.TotalComplaints) [CumulativeTotal],
			SUM(CX.Ombudsman) [CumulativeOmbudsman],
			SUM(CX.ControlMail) [CumulativeControlMail]
		FROM
			@Complaints C
			INNER JOIN @Complaints CX 
				ON CX.CalendarMonth <= C.CalendarMonth 
				AND CX.CalendarYear <= C.CalendarYear 
				AND CX.ComplaintGroupId = C.ComplaintGroupId
		GROUP BY
			C.CalendarMonth,
			C.CalendarYear,
			C.ComplaintGroupId
	) C 
		ON C.CalendarMonth = T.CalendarMonth 
		AND C.CalendarYear = T.CalendarYear 
		AND C.ComplaintGroupId = T.ComplaintGroupId

--SELECT * FROM @Complaints
DECLARE @TotalServiced INT
SET @TotalServiced =
(
	SELECT 
		COUNT(DISTINCT LNXX.BF_SSN)
	FROM 
		CDW.dbo.LNXX_LON LNXX
	WHERE 
		CONVERT(DATETIME, LNXX.LD_PIF_RPT, XXX) BETWEEN DATEADD(yy, DATEDIFF(yy,X,@reportDate), X) AND DATEADD(s,-X,DATEADD(mm, DATEDIFF(m,X,@reportDate)+X,X)) --make sure it happened within the year
		OR ISNULL(LNXX.LD_PIF_RPT,'') = ''
)

SELECT 
	CG.ComplaintGroupId AS REF,
	Y.MonthName_Year,
	CG.GroupName, 
	C.TotalComplaints,
	C.Ombudsman,
	C.ControlMail,
	C.CumulativeTotal,
	C.CumulativeOmbudsman,
	C.CumulativeControlMail,
	@TotalServiced AS TotalServiced
FROM 
	@MonthYears Y
	LEFT JOIN @Complaints C 
		ON Y.MonthNumber = C.CalendarMonth 
		AND Y.YearNumber = C.CalendarYear
	LEFT JOIN CLS.complaints.ComplaintGroups CG	
		ON CG.ComplaintGroupId = C.ComplaintGroupId
WHERE
	C.CalendarMonth <= MONTH(@reportDate)
	AND	C.CalendarYear = YEAR(@reportDate)
ORDER BY 
	Y.YearNumber DESC,
	Y.MonthNumber DESC