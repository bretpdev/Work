CREATE PROCEDURE [dbo].[GetSsnFromAppId]
	@AppId int
AS
	SELECT
		B.SSN
	FROM
		Loans L
		INNER JOIN Borrowers B
			ON B.borrower_id = L.borrower_id
		WHERE
			L.application_id = @AppId
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSsnFromAppId] TO [db_executor]
    AS [dbo];

