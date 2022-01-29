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
	AND DealId = 'PSAOJ' -- LNC

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
	AND DealId = 'PSAOI'  --DLO