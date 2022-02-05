
CREATE PROCEDURE spOpsCbp_GetDaysDelinquent
	@AccountNumber	CHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	COALESCE(DELQ.CUR_DLQ, 0) as DaysDelinquent
	FROM	dbo.BORR_Delinquency DELQ
	WHERE	DELQ.DF_SPE_ACC_ID = @AccountNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spOpsCbp_GetDaysDelinquent] TO [db_executor]
    AS [dbo];

