CREATE PROCEDURE [dbo].[LT_TS06BNEGAM_InterestAmount]
	@AccountNumber		char(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		SUM(FS12.LA_PRJ_NEG_AMR_INT) AS [InterestAmount]
	FROM
		[FS12_NegAm] FS12
	WHERE 
		FS12.DF_SPE_ACC_ID = @AccountNumber
		AND FS12.LD_CRT_NEG_AMR_LTR = (select max(LD_CRT_NEG_AMR_LTR) from FS12_NegAm where DF_SPE_ACC_ID = FS12.DF_SPE_ACC_ID)
	GROUP BY FS12.DF_SPE_ACC_ID

END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned.',11,2)
	END