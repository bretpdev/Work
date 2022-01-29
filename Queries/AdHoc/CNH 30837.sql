SELECT 
	COUNT(CR.CallRecordId) AS NumberOfCalls,
	R.ReasonText
FROM 
	MauiDUDE.calls.CallRecords CR
	INNER JOIN MauiDUDE.calls.Reasons R
		ON R.ReasonId = CR.ReasonId
	INNER JOIN MauiDUDE.calls.Categories C
		ON C.CategoryId = R.CategoryId
WHERE
	C.Title = 'Call from a School'
	AND R.ReasonText IN('Borrower default aversion efforts','Borrower status request','Requesting school delinquency reports','Update borrower demographics')
	AND CR.RecordedOn BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	AND R.Cornerstone = X
GROUP BY
	R.ReasonText

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
	C.Title = 'Call from a School'
	AND R.ReasonText IN('Borrower default aversion efforts','Borrower status request','Requesting school delinquency reports','Update borrower demographics')
	AND CR.RecordedOn BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	AND R.Cornerstone = X
