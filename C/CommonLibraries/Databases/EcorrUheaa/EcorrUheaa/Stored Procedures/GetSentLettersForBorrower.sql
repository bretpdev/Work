CREATE PROCEDURE [dbo].[GetSentLettersForBorrower]
	@AccountNumber VARCHAR(10)

AS
	SELECT 
		dd.DocumentDetailsId,
		L.Letter AS LetterId,
		DD.DocDate as SentAt
	FROM
		DocumentDetails DD
		INNER JOIN Letters L
			ON L.LetterId = DD.LetterId
	WHERE
		DD.ADDR_ACCT_NUM = @AccountNumber
		AND DD.Active = 1
RETURN 0
