
CREATE PROCEDURE [dbo].[LT_TS06BCAP_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		FMT.Label AS [Loan Program],
		LN10.LD_LON_1_DSB AS [First Disbursement Date],
		SUM(LN90.LA_FAT_CUR_PRI) AS [Amount Capitalized],
		LN10.LA_CUR_PRI AS [Current Principal Balance]
	FROM
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
		JOIN
			(
				SELECT
					DF_SPE_ACC_ID,
					LN_SEQ,
					MAX(CAST(LD_FAT_EFF AS DATE)) AS LD_FAT_EFF
				FROM
					LN90_FinancialHistory
				WHERE
					DF_SPE_ACC_ID = @AccountNumber
					AND 
					PC_FAT_TYP = '70'
					AND PC_FAT_SUB_TYP = '01'
					AND LC_STA_LON90 = 'A'
					AND LC_FAT_REV_REA = ''
				GROUP BY
					DF_SPE_ACC_ID,
					LN_SEQ
			) HIST
			ON LN10.DF_SPE_ACC_ID = HIST.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = HIST.LN_SEQ
		JOIN LN90_FinancialHistory LN90
			ON LN10.DF_SPE_ACC_ID = LN90.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN90.LN_SEQ
			AND HIST.LD_FAT_EFF = CAST(LN90.LD_FAT_EFF AS DATE)
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
	GROUP BY
		LN10.LD_LON_1_DSB,
		FMT.Label,
		LN10.LA_CUR_PRI
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BCAP_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BCAP_Loans] TO [db_executor]
    AS [dbo];
