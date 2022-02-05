CREATE PROCEDURE [dbo].[LT_TS06BSCRAA_Loans]
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
		LN10.LA_CUR_PRI AS [Current Principal Balance],
		CONVERT(VARCHAR, AD.BeginDate, 101) AS [SCRA Rate Begin Date],
		CASE WHEN ISNULL(AD.EndDate,'2099-12-31') = '2099-12-31' THEN 'To Be Determined'
		     ELSE CONVERT(VARCHAR, AD.EndDate, 101) END AS [SCRA Rate End Date]
	FROM
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
			AND LN10.LA_CUR_PRI > 0
		JOIN CLS.[scra].Borrowers B 
			ON LN10.DF_SPE_ACC_ID = B.BorrowerAccountNumber
		JOIN CLS.[scra].[ActiveDuty] AD 
			ON B.BorrowerId = AD.BorrowerId
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BSCRAA_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BSCRAA_Loans] TO [db_executor]
    AS [dbo];

