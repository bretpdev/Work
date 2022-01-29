CREATE PROCEDURE [dbo].[LT_TS06BTM2C_ConsolidationPayment]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		LN90.LD_FAT_EFF AS [date payment received],
		SUM((COALESCE(LN90.LA_FAT_PCL_FEE,0) + COALESCE(LN90.LA_FAT_NSI,0) + COALESCE(LN90.LA_FAT_LTE_FEE,0) + COALESCE(LN90.LA_FAT_ILG_PRI,0) + COALESCE(LN90.LA_FAT_CUR_PRI,0) + COALESCE(LN90.LA_FAT_NSI_ACR,0))) * -1 AS [consolidation payment amount]
	FROM 
		LN10_Loan LN10
		JOIN LN90_FinancialHistory LN90
			ON LN10.DF_SPE_ACC_ID = LN90.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN90.LN_SEQ
			AND LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '81'
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
	GROUP BY
		LN90.LD_FAT_EFF
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BTM2C_ConsolidationPayment])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BTM2C_ConsolidationPayment] TO [db_executor]
    AS [dbo];
