SELECT DISTINCT 
	BF_SSN,
	LastName,
	FirstName, 
	LoanId,
	LoanProgramType,
	GuarantyDate,
	SaleDate,
	DealId,
	PostSale
FROM 
	CDW..CS_Transfer_EAXX  EAXX
WHERE 
	TransferNumber = X
	AND DealId = 'PSAON' -- LNC

SELECT DISTINCT 
	BF_SSN,
	LastName,
	FirstName, 
	LoanId,
	LoanProgramType,
	GuarantyDate,
	SaleDate,
	DealId,
	PostSale
FROM 
	CDW..CS_Transfer_EAXX 
WHERE 
	TransferNumber = X
	AND DealId = 'PSAOM'  --DLO