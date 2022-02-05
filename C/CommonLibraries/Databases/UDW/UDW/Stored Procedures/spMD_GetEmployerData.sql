CREATE PROCEDURE [dbo].[spMD_GetEmployerData]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--get all Employer information
	SELECT IM_IST_FUL as [Name],
		IX_GEN_STR_ADR_1 as Addr1,
		IX_GEN_STR_ADR_2 as Addr2,
		IM_GEN_CT as City,
		IC_GEN_ST as State,
		IF_GEN_ZIP as Zip,
		IN_GEN_PHN as Phone
	FROM dbo.BR02_Employer
	WHERE DF_SPE_ACC_ID = @AccountNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetEmployerData] TO [UHEAA\Imaging Users]
    AS [dbo];

