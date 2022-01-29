USE [Income_Driven_Repayment]
GO
/****** Object:  StoredProcedure [dbo].[GetBorrowerApplicationSpouses]    Script Date: X/XX/XXXX X:XX:XX PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetBorrowerApplicationSpouses]
	@Ssn char(X)
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
	AND S.SSN IS NOT NULL

RETURN X

