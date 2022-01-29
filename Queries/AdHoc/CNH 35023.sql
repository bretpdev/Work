-- if it's the first day of the month report on previous month
DECLARE	@ReportMonth VARCHAR(X) = MONTH(DATEADD(DAY, -X, GETDATE()))-X
DECLARE @ReportYear VARCHAR(X) = YEAR(DATEADD(DAY, -X, GETDATE()))
DECLARE	@BeginDate DATE = CAST(@ReportYear + '-' + @ReportMonth + '-X' AS DATE)
DECLARE	@EndDate DATE = DATEADD(DAY, -X, DATEADD(MONTH, +X, @BeginDate)) -- add a month subtract a day to get last day of month

select @ReportMonth, @ReportYear, @BeginDate, @EndDate

SELECT 
	CR.CallRecordId,
	CR.Comments,
	CR.RecordedOn,
	CR.RecordedBy,
	R.ReasonText,
	C.Title
FROM 
	MauiDUDE.calls.CallRecords CR
	INNER JOIN MauiDUDE.calls.Reasons R
		ON R.ReasonId = CR.ReasonId
	INNER JOIN MauiDUDE.calls.Categories C
		ON C.CategoryId = R.CategoryId
WHERE
	C.Title = 'school call'
	AND R.ReasonText IN('Default Aversion','School Reports','Status Requests','Transferred to Specialist','Updated Demos')
	AND CR.RecordedOn BETWEEN @BeginDate AND @EndDate
	AND R.Cornerstone = X

	select * from MauiDUDE.calls.Categories where title in('Call from a school','school call')
	select * from MauiDUDE.calls.Categories
	select * from MauiDUDE.calls.Reasons where Enabled = X and CategoryId in (X,XX)