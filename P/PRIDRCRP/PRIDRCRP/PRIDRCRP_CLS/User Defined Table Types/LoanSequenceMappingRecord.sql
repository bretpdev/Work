CREATE TYPE [pridrcrp].[LoanSequenceMappingRecord] AS TABLE
(
	Ssn VARCHAR(9) NOT NULL,
	DisbursementDate DATETIME NOT NULL,
	DisbursementAmount DECIMAL(14,2) NOT NULL,
	InterestRate NUMERIC(5,3) NOT NULL,
	GuaranteeDate DATETIME NOT NULL,
	LoanNum INT NOT NULL,
	LoanSequence INT NULL
)
