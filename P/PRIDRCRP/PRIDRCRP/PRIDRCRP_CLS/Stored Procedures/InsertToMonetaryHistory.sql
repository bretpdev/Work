CREATE PROCEDURE [pridrcrp].[InsertToMonetaryHistory]
(
	@MonetaryHistoryRecord MonetaryHistoryRecord READONLY
)
AS

DECLARE @NewIds TABLE
(
	MonetaryHistoryId INT
)

INSERT INTO [pridrcrp].MonetaryHistory (Ssn, LoanNum, TransactionDate, PostDate, TransactionCode, CancelCode, TransactionAmount, AppliedPrincipal, AppliedInterest, AppliedFees, PrincipalBalanceAfterTransaction, InterestBalanceAfterTransaction, FeesBalanceAfterTransaction, LoanSequence)
OUTPUT inserted.MonetaryHistoryId INTO @NewIds
SELECT
	MHR.[Ssn],
	MHR.[LoanNum],
	MHR.[TransactionDate],
	MHR.[PostDate],
	MHR.[TransactionCode],
	MHR.[CancelCode],
	MHR.[TransactionAmount],
	MHR.[AppliedPrincipal],
	MHR.[AppliedInterest],
	MHR.[AppliedFees],
	MHR.[PrincipalBalanceAfterTransaction],
	MHR.[InterestBalanceAfterTransaction],
	MHR.[FeesBalanceAfterTransaction],
	MHR.LoanSequence
FROM
	@MonetaryHistoryRecord MHR
	LEFT JOIN [pridrcrp].MonetaryHistory MH
		ON MHR.[Ssn] = MH.[Ssn]
		AND MHR.[LoanNum] = MH.[LoanNum]
		AND MHR.[TransactionDate] = MH.[TransactionDate]
		AND MHR.[PostDate] = MH.[PostDate]
		AND MHR.[TransactionCode] = MH.[TransactionCode]
		AND MHR.[CancelCode] = MH.[CancelCode]
		AND MHR.[TransactionAmount] = MH.[TransactionAmount]
		AND MHR.[AppliedPrincipal] = MH.[AppliedPrincipal]
		AND MHR.[AppliedInterest] = MH.[AppliedInterest]
		AND MHR.[AppliedFees] = MH.[AppliedFees]
		AND MHR.[PrincipalBalanceAfterTransaction] = MH.[PrincipalBalanceAfterTransaction]
		AND MHR.[InterestBalanceAfterTransaction] = MH.[InterestBalanceAfterTransaction]
		AND MHR.[FeesBalanceAfterTransaction] = MH.[FeesBalanceAfterTransaction]
		AND COALESCE(MHR.[LoanSequence],0) = COALESCE(MH.[LoanSequence],0) --Allows for field to be null
WHERE
	MH.Ssn IS NULL

SELECT
	MonetaryHistoryId
FROM
	@NewIds

	--Need to pull back records that already have Monetary history ids for the case that they havent had the mapping set up yet
	UNION

	(
		SELECT
			MonetaryHistoryId
		FROM
			@MonetaryHistoryRecord MHR
			INNER JOIN [pridrcrp].MonetaryHistory MH
				ON MHR.[Ssn] = MH.[Ssn]
				AND MHR.[LoanNum] = MH.[LoanNum]
				AND MHR.[TransactionDate] = MH.[TransactionDate]
				AND MHR.[PostDate] = MH.[PostDate]
				AND MHR.[TransactionCode] = MH.[TransactionCode]
				AND MHR.[CancelCode] = MH.[CancelCode]
				AND MHR.[TransactionAmount] = MH.[TransactionAmount]
				AND MHR.[AppliedPrincipal] = MH.[AppliedPrincipal]
				AND MHR.[AppliedInterest] = MH.[AppliedInterest]
				AND MHR.[AppliedFees] = MH.[AppliedFees]
				AND MHR.[PrincipalBalanceAfterTransaction] = MH.[PrincipalBalanceAfterTransaction]
				AND MHR.[InterestBalanceAfterTransaction] = MH.[InterestBalanceAfterTransaction]
				AND MHR.[FeesBalanceAfterTransaction] = MH.[FeesBalanceAfterTransaction]
				AND COALESCE(MHR.[LoanSequence],0) = COALESCE(MH.[LoanSequence],0) --Allows for field to be null		
	)