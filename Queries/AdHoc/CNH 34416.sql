/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
	R.ReasonText as [Call Reason],
	MONTH(RecordedOn) as MON,
	YEAR(RecordedOn) AS YR,	
	COUNT(*) as [Call Count]
FROM 
	[MauiDUDE].[calls].[CallRecords] cr
inner join calls.Reasons r
	on r.ReasonId = cr.ReasonId
where 
	IsCornerstone = X 
	and cast(RecordedOn as date) between 'XX/XX/XXXX' and 'XX/XX/XXXX'
GROUP BY
	R.ReasonText,
	MONTH(RecordedOn),
	YEAR(RecordedOn) 
ORDER BY
	YEAR(RecordedOn),
	MONTH(RecordedOn),
	R.ReasonText