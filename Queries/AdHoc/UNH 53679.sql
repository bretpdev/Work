--run on NOCHOUSE
SELECT 
	[DocTyp]
	,[Unit]
	,[Status]
	,[ID]
	,[DocName]
FROM 
	[BSYS].[dbo].[LTDB_DAT_DocDetail]
WHERE
	DocTyp = 'Script'
	AND Unit = 'Loan Management'
	AND Status = 'Active'
