SELECT
	ID AS [LETTER ID],
	DocName,
	case DocTyp when 'Compass' then 'System Letter' else DocTyp end as [type]
FROM 
	BSYS..LTDB_DAT_DocDetail
WHERE
	[Status] IN ('Active','Development')
	AND
	(
		DocName LIKE '%FED%'
		OR
		DocName LIKE '%CORNERSTONE%'
		OR
		[Description] LIKE '%FED%'
		OR
		[Description] LIKE '%CORNERSTONE%'
	)
	AND ID IS NOT NULL