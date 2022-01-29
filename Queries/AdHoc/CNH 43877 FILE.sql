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
	TransferNumber = XX
	AND DealId = 'PSAOS' -- LNC

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
	TransferNumber = XX
	AND DealId = 'PSAOR'  --DLO