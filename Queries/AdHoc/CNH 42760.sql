USE BSYS
GO

SELECT distinct
	r.Request,
	dd.ID,
	dd.DocName
FROM
	BSYS..LTDB_DAT_Requests r
	inner join Bsys..LTDB_DAT_DocDetail dd
		on dd.DocName = r.DocName
WHERE Requested >= 'XX/XX/XXXX'
AND Title LIKE '%fed%'
AND Title NOT LIKE '%Retire%'