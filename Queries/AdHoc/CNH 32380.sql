SELECT
	DD.SSN,
	MIN(DD.printed) [FirstSent],
	MAX(DD.printed) [LastSent],
	COUNT(DD.printed) [LettersSent]
FROM
	ECorrFed..DocumentDetails DD
	INNER JOIN ECorrFed..Letters L on L.LetterId = DD.LetterId
WHERE
	L.Letter = 'TSXXBIDRPS'
	AND
	CAST(DD.Printed AS DATE) BETWEEN 'XX/XX/XX' AND 'XX/XX/XX'
GROUP BY
	DD.SSN
HAVING
	COUNT(DD.Printed) > X