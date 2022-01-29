SELECT DISTINCT
	ID
	,DocName
	,DocTyp
	,[Status]
	--,[Description]
FROM 
	[BSYS].[dbo].[LTDB_DAT_DocDetail]
WHERE
	[Status] = 'Active'
	AND UPPER(DocName) NOT LIKE '%FED'