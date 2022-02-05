CREATE PROCEDURE [dbo].[spMD_GetCallForwardingData]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT FORWARDING
	FROM dbo.LN10_Loan
	WHERE DF_SPE_ACC_ID = @AccountNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetCallForwardingData] TO [Imaging Users]
    AS [dbo];

