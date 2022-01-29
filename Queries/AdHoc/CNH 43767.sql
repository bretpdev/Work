SELECT
	ID,
	DocName,
	DocTyp,
	[Status]
FROM 
	BSYS..LTDB_DAT_DocDetail
WHERE
	UPPER(DocName) LIKE '%FED%'	
	AND [Status] IN ('Active','Development')
	



