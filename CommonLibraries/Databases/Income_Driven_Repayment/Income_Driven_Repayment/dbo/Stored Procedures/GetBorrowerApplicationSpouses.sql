CREATE PROCEDURE [dbo].[GetBorrowerApplicationSpouses]
	@Ssn char(9)
AS

SELECT
	distinct s.ssn, RTRIM(s.first_name) FirstName, RTRIM(s.last_name) LastName, s.birth_date BirthDate
FROM
	Borrowers b
JOIN
	Loans l on l.borrower_id = b.borrower_id
JOIN
	Applications a on l.application_id = a.application_id
JOIN
	Spouses s on a.spouse_id = s.spouse_id
WHERE
	b.Ssn = @Ssn

RETURN 0