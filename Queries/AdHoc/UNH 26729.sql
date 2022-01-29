/*UHEAA only*/

USE BSYS
GO

SELECT DISTINCT 
	DocName
	,DocTyp
	,[Status]
FROM 
	dbo.LTDB_DAT_DocDetail DD
WHERE	
	DD.DocTyp = 'Compass'
	AND	DD.DocName NOT LIKE '%FED%'

UNION

SELECT DISTINCT 
	DocName
	,DocTyp
	,[Status]
FROM 
	dbo.LTDB_DAT_DocDetail DD
WHERE	
	DD.DocTyp = 'Manual'
	AND	DD.DocName NOT LIKE '%FED%'

UNION
	
SELECT DISTINCT 
	DocName
	,DocTyp
	,[Status]
FROM 
	dbo.LTDB_DAT_DocDetail DD
WHERE
	DD.DocTyp = 'Script'
	AND	DD.DocName NOT LIKE '%FED%'
ORDER BY 
	DocTyp
	,[Status]