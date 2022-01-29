USE UDW
GO
--Issue:
--Dave S has asked for consolidation payoff information.  
--Can you do a quick query for all loans paid by consolidaiton since 12/1/2020?

--LN90.PC_FAT_TYP = 10
--LN90.PC_FAT_SUB_TYP = 70
--LN90.LD_FAT_EFF (effective date)
--LN90.LA_FAT_CUR_PRI (amount to principal)
--LN90.LA_FAT_NSI (amount to interest)

--Please provide a summary by month of the number of 10 70 payments 
--and the total dollar amount by month (sum of principal and interest).

SELECT
	LN90.BF_SSN,
	LN90.LN_SEQ,
	CAST(LN90.LD_FAT_EFF AS DATE) AS LD_FAT_EFF,
	ABS(ISNULL(LN90.LA_FAT_CUR_PRI,0.00)) AS AmountToPrincipal,
	ABS(ISNULL(LN90.LA_FAT_NSI,0.00)) AS AmountToInterest,
	ABS(ISNULL(LN90.LA_FAT_CUR_PRI,0.00)) + ABS(ISNULL(LN90.LA_FAT_NSI,0.00)) AS TotalAmount
FROM
	UDW..LN90_FIN_ATY LN90
WHERE
	LN90.PC_FAT_TYP = '10'
	AND LN90.PC_FAT_SUB_TYP = '70'
	AND CAST(LN90.LD_FAT_EFF AS DATE) >= '12/1/2020'


SELECT
	YEAR(LN90.LD_FAT_EFF) AS EffYear,
	MONTH(LN90.LD_FAT_EFF) AS EffMonth,
	SUM(ABS(ISNULL(LN90.LA_FAT_CUR_PRI,0.00))) AS AmountToPrincipal,
	SUM(ABS(ISNULL(LN90.LA_FAT_NSI,0.00))) AS AmountToInterest,
	SUM(ABS(ISNULL(LN90.LA_FAT_CUR_PRI,0.00)) + ABS(ISNULL(LN90.LA_FAT_NSI,0.00))) AS TotalAmount
FROM
	UDW..LN90_FIN_ATY LN90
WHERE
	LN90.PC_FAT_TYP = '10'
	AND LN90.PC_FAT_SUB_TYP = '70'
	AND CAST(LN90.LD_FAT_EFF AS DATE) >= '12/1/2020'
GROUP BY
	YEAR(LN90.LD_FAT_EFF),
	MONTH(LN90.LD_FAT_EFF)