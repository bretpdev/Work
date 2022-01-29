DECLARE @accruedinterest TABLE(SSN CHAR(9), LNSEQ INT)
INSERT INTO @accruedinterest(SSN, LNSEQ)
VALUES('090529184',1),
('090529184',3),
('090685251',1),
('090748406',1),
('091761180',1),
('091761180',3),
('091761180',5),
('094646297',1),
('084541000',1),
('087828394',1),
('315947852',1),
('322821363',1),
('330643382',1),
('330767284',1)

SELECT CLM.BorrowerSsn, CLM.LN_SEQ, SUM(DCER.BorrowerAccruedInterest) AS InterestAmount, '2016-02-01' AS InterestAccruedDate
FROM EA27_BANA.dbo.CompassLoanMapping CLM 
INNER JOIN @accruedinterest AI 
	ON AI.SSN = CLM.BorrowerSsn
	AND AI.LNSEQ = CLM.LN_SEQ
INNER JOIN EA27_BANA.dbo._07_08DisbClaimEnrollRecord DCER
	ON DCER.BorrowerSSN = CLM.BorrowerSsn
	AND DCER.loan_number = CLM.loan_number
GROUP BY CLM.BorrowerSsn, CLM.LN_SEQ

