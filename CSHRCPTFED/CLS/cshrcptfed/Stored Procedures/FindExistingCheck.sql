CREATE PROCEDURE [cshrcptfed].[FindExistingCheck]
	@AccountNumber CHAR(10),
	@CheckNumber VARCHAR(15),
	@Date DATETIME,
	@Payee INT
AS

	SELECT 
		AddedAt, AddedBy
	FROM 
		CLS.cshrcptfed.CashReceipts 
	WHERE
		Account = @AccountNumber
		AND 
		CheckNum = @CheckNumber
		AND
		Payee = @Payee
		AND 
		CONVERT(VARCHAR, DateRecvd, 101) = CONVERT(VARCHAR, @Date, 101)

RETURN 0
