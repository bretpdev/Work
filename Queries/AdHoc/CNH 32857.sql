SELECT
	DD.DocName,
	DD.ID,
	DD.DocTyp,
	R.Request
FROM
	LTDB_DAT_DocDetail DD
	LEFT JOIN
	(
		SELECT 
			DocName,
			MAX(Request) [Request]
		FROM
			LTDB_DAT_Requests
		GROUP BY
			DocName
	) R ON DD.DocName = R.DocName
WHERE
	DD.[Status] = 'Active'
	AND
	ID IS NOT NULL

SELECT
	DD.DocName,
	DD.ID,
	DD.DocTyp,
	R.Request
FROM
	LTDB_DAT_DocDetail DD
	LEFT JOIN
	(
		SELECT 
			DocName,
			MAX(Request) [Request]
		FROM
			LTDB_DAT_Requests
		GROUP BY
			DocName
	) R ON DD.DocName = R.DocName
WHERE
	DD.[Status] = 'Active'
	AND
	ID IS NOT NULL
	AND
	(
		DD.DocName LIKE '%diligence%'
		OR
		DD.[Description] LIKE '%diligence%'
	)