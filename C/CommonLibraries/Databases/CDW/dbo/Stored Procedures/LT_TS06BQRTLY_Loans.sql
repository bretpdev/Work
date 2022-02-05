CREATE PROCEDURE [dbo].[LT_TS06BQRTLY_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		FMT.Label AS [Loan Program],
		'$' + CAST(LN10.LA_LON_AMT_GTR AS VARCHAR) AS [Total Principal Disbursed],
		'$' + CAST(LN10.LA_NSI_OTS AS VARCHAR) AS [Total Outstanding Interest],
		CAST(LN72.LR_ITR AS VARCHAR) + '%' AS [Interest Rate],
		'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR) AS [Total Balance]
	FROM
		LN10_LON LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
			AND LN10.LA_CUR_PRI > 0
		JOIN PD10_Borrower PD10
			ON LN10.BF_SSN = PD10.BF_SSN
		JOIN LN72_InterestRate LN72
			ON PD10.DF_SPE_ACC_ID = LN72.DF_SPE_ACC_ID
			AND LN72.LN_SEQ = LN10.LN_SEQ
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned.  ([dbo].[LT_TS06BQRTLY_Loans])',11,2)
	END