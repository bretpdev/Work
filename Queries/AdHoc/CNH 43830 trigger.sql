DECLARE @MinSaleDLO INT = X
DECLARE @MinSaleNameDLO VARCHAR(XX) = (SELECT LoanSale FROM CDW..CS_TransferPIF_LoanSaleMapping WHERE LoanSaleId = @MinSaleDLO)
DECLARE @MinSaleLNC INT = XXX
DECLARE @MinSaleNameLNC VARCHAR(XX) = (SELECT LoanSale FROM CDW..CS_TransferPIF_LoanSaleMapping WHERE LoanSaleId = @MinSaleLNC)

DROP TABLE IF EXISTS #Output;
CREATE TABLE #Output(TriggerFile VARCHAR(XX), OrderBy INT)
INSERT INTO #Output

	SELECT
		@MinSaleNameDLO AS TriggerFile,
		X

	UNION ALL

	SELECT
		TXL.BF_SSN + RIGHT('XXXX' + CAST(TXL.LN_SEQ AS VARCHAR(X)),X) AS TriggerFile,
		X
	FROM
		CDW..CS_TransferPIFLoans TXL
	WHERE
		LoanSaleId = @MinSaleDLO

	UNION ALL

	SELECT
		@MinSaleNameLNC AS TriggerFile,
		X

	UNION ALL

	SELECT
		TXL.BF_SSN + RIGHT('XXXX' + CAST(TXL.LN_SEQ AS VARCHAR(X)),X) AS TriggerFile,
		X
	FROM
		CDW..CS_TransferPIFLoans TXL
	WHERE
		LoanSaleId = @MinSaleLNC

SELECT TriggerFile FROM #Output WHERE TriggerFile IS NOT NULL ORDER BY OrderBy