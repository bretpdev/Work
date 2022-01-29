CREATE PROCEDURE [dbo].[GetSsnFromAppId]
	@AppId INT
AS
	SELECT DISTINCT 
		B.SSN
	FROM
		Loans L
		INNER JOIN Borrowers B
			ON B.borrower_id = L.borrower_id
	WHERE
		L.application_id = @AppId
RETURN 0