SELECT 
	'"' + CONVERT(VARCHAR,NCH.ActivityDate,X) + ', ' + LEFT(CONVERT(VARCHAR,NCH.ActivityDate,X),X) + '"' AS Date,
	CASE WHEN COALESCE(NCH.CallLength,X) = X THEN ''
	     ELSE FORMAT(NCH.CallLength / XX, 'XX') + ':' + FORMAT(NCH.CallLength % XX, 'XX')
	END AS TotalHandleTime
FROM
	NobleCalls..NobleCallHistory NCH
WHERE
	NCH.RegionId = X --Cornerstone
	AND NCH.IsInbound = X --Outbound calls
	AND NCH.DeletedAt IS NULL
	AND CAST(NCH.ActivityDate AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
ORDER BY
	Date
