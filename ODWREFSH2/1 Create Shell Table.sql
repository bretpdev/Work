USE ODW
GO

SELECT TOP 1
	*
INTO 
	dbo.DC11_LON_FAT
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				*
			FROM
				OLWHRM1.DC11_LON_FAT
		'
	);

SELECT TOP 1
	*
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				COUNT(*)
			FROM
				OLWHRM1.DC11_LON_FAT
		'
	); --5,317,429