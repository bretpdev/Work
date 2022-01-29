DECLARE @DATA TABLE (BF_SSN INT, DATE_OF_ACTION_CODE VARCHAR(15))
INSERT INTO @DATA VALUES
('533115055','2/17/2021'),
('567199840','11/6/2020'),
('529456657','2/9/2021'),
('528478233','4/19/2021'),
('295901175','2/22/2021'),
('465896099','3/29/2021'),
('474809243','12/10/2020')


SELECT DISTINCT
	DC02.BF_SSN,
	D.DATE_OF_ACTION_CODE,
	COUNT(*) AS LOAN_COUNT,
	SUM(ISNULL(LA_CLM_BAL,0.00) - ISNULL(LA_CLM_PRJ_COL_CST,0.00)) AS PRINCIPAL_INTEREST
FROM
	@DATA D
	INNER JOIN ODW..DC02_BAL_INT DC02
		ON CAST(DC02.BF_SSN AS INT) = D.BF_SSN
GROUP BY
	DC02.BF_SSN,
	D.DATE_OF_ACTION_CODE