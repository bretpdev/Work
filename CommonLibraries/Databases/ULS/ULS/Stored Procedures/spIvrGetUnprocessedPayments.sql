
CREATE PROCEDURE spIvrGetUnprocessedPayments

AS
BEGIN
	SET NOCOUNT ON;
	SELECT	A.RecNum,
			A.AccountNumber,
			A.BankAccountNum,
			A.AccountType,
			A.RoutingNum,
			CONVERT(VARCHAR(20),A.Amount,1) AS  Amount,
			CONVERT(VARCHAR(12),A.AuthDate,101) AS AuthDate
	 FROM	IvrCheckByPhone A 
	 WHERE	A.ProcessedDate IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetUnprocessedPayments] TO [UHEAA\UHEAAUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetUnprocessedPayments] TO [db_executor]
    AS [dbo];

