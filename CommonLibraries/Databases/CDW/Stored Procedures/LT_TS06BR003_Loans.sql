CREATE PROCEDURE [dbo].[LT_TS06BR003_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		FMT.Label AS [Loan Program],
		LN10.LD_LON_1_DSB AS [Disbursement Date],
		LN10.LA_LON_AMT_GTR AS  [Original Balance],
		LN90.LA_FAT_CUR_PRI+LN90.LA_FAT_NSI+LN90.LA_FAT_LTE_FEE AS [Refund Amount],
		convert(varchar(10), LN90.LD_FAT_EFF, 101) as [Refund Date],
		LN10.LA_CUR_PRI AS [Current Principal Balance]
	FROM 
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
		JOIN
			(
				SELECT
					DF_SPE_ACC_ID,
					LN_SEQ,
					MAX(convert(date,LD_FAT_EFF)) AS LD_FAT_EFF
				FROM
					LN90_FinancialHistory 
				WHERE
					((PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP =  '45') OR (PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP =  '40'))
					AND LC_FAT_REV_REA = '' -- reversed records
					AND LC_STA_LON90 = 'A' -- active
				GROUP BY
					DF_SPE_ACC_ID,
					LN_SEQ
			) MX
			ON LN10.DF_SPE_ACC_ID = MX.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = MX.LN_SEQ
		JOIN LN90_FinancialHistory LN90
			ON MX.DF_SPE_ACC_ID = LN90.DF_SPE_ACC_ID
			AND MX.LN_SEQ = LN90.DF_SPE_ACC_ID
			AND MX.LD_FAT_EFF = LN90.LD_FAT_EFF
	WHERE 
		((LN90.PC_FAT_TYP = '10' AND LN90.PC_FAT_SUB_TYP =  '45') OR (LN90.PC_FAT_TYP = '10' AND LN90.PC_FAT_SUB_TYP =  '40'))
		AND LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BR003_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BR003_Loans] TO [db_executor]
    AS [dbo];
