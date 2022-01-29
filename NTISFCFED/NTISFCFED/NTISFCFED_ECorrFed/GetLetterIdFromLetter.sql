CREATE PROCEDURE [dbo].[GetLetterIdFromLetter]
	@Letter VARCHAR(250),
	@AccountNumber CHAR(10)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		l.Letter
	FROM
		DocumentDetails DD
		INNER JOIN Letters L
			ON L.LetterId = DD.LetterId
	WHERE 
		DD.[Path] LIKE '%' + @Letter + '%'
		AND DD.ADDR_ACCT_NUM = @AccountNumber
RETURN 0
