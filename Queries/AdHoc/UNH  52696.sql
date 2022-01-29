/*
Query - 
Find all letters in LTS where Addressee = Endorser and Status = Active

Output in Excel - 
Document Name
Document Description
Letter ID
*/

SELECT
	DD.DocName [Document Name],
	REPLACE(REPLACE('"' + CAST(DD.[Description] AS VARCHAR(MAX)), CHAR(10), ' '), CHAR(13), ' ') + '"' [Document Description],
	DD.ID [Letter ID]
FROM	
	[dbo].[LTDB_DAT_DocDetail] DD
WHERE
	DD.Addressee = 'Endorser'
	AND
	DD.[Status] = 'Active'


