--Resolved Complaints
SELECT
	SUM(DATEDIFF(DAY,C.AddedOn,CH.AddedOn)) / COUNT(C.ComplaintId) AS AverageDays,
	MONTH(C.AddedOn) AS [Month],
	YEAR(C.AddedOn) AS [Year],
	COUNT(C.ComplaintId) AS [Number of Complaints]
FROM 
	CLS.complaints.Complaints C 
	LEFT JOIN CLS.complaints.ComplaintHistory CH 
		ON CH.ComplaintHistoryId = C.ResolutionComplaintHistoryId
WHERE
	CH.ComplaintHistoryId IS NOT NULL
	AND C.AddedOn >= 'XXXX-XX-XX'
GROUP BY
	MONTH(C.AddedOn),
	YEAR(C.AddedOn)
ORDER BY
	YEAR(C.AddedOn) DESC,
	MONTH(C.AddedOn) DESC

--Unresolved Complaints
SELECT
	SUM(DATEDIFF(DAY,C.AddedOn,GETDATE())) / COUNT(C.ComplaintId) AS AverageDays,
	MONTH(C.AddedOn) AS [Month],
	YEAR(C.AddedOn) AS [Year],
	COUNT(C.ComplaintId) AS [Number Of Complaints]
FROM 
	CLS.complaints.Complaints C 
	LEFT JOIN CLS.complaints.ComplaintHistory CH 
		ON CH.ComplaintHistoryId = C.ResolutionComplaintHistoryId
WHERE
	CH.ComplaintHistoryId IS NULL
	AND C.AddedOn >= 'XXXX-XX-XX'
GROUP BY
	MONTH(C.AddedOn),
	YEAR(C.AddedOn)
ORDER BY
	YEAR(C.AddedOn) DESC,
	MONTH(C.AddedOn) DESC