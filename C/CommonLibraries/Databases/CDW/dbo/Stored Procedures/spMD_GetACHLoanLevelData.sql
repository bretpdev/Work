CREATE PROCEDURE [dbo].[spMD_GetACHLoanLevelData]
	@AccountNumber				VARCHAR(10),
	@OnACHIndicator				BIT
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LN10.LN_SEQ as LoanSequenceNumber,
		LN10.LD_LON_1_DSB as FirstDisbursementDate,
		LN10.IC_LON_PGM as LoanType
	FROM dbo.LN10_Loan LN10
		LEFT OUTER JOIN dbo.LN83_EFTStatus LN83
			ON LN10.DF_SPE_ACC_ID = LN83.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN83.LN_SEQ
	WHERE LN10.DF_SPE_ACC_ID = @AccountNumber
		AND (
				(@OnACHIndicator = 1 AND LC_STA_LN83 = 'A') 
				OR 
				(@OnACHIndicator = 0 AND LC_STA_LN83 <> 'A')
			)
		AND LA_CUR_PRI > 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetACHLoanLevelData] TO [Imaging Users]
    AS [dbo];

