SELECT
	ID,
	*
FROM
	[BSYS].[dbo].[LTDB_DAT_DocDetail]
WHERE
	[Status] = 'Active'
	AND
	DocName LIKE '%FED%'