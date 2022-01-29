CREATE PROCEDURE [dbo].[spMD_GetFinancialTransactions]
	@AccountNumber				VARCHAR(10)  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SELECT 
		LC_FAT_REV_REA as ReversalReason,
		CONVERT(VARCHAR, LD_FAT_EFF, 101) as EffectiveDate,
		ISNULL(CONVERT(VARCHAR, LD_FAT_PST, 101), '') as PostedDate,
		PC_FAT_TYP + PC_FAT_SUB_TYP as TransactionType,
		(ISNULL(LA_FAT_CUR_PRI,0.00)) as AppliedPrincipal,
		(ISNULL(LA_FAT_LTE_FEE, 0.00)) as AppliedLateFee,
		(ISNULL(LA_FAT_NSI,0.00)) as AppliedInterest,
		CAST(SUM((ISNULL(LA_FAT_CUR_PRI,0) + ISNULL(LA_FAT_NSI,0) + ISNULL(LA_FAT_LTE_FEE,0))) OVER  (PARTITION BY PD10.DF_SPE_ACC_ID, LN90.LD_FAT_EFF, LN90.LN_SEQ, LN90.LN_FAT_SEQ) AS DECIMAL (18,2)) AS TransactionAmount,
		CAST(LN_SEQ AS INT)  as LoanSeqNum
	FROM 
		LN90_FIN_ATY LN90
		INNER JOIN PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN90.BF_SSN
	WHERE 
		PD10.DF_SPE_ACC_ID = @AccountNumber
		AND LN90.PC_FAT_TYP IN ('05','10','14','15','17','26','50','51','55','60')
	ORDER BY 
		LD_FAT_EFF DESC,
		LN_SEQ
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetFinancialTransactions] TO [UHEAA\Imaging Users]
    AS [dbo];

