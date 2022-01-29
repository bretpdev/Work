CREATE PROCEDURE [dbo].[LT_TS06BTPDSS]
	@AccountNumber			CHAR(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		LN10.LN_SEQ,
		LN10.IC_LON_PGM,
		LN10.LD_LON_1_DSB,
		LN10.LA_CUR_PRI
	FROM
		dbo.LN10_Loan LN10
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BTPDSS])',11,2)
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BTPDSS] TO [db_executor]
    AS [dbo];

