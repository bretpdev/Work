create PROCEDURE [dbo].[LT_TS06BQRTLY_Loans]

	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @RowCount INT = 0
	

					SELECT DISTINCT
						ISNULL(FMT.Label,LN10.IC_LON_PGM) [Loan Program],
						'$' + CASE WHEN LN10.LA_LON_AMT_GTR = 0.00 THEN '0.00' ELSE CAST(LN10.LA_LON_AMT_GTR AS VARCHAR(15)) END AS "Total Principal Disbursed",
						'$' + CASE WHEN LN10.LA_NSI_OTS = 0.00 THEN '0.00' ELSE CAST(LN10.LA_NSI_OTS AS VARCHAR(15)) END AS "Total Outstanding Interest",
						CASE WHEN LN72.LR_ITR = 0.000 THEN '0.000' ELSE CAST(LN72.LR_ITR AS VARCHAR(7)) END + '%' AS "Interest Rate",
						'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR(15)) AS "Total Balance"
						
					FROM
						CDW..PD10_PRS_NME PD10
						INNER JOIN CDW..LN10_LON LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN CDW..LN72_INT_RTE_HST LN72 ON PD10.DF_PRS_ID = LN72.BF_SSN AND LN72.LN_SEQ = LN10.LN_SEQ
						LEFT JOIN FormatTranslation FMT ON LN10.IC_LON_PGM = FMT.Start
					WHERE
						LN10.LA_CUR_PRI > 0
						AND
						LN72.LC_STA_LON72 = 'A'
						AND 
						GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
						AND
						PD10.DF_SPE_ACC_ID =  @AccountNumber 

		SET @Rowcount = @@ROWCOUNT
	

	IF @Rowcount = 0 
	BEGIN
		RAISERROR('[dbo].[LT_TS06BQRTLY_Loans] returned no data for AccountNumber %s)',11,2, @AccountNumber)
	END

END