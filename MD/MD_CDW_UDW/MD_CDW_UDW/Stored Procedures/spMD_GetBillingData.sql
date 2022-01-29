CREATE PROCEDURE [dbo].[spMD_GetBillingData] 
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT distinct LD_BIL_DU_LON as Due,
		LD_BIL_CRT as Billed,
		BIL_SAT as Satisfied,
		LD_BIL_STS_RIR_TOL as DateSatisfied,
		cast(LD_BIL_DU_LON as DateTime) as orderdate
	FROM dbo.BL10_Bill
	WHERE DF_SPE_ACC_ID = @AccountNumber
	ORDER BY BIL_SAT, cast(LD_BIL_DU_LON as DateTime) DESC
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetBillingData] TO [Imaging Users]
    AS [dbo];

