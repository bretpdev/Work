--Need to break up this pop into 31,000 record chunks and assign a LoanSaleId to each

--SELECT 
--	T1L.BF_SSN + RIGHT('0000' + CAST(T1L.LN_SEQ AS VARCHAR(2)),4) AS TriggerFile,
--	* 
--FROM 
--	CDW..CS_Transfer1Loans T1L 
--	INNER JOIN CDW..CS_Transfer1 T1 
--		ON T1.BF_SSN = T1L.BF_SSN
--ORDER BY
--	T1.BorrowerId,
--	T1L.LoanProgram

DECLARE @Repeat INT = (SELECT TOP 1 BorrowerId FROM CDW..CS_Transfer1 T1 WHERE T1.DLO_SaleId IS NULL OR T1.LNC_SaleId IS NULL)
DECLARE @SaleDLO INT = 1
DECLARE @SaleLNC INT = 100
WHILE(@Repeat IS NOT NULL)
BEGIN
	DECLARE @StartBorrower INT = (SELECT MIN(BorrowerId) FROM CDW..CS_Transfer1 T1 WHERE T1.DLO_SaleId IS NULL OR T1.LNC_SaleId IS NULL)

	;WITH LN_SALE AS
	(
		SELECT
			T1.BorrowerId,
			T1.BF_SSN,
			TotalLoanCount = T1.DLO_Count + T1.LNC_Count
		FROM 
			CDW..CS_Transfer1 T1
		WHERE
			T1.BorrowerId = @StartBorrower
			AND
			(
				T1.LNC_SaleId IS NULL
				OR T1.DLO_SaleId IS NULL
			)

		UNION ALL

		SELECT
			T1.BorrowerId,
			T1.BF_SSN,
			LS.TotalLoanCount + T1.DLO_Count + T1.LNC_Count
		FROM
			LN_SALE LS
			INNER JOIN CDW..CS_Transfer1 T1
				ON T1.BorrowerId = LS.BorrowerId + 1
		WHERE
			T1.LNC_SaleId IS NULL
			OR T1.DLO_SaleId IS NULL			
	)
	UPDATE
		T1
	SET
		T1.DLO_SaleId = @SaleDLO,
		T1.LNC_SaleId = @SaleLNC 
	FROM
		LN_SALE LS
		INNER JOIN CDW..CS_Transfer1 T1
			ON T1.BorrowerId = LS.BorrowerId
		INNER JOIN CDW..CS_Transfer1Loans T1L
			ON T1L.BF_SSN = T1.BF_SSN
	WHERE
		LS.TotalLoanCount <= 31500
	OPTION(maxrecursion 0)

	SET @SaleDLO = @SaleDLO + 1
	SET @SaleLNC = @SaleLNC + 1
	SET @Repeat = (SELECT TOP 1 BorrowerId FROM CDW..CS_Transfer1 T1 WHERE T1.DLO_SaleId IS NULL OR T1.LNC_SaleId IS NULL)
END