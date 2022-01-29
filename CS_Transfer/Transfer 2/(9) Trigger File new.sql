DECLARE @MinSaleDLO INT = 1
DECLARE @MinSaleNameDLO VARCHAR(20) = (SELECT LoanSale FROM CDW..CS_Transfer2_LoanSaleMapping WHERE LoanSaleId = @MinSaleDLO)
DECLARE @MinSaleLNC INT = 100
DECLARE @MinSaleNameLNC VARCHAR(20) = (SELECT LoanSale FROM CDW..CS_Transfer2_LoanSaleMapping WHERE LoanSaleId = @MinSaleLNC)

DROP TABLE IF EXISTS #Output;
CREATE TABLE #Output(TriggerFile VARCHAR(40), OrderBy INT)
INSERT INTO #Output

	SELECT
		@MinSaleNameDLO AS TriggerFile,
		1

	UNION ALL

	SELECT
		T1L.BF_SSN + RIGHT('0000' + CAST(T1L.LN_SEQ AS VARCHAR(2)),4) AS TriggerFile,
		2
	FROM
		CDW..CS_Transfer2Loans T1L
	WHERE
		LoanSaleId = @MinSaleDLO

	UNION ALL

	SELECT
		@MinSaleNameLNC AS TriggerFile,
		3

	UNION ALL

	SELECT
		T1L.BF_SSN + RIGHT('0000' + CAST(T1L.LN_SEQ AS VARCHAR(2)),4) AS TriggerFile,
		4
	FROM
		CDW..CS_Transfer2Loans T1L
	WHERE
		LoanSaleId = @MinSaleLNC

SELECT TriggerFile FROM #Output WHERE TriggerFile IS NOT NULL ORDER BY OrderBy