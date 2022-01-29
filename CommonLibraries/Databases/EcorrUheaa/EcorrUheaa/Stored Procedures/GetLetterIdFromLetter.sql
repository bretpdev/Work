CREATE PROCEDURE [dbo].[GetLetterIdFromLetter]
	@Letter VARCHAR(250)
	
AS
	SELECT
		l.Letter
	FROM
		DocumentDetails DD
		INNER JOIN Letters L
			ON L.LetterId = DD.LetterId
	WHERE 
		DD.[Path] LIKE '%' + @Letter
RETURN 0
