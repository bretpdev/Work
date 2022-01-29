UPDATE 
	T1L
SET
	T1L.LoanSaleId = T1.DLO_SaleId
FROM
	CDW..CS_Transfer1 T1
	INNER JOIN CDW..CS_Transfer1Loans T1L
		ON T1L.BF_SSN = T1.BF_SSN
		AND T1L.LoanProgram = 'DLO'

UPDATE 
	T1L
SET
	T1L.LoanSaleId = T1.LNC_SaleId
FROM
	CDW..CS_Transfer1 T1
	INNER JOIN CDW..CS_Transfer1Loans T1L
		ON T1L.BF_SSN = T1.BF_SSN
		AND T1L.LoanProgram = 'LNC'
