USE AuditCDW
GO

SELECT
	*
INTO 
	dbo.LNXX_LON_JanXXXX
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				*
			FROM
				PKUS.LNXX_LON
		'
	) 
