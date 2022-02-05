CREATE PROCEDURE [dbo].[LT_TS06BTSA_Transactions]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	WITH LN90_RT
	(
		DF_SPE_ACC_ID,
		LN_SEQ,
		LD_FAT_EFF,
		LN_FAT_SEQ,
		Label,
		LA_TRAN_TOT,
		LA_FAT_CUR_PRI,
		LA_FAT_NSI,
		LA_PRI_BAL
	)
	AS
	(
		SELECT
			LN90.DF_SPE_ACC_ID,
			LN90.LN_SEQ,
			LN90.LD_FAT_EFF,
			LN90.LN_FAT_SEQ,
			FMT.Label,
			LN90.LA_FAT_CUR_PRI+LA_FAT_NSI+LA_FAT_LTE_FEE AS LA_TRAN_TOT,
			LN90.LA_FAT_CUR_PRI,
			LN90.LA_FAT_NSI,
			(
				  SELECT
						SUM(PMTS.LA_FAT_CUR_PRI)
				  FROM
						LN90_FinancialHistory PMTS
				  WHERE
						CAST(PMTS.LD_FAT_EFF AS DATE) <= CAST(LN90.LD_FAT_EFF AS DATE)
						AND PMTS.LN_FAT_SEQ <= LN90.LN_FAT_SEQ
						AND PMTS.DF_SPE_ACC_ID = LN90.DF_SPE_ACC_ID
						AND PMTS.LN_SEQ = LN90.LN_SEQ
						AND PMTS.LC_FAT_REV_REA = '' -- reversed records
						AND PMTS.LC_STA_LON90 = 'A' -- active
			)
		FROM
			LN90_FinancialHistory LN90
			JOIN FormatTranslation FMT
				  ON LN90.PC_FAT_TYP+LN90.PC_FAT_SUB_TYP = FMT.Start
				  AND FMT.FmtName = '$FATRAN'
		WHERE 
			LN90.DF_SPE_ACC_ID = @AccountNumber
			AND LN90.LC_FAT_REV_REA = '' -- reversed records
			AND LN90.LC_STA_LON90 = 'A' -- active
	)

	SELECT 
		LN_SEQ AS [Loan Sequence],
		LD_FAT_EFF AS [Date],
		Label AS [Transaction Type],
		LA_TRAN_TOT AS [Total Amount of Transaction],
		LA_FAT_CUR_PRI AS [Amount to Principal],
		LA_FAT_CUR_PRI AS [Amount to Interest],
		LA_PRI_BAL AS [Principal Balance] 
	FROM 
		LN90_RT 
	ORDER BY
		DF_SPE_ACC_ID,
		LN_SEQ,
		CAST([LD_FAT_EFF] AS DATE),
		LN_FAT_SEQ
	;
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BTSA_Transactions])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BTSA_Transactions] TO [db_executor]
    AS [dbo];
