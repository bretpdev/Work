
CREATE PROCEDURE [dbo].[spMD_GetRPSTypeData]
	@AccountNumber					VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT TYP_SCH_DIS
	FROM dbo.LN65_RepaymentSched
	WHERE DF_SPE_ACC_ID = @AccountNumber 
		AND TYP_SCH_DIS <> ''
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetRPSTypeData] TO [UHEAA\Imaging Users]
    AS [dbo];

