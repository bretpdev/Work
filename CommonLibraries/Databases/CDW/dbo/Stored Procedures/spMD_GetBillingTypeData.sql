CREATE PROCEDURE [dbo].[spMD_GetBillingTypeData] 
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT BIL_MTD as [Type]
	FROM dbo.LOAN_Bill
	WHERE DF_SPE_ACC_ID = @AccountNumber
	AND BIL_MTD <> ''

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetBillingTypeData] TO [Imaging Users]
    AS [dbo];

