DECLARE @DATA TABLE(SSN INT)
INSERT INTO @DATA VALUES
(517907511),
(530029108)










SELECT 
	DC02.BF_SSN,
	COUNT(*) AS LOAN_COUNT,
	SUM(ISNULL(LA_CLM_BAL,0.00) - ISNULL(LA_CLM_PRJ_COL_CST,0.00)) AS PRINCIPAL_INTEREST
FROM 
	@DATA D
	INNER JOIN ODW..DC02_BAL_INT DC02
		ON CAST(DC02.BF_SSN AS INT) = D.SSN
WHERE
	LA_CLM_BAL > 0.00
GROUP BY
	DC02.BF_SSN