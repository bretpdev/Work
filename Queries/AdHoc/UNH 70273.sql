USE ULS
GO

SELECT
	L.Letter,
	CASE WHEN PP.OnEcorr = 1 THEN 'E-CORR' ELSE 'PRINT' END DELIVERY_METHOD,
	COUNT(*) AS LETTER_COUNT
FROM
	ULS.[print].PrintProcessing PP
	INNER JOIN ULS.[print].ScriptData SD
		ON SD.ScriptDataId = PP.ScriptDataId
	INNER JOIN ULS.[print].Letters L
		ON L.LetterId = SD.LetterId
	
WHERE 
	SD.ScriptDataId IN (7,62,191,192,193,194,195,196)
	AND YEAR(PP.AddedAt) = 2020
GROUP BY
	L.Letter,
	CASE WHEN PP.OnEcorr = 1 THEN 'E-CORR' ELSE 'PRINT' END