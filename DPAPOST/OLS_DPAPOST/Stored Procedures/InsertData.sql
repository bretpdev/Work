CREATE PROCEDURE [dpapost].[InsertData]
	@AccountNumber VARCHAR(10),
	@Amount FLOAT
AS
	MERGE
		OLS.dpapost.PostingData PD
	USING
		(
			SELECT
				@AccountNumber AccountNumber,
				@Amount Amount
		) ID
			ON ID.AccountNumber = PD.AccountNumber
			AND ID.Amount = PD.Amount
			AND CAST(PD.AddedAT AS DATE) = CAST(GETDATE() AS DATE)
			AND PD.DeletedAt IS NULL
	WHEN NOT MATCHED THEN
		INSERT (AccountNumber, Amount)
		VALUES(@AccountNumber, @Amount);