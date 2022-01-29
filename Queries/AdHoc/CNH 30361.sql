--a list of all federal leters in LTS
SELECT DISTINCT
	DocDetailId
	,DocName
	,DocID
	,ID
	,[Status]
FROM 
	[BSYS].[dbo].[LTDB_DAT_DocDetail]
WHERE
	UPPER(DOCNAME) LIKE UPPER('%FED%')

