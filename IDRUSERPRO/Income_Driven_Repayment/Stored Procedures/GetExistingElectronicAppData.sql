CREATE PROCEDURE [dbo].[GetExistingElectronicAppData]
	@Eapp char(10)
AS
	SELECT DISTINCT
		account_number AS AccountNumber,
		RTRIM(first_name) AS FirstName,
		RTRIM(last_name) AS LastName,
		A.e_application_id AS EappId
	FROM
		[dbo].[Borrowers] B
		INNER JOIN Loans L
			ON L.borrower_id = B.borrower_id
		INNER JOIN Applications A
			ON A.application_id = L.application_id
			AND A.e_application_id = @Eapp
	WHERE
		A.e_application_id != ''
		
RETURN 0