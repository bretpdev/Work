CREATE PROCEDURE [dbo].[spMD_GetFinancialTransactions]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT FAT_REV_REA as ReversalReason,
		LD_FAT_EFF as EffectiveDate,
		LD_FAT_PST as PostedDate,
		PC_FAT_TYP + PC_FAT_SUB_TYP as TransactionType,
		LA_FAT_CUR_PRI as AppliedPrincipal,
		LA_FAT_LTE_FEE as AppliedLateFee,
		LA_FAT_NSI as AppliedInterest,
		TRAN_AMT as TransactionAmount,
		LN_SEQ as LoanSeqNum
	FROM dbo.LN90_FinancialTran
	WHERE DF_SPE_ACC_ID = @AccountNumber
	ORDER BY CAST(LD_FAT_EFF as DateTime) DESC, LN_SEQ
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetFinancialTransactions] TO [Imaging Users]
    AS [dbo];

