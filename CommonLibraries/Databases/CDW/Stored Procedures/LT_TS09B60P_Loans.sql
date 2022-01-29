CREATE PROCEDURE [dbo].[LT_TS09B60P_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		LN10.IC_LON_PGM AS [Loan Program],
		'US Dept of Ed' AS [Current Owner],
		LN10.LD_LON_1_DSB AS [First Disbursement Date],
		LN16.LN_DLQ_MAX + 1	 AS [Days Delinquent],
		LN16.LD_DLQ_OCC AS [Due Date],
		DUE.CUR_DUE AS [Total Past Due Amount]
	FROM
		LN10_Loan LN10
		--JOIN FormatTranslation FMT
		--	ON LN10.IC_LON_PGM = FMT.Start
		--	AND LN10.LA_CUR_PRI > 0
		JOIN LN16_Delinquency LN16
			ON LN10.DF_SPE_ACC_ID = LN16.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN16.LN_SEQ
		JOIN 
			(
				SELECT 
					BL10.DF_SPE_ACC_ID,
					BL10.LN_SEQ,
					SUM(COALESCE(BL10.LA_BIL_CUR_DU,0) - COALESCE(BL10.LA_TOT_BIL_STS,0)) AS CUR_DUE
				FROM 
					BL10_Bill BL10
				WHERE
					BL10.DF_SPE_ACC_ID = @AccountNumber
					AND CAST(BL10.LD_BIL_DU_LON AS DATE) < DATEADD(M,1,GETDATE())
					AND BL10.LC_IND_BIL_SNT <> '0'
				GROUP BY
					BL10.DF_SPE_ACC_ID,
					BL10.LN_SEQ
			) DUE
				ON LN10.DF_SPE_ACC_ID = DUE.DF_SPE_ACC_ID
				AND LN10.LN_SEQ = DUE.LN_SEQ
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS09B60P_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS09B60P_Loans] TO [db_executor]
    AS [dbo];
