SELECT 
	C.Title,
	R.ReasonText,
	CR.Comments,
	CR.LetterID,
	CASE WHEN CR.IsOutbound = 0 THEN 'Inbound' ELSE 'Outbound' END AS CallDirection,
	CR.RecordedBy,
	CR.RecordedOn
FROM
	MauiDUDE.calls.CallRecords CR
	INNER JOIN MauiDUDE.calls.Reasons R
		ON R.ReasonId = CR.ReasonId
	INNER JOIN MauiDUDE.calls.Categories C
		ON C.CategoryId = R.CategoryId
WHERE
	CR.IsCornerstone = 0
ORDER BY
	CR.LetterID DESC,
	CR.Comments DESC,
	CASE WHEN CR.IsOutbound = 0 THEN 'Inbound' ELSE 'Outbound' END,
	C.Title,
	R.ReasonText
