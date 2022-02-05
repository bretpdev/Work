CREATE PROCEDURE [dbo].[LT_TS06BTM2LM]
	@AccountNumber			varchar(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT top 1
		LN10.LD_PIF_RPT as [EffDate]
	FROM
		dbo.LN10_Loan LN10
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber 
		AND LN10.LD_PIF_RPT = (SELECT MAX(cast(LN10date.LD_PIF_RPT as datetime)) 
							FROM LN10_Loan LN10date 
							WHERE LN10date.DF_SPE_ACC_ID = LN10.DF_SPE_ACC_ID)
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BTM2LM])',11,2)
	END