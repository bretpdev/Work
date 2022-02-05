CREATE PROCEDURE InsertCreditBalance
	AS
	
DELETE FROM ServicerInventoryMetrics.dbo.CreditBalance

;WITH Transactions (DF_SPE_ACC_ID, LN_SEQ, LN_FAT_SEQ, LA_FAT_CUR_PRI, LD_FAT_APL)
AS
(
	SELECT
		FH.DF_SPE_ACC_ID, 
		FH.LN_SEQ, 
		FH.LN_FAT_SEQ, 
		FH.LA_FAT_CUR_PRI,
		FH.LD_FAT_APL
	FROM
		CDW.dbo.LN90_FinancialHistory FH
		JOIN (
				SELECT DISTINCT
					LN10.DF_SPE_ACC_ID
					,TOT.TOT_LA_CUR_PRI
				FROM
					CDW.dbo.LN10_Loan LN10
					JOIN (
							SELECT
								LN10.DF_SPE_ACC_ID
								,SUM(LN10.LA_CUR_PRI) AS TOT_LA_CUR_PRI
							FROM 
								CDW.dbo.LN10_Loan LN10
							GROUP BY LN10.DF_SPE_ACC_ID
							) TOT
						ON LN10.DF_SPE_ACC_ID = TOT.DF_SPE_ACC_ID
				WHERE
					TOT.TOT_LA_CUR_PRI < -4.99	
					) LN10
			ON FH.DF_SPE_ACC_ID = LN10.DF_SPE_ACC_ID
	WHERE
		FH.LC_FAT_REV_REA = '' -- not a reversal
		AND
		FH.LC_STA_LON90 = 'A' -- active record
)
INSERT INTO ServicerInventoryMetrics.dbo.CreditBalance
   (DF_SPE_ACC_ID, 
	LN_SEQ,
	RunningTotal,
	LD_FAT_APL,
	LN_FAT_SEQ)
SELECT
	RT.DF_SPE_ACC_ID, 
	RT.LN_SEQ,
	RT.RunningTotal,
	RT.LD_FAT_APL,
	MIN(RT.LN_FAT_SEQ) [min_LN_FAT_SEQ]
FROM
	( -- Running Total
		SELECT
			T.DF_SPE_ACC_ID, 
			T.LN_SEQ, 
			T.LN_FAT_SEQ,
			T.LD_FAT_APL,
			T.LA_FAT_CUR_PRI,
			RunningTotal = (SELECT SUM(LA_FAT_CUR_PRI) FROM Transactions WHERE DF_SPE_ACC_ID = T.DF_SPE_ACC_ID AND LN_SEQ = T.LN_SEQ AND LN_FAT_SEQ <= T.LN_FAT_SEQ)
		FROM
			Transactions T
	) AS RT
GROUP BY
	RT.DF_SPE_ACC_ID, 
	RT.LN_SEQ,
	RT.LD_FAT_APL,
	RT.RunningTotal
HAVING
	RT.RunningTotal < 0.00