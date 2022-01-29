
CREATE PROCEDURE [dbo].[GetPaymentAmount] 

@AccountNumber VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

    SELECT
		SUM(LN65.LA_RPS_ISL)
	FROM
		LN65_RepaymentSched LN65
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPaymentAmount] TO [db_executor]
    AS [dbo];

