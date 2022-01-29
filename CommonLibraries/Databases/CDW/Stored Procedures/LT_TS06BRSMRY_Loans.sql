CREATE PROCEDURE [dbo].[LT_TS06BRSMRY_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		LN10.LD_LON_1_DSB AS [First Disbursement Date],
		FMT.Label AS [Loan Program],
		COALESCE(BILL.LA_INT_PD_LST_STM, 0) AS [Interest Accrued Since Last Statement],
		DW01.WA_TOT_BRI_OTS AS [Current Interest Balance],
		LN10.LA_CUR_PRI AS [Current Principal Balance],
		FB10.LD_FOR_END AS [Anticipated Capitalization Date],
		CONVERT(DECIMAL(18,2),ROUND((DATEDIFF(DAY,GETDATE(),CAST(FB10.LD_FOR_END AS DATE)) * LN72.LR_ITR/100/365 * LN10.LA_CUR_PRI) + DW01.WA_TOT_BRI_OTS, 2)) AS [Anticipated Capitalization Amount]
	FROM
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
		JOIN FB10_Forbearance FB10
			ON LN10.DF_SPE_ACC_ID = FB10.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = FB10.LN_SEQ
			AND GETDATE() BETWEEN CAST(FB10.LD_FOR_BEG AS DATE) AND CAST(FB10.LD_FOR_END AS DATE)
		LEFT JOIN --get int paid since last statement on the highest numbered bill (LN_SEQ_BIL_WI_DTE) for the most recent statement (LD_BIL_CRT) 
			(
				SELECT
					BL10.DF_SPE_ACC_ID,
					BL10.LN_SEQ,
					BL10.LA_INT_PD_LST_STM
				FROM
					(
						SELECT
							DF_SPE_ACC_ID,
							LN_SEQ,
							MAX(CAST(LD_BIL_CRT AS DATE)) AS LD_BIL_CRT
						FROM
							BL10_Bill
						WHERE
							DF_SPE_ACC_ID = @AccountNumber
						GROUP BY
							DF_SPE_ACC_ID,
							LN_SEQ
					) MXBL
					JOIN BL10_Bill BL10
						ON MXBL.DF_SPE_ACC_ID = BL10.DF_SPE_ACC_ID
						AND MXBL.LN_SEQ = BL10.LN_SEQ
						AND MXBL.LD_BIL_CRT = BL10.LD_BIL_CRT
			) BILL
			ON LN10.DF_SPE_ACC_ID = BILL.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = BILL.LN_SEQ
		JOIN DW01_Loan DW01
			ON LN10.DF_SPE_ACC_ID = DW01.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = DW01.LN_SEQ
		JOIN LN72_InterestRate LN72
			ON LN10.DF_SPE_ACC_ID = LN72.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN72.LN_SEQ
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BRSMRY_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BRSMRY_Loans] TO [db_executor]
    AS [dbo];
