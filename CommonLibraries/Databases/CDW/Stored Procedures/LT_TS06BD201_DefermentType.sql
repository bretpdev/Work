CREATE PROCEDURE [dbo].[LT_TS06BD201_DefermentType]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		DF10.DFR_TYP AS [Deferment Type]
	FROM
		DF10_Deferment DF10
	WHERE 
		DF10.DF_SPE_ACC_ID = @AccountNumber
		AND GETDATE() BETWEEN CAST(DF10.LD_DFR_BEG AS DATE) AND CAST(DF10.LD_DFR_END AS DATE)
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BD201_DefermentType])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BD201_DefermentType] TO [db_executor]
    AS [dbo];
