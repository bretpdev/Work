DECLARE @MinSaleDLO INT = (SELECT MIN(LoanSaleId) FROM CDW..CS_Transfer1Loans)
DECLARE @MinSaleNameDLO VARCHAR(20) = (SELECT LoanSale FROM CDW..CS_Transfer1_LoanSaleMapping WHERE LoanSaleId = @MinSaleDLO)
DECLARE @MinSaleLNC INT = (SELECT MIN(LoanSaleId) FROM CDW..CS_Transfer1Loans WHERE LoanSaleId >= 100)
DECLARE @MinSaleNameLNC VARCHAR(20) = (SELECT LoanSale FROM CDW..CS_Transfer1_LoanSaleMapping WHERE LoanSaleId = @MinSaleLNC)
DECLARE @Run INT = 1

DROP TABLE IF EXISTS #Output;
CREATE TABLE #Output(TriggerFile VARCHAR(40), Run INT, OrderBy INT)


WHILE @MinSaleDLO IS NOT NULL
BEGIN
	INSERT INTO #Output

	SELECT
		@MinSaleNameDLO AS TriggerFile,
		@Run,
		1

	UNION ALL

	SELECT
		T1L.BF_SSN + RIGHT('0000' + CAST(T1L.LN_SEQ AS VARCHAR(2)),4) AS TriggerFile,
		@Run,
		2
	FROM
		CDW..CS_Transfer1Loans T1L
	WHERE
		LoanSaleId = @MinSaleDLO

	UNION ALL

	SELECT
		@MinSaleNameLNC AS TriggerFile,
		@Run,
		3

	UNION ALL

	SELECT
		T1L.BF_SSN + RIGHT('0000' + CAST(T1L.LN_SEQ AS VARCHAR(2)),4) AS TriggerFile,
		@Run,
		4
	FROM
		CDW..CS_Transfer1Loans T1L
	WHERE
		LoanSaleId = @MinSaleLNC

	SET @MinSaleDLO = (SELECT MIN(LoanSaleId) FROM CDW..CS_Transfer1Loans WHERE LoanSaleId < 100 AND LoanSaleId > @MinSaleDLO)
	SET @MinSaleNameDLO = (SELECT LoanSale FROM CDW..CS_Transfer1_LoanSaleMapping WHERE LoanSaleId = @MinSaleDLO)
	SET @MinSaleLNC = (SELECT MIN(LoanSaleId) FROM CDW..CS_Transfer1Loans WHERE LoanSaleId > @MinSaleLNC)
	SET @MinSaleNameLNC = (SELECT LoanSale FROM CDW..CS_Transfer1_LoanSaleMapping WHERE LoanSaleId = @MinSaleLNC)
	
	SET @Run = @Run + 1
END

SELECT * FROM #Output WHERE TriggerFile IS NOT NULL  ORDER BY Run, OrderBy

