CREATE PROCEDURE [dbo].[GetSoaLoanInfo]
	@AccountNumber char(10)
AS
	SELECT 
		ln10.LN_SEQ AS [Loan Seq],
		IC_LON_PGM as [Loan Program],
		LD_LON_1_DSB as [1st Disb Date],
		isnull(ABS(LN90.LA_FAT_CUR_PRI), cod.Balance) AS [Principl Balance at Transfer],
		ln10.LA_CUR_PRI as [Current Principal Balance]
	FROM	
		LN10_Loan LN10
	left JOIN
		(
			SELECT
				DF_SPE_ACC_ID,
				LN_SEQ,
				MIN(CAST(LD_FAT_EFF AS DATE)) AS MIN_LD_FAT_EFF
			FROM
				LN90_FinancialHistory
			WHERE
				PC_FAT_TYP = '02'
				AND LC_STA_LON90 = 'A'
				AND (LC_FAT_REV_REA = '' or LC_FAT_REV_REA is null)
			GROUP BY 
				DF_SPE_ACC_ID,
				LN_SEQ
		) MIN_EFF
			ON LN10.DF_SPE_ACC_ID = MIN_EFF.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = MIN_EFF.LN_SEQ
	left JOIN
		(
			SELECT
				DF_SPE_ACC_ID,
				LN_SEQ,
				MIN(CAST(LD_FAT_APL AS DATE)) AS MIN_LD_FAT_APL
			FROM
				LN90_FinancialHistory
			WHERE
				PC_FAT_TYP = '02'
				AND LC_STA_LON90 = 'A'
				AND (LC_FAT_REV_REA = '' or LC_FAT_REV_REA is null)
			GROUP BY 
				DF_SPE_ACC_ID,
				LN_SEQ
		) MIN_APL
			ON LN10.DF_SPE_ACC_ID = MIN_APL.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = MIN_APL.LN_SEQ
		left join 
		(
			SELECT
				DF_SPE_ACC_ID,
				LN_SEQ,
				sum(LA_FAT_CUR_PRI) AS Balance
			FROM
				LN90_FinancialHistory
			WHERE
				(PC_FAT_SUB_TYP = '01' and PC_FAT_TYP = '01')
				AND LC_STA_LON90 = 'A'
				AND (LC_FAT_REV_REA = '' or LC_FAT_REV_REA is null)
			GROUP BY 
				DF_SPE_ACC_ID,
				LN_SEQ
		) cod
			ON LN10.DF_SPE_ACC_ID = cod.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = cod.LN_SEQ
		left JOIN LN90_FinancialHistory LN90
			ON LN90.DF_SPE_ACC_ID = LN10.DF_SPE_ACC_ID
			AND LN90.LN_SEQ = LN10.LN_SEQ
			AND CAST(LN90.LD_FAT_EFF AS DATE) = MIN_EFF.MIN_LD_FAT_EFF
			AND CAST(LN90.LD_FAT_APL AS DATE) = MIN_APL.MIN_LD_FAT_APL
			
	WHERE 
		ln10.DF_SPE_ACC_ID = @AccountNumber
	ORDER BY LN10.LN_SEQ
RETURN 0
