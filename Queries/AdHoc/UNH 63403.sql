SELECT
	*
FROM
	[UDW].[dbo].[FB10_BR_FOR_REQ]
WHERE
	LC_FOR_TYP = '02'
	AND LD_FOR_REQ_BEG > CONVERT(DATE, '20190722')
	AND LD_FOR_REQ_BEG < CONVERT(DATE, '20190725')