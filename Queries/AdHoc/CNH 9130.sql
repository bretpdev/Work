-- XXX
SELECT
	*
FROM
	dbo.LTDB_DAT_DocDetail DD
WHERE	
	DD.DocTyp = 'Compass'
	AND
	DD.DocName NOT LIKE '%FED%'
	AND
	DD.[Status] IN ('Active', 'Development')
	
SELECT XXX * X
-- XXX



SELECT
	*
FROM
	dbo.LTDB_DAT_DocDetail DD
WHERE	
	DD.DocTyp = 'Manual'
	AND
	DD.DocName NOT LIKE '%FED%'
	AND
	DD.[Status] IN ('Active', 'Development')
	

SELECT XXX * X
	
SELECT
	*
FROM
	dbo.LTDB_DAT_DocDetail DD
WHERE	
	DD.DocTyp = 'Script'
	AND
	DD.DocName NOT LIKE '%FED%'
	AND
	DD.[Status] IN ('Active', 'Development')
	
	
	
	
SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				LNXX.BF_SSN
			FROM
				PKUB.LNXX_LON LNXX
			WHERE
				LNXX.LF_DOE_SCL_ORG = ''XXXXXXXX''
			GROUP BY
				LNXX.BF_SSN
			ORDER BY
				LNXX.BF_SSN
		'
	)