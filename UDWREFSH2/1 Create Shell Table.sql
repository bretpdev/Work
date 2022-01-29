USE ODW

GO


SELECT TOP 1
	*
INTO 
	dbo.AY01_BR_ATY
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				*
			FROM
				OLWHRM1.AY01_BR_ATY
		'
	) 

SELECT TOP 1
	*

FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				count(*)
			FROM
				OLWHRM1.AY01_BR_ATY
		'
	) 