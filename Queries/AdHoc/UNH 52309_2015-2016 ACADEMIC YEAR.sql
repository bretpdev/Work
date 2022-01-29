--WE HAVE TO USE THE TIPL DATABASE FOR THIS SINCE THEY WANT IT ON ACEDMIC YEAR AND NOT WHEN THE LOAN WAS DISB
--COMPASS DOES NOT TRACK ACADEMIC YEAR.
SELECT
	B.FirstName + ' ' + B.LastName AS [BORROWER NAME],
	SUM(DisbAmount) AS [AMOUNT BORROWED]
FROM 
	[TLP].[dbo].[LoanDat] LOAN
	INNER JOIN TLP.dbo.BorrowerDat B
		ON B.SSN = LOAN.SSN
WHERE 
(
	(Term = 'Fall' and YEAR(TermBeginDate) = 2015) 
	or (Term = 'Spring' and YEAR(TermBeginDate) = 2016)
	or (Term = 'Summer' and YEAR(TermBeginDate) = 2016)
)
	AND LoanStatus = 'Disbursed'
GROUP BY
	B.FirstName + ' ' + B.LastName