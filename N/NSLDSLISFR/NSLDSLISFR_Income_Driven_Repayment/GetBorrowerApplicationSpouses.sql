CREATE PROCEDURE [dbo].[GetBorrowerApplicationSpouses]
	@Ssn CHAR(9)
AS

SELECT DISTINCT 
	S.Ssn, 
	RTRIM(S.first_name) AS FirstName, 
	RTRIM(S.last_name) AS LastName, 
	S.birth_date AS BirthDate
FROM
	Borrowers B
	INNER JOIN Loans L 
		ON L.borrower_id = B.borrower_id
	INNER JOIN Applications A 
		ON L.application_id = A.application_id
	INNER JOIN Spouses S 
		ON A.spouse_id = S.spouse_id
WHERE
	B.Ssn = @Ssn
	AND S.SSN IS NOT NULL

RETURN 0
