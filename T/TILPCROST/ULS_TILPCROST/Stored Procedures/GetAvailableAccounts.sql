CREATE PROCEDURE [tilpcrost].[GetAvailableAccounts]
AS
	SELECT
		AccountsId,
		AccountNumber,
		Ssn,
		TransactionType,
		LoanSequence,
		PrincipalAmount,
		TransactionDate,
		LastName
	FROM
		tilpcrost.Accounts
	WHERE
		ProcessedAt IS NULL
		AND DeletedAt IS NULL
RETURN 0