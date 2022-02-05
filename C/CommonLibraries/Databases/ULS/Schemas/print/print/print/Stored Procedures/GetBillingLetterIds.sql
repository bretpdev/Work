CREATE PROCEDURE [print].[GetBillingLetterIds]

AS
	SELECT 
		Letter
	FROM
		Letters L 
		INNER JOIN ScriptData SD
			ON SD.LetterId = L.LetterId
		WHERE
			SD.ScriptID = 'BILLING'
RETURN 0
