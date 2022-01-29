SELECT
	DISTINCT DD.ID
FROM
	[BSYS].[dbo].[LTDB_DAT_DocDetail] DD
	LEFT JOIN BSYS..LTDB_REF_KeyWords K
		ON DD.DocName = K.DocName
WHERE
	DD.DocName LIKE '%fed%'
	AND
	DD.[Status] = 'Active'
	AND
	(
		DD.DocName LIKE '%refund%'
		OR
		DD.Description LIKE '%refund%'
		OR
		K.KeyWord LIKE '%refund%'
	)