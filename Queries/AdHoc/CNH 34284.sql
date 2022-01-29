SELECT 
	COUNT(CR.CallRecordId) AS AnnualNumberOfCalls,
	C.Title AS Category
FROM 
	MauiDUDE.calls.CallRecords CR 
	INNER JOIN MauiDUDE.calls.Reasons R 
		ON R.ReasonId = CR.ReasonId 
	INNER JOIN MauiDUDE.calls.Categories C 
		ON C.CategoryId = R.CategoryId 
WHERE 
	CR.IsCornerstone = X 
	AND CR.IsOutbound = X 
	AND CAST(RecordedOn AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
GROUP BY
	C.Title
ORDER BY
	AnnualNumberOfCalls
DESC
