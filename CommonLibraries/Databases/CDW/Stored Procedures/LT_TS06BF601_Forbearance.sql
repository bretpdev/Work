CREATE PROCEDURE [dbo].[LT_TS06BF601_Forbearance]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		FB10.FOR_TYP AS [Forbearance Type],
		FB10.LD_FOR_BEG AS [Forbearance Begin Date],
		FB10.LD_FOR_END AS [Forbearance End Date]
	FROM
		FB10_Forbearance FB10
	WHERE 
		FB10.DF_SPE_ACC_ID = @AccountNumber
		AND GETDATE() BETWEEN CAST(FB10.LD_FOR_BEG AS DATE) AND CAST(FB10.LD_FOR_END AS DATE)
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BF601_Forbearance])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BF601_Forbearance] TO [db_executor]
    AS [dbo];
