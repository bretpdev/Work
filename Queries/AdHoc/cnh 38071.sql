SELECT
	ID,
	DocName
FROM
	BSYS..LTDB_DAT_DocDetail
WHERE 
	ID LIKE 'TS%' --All Fed System letters start with TS
	AND [Status] IN ('Active','Development')