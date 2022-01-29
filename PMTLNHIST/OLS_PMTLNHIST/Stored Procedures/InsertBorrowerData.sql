CREATE PROCEDURE [pmtlnhist].[InsertBorrowerData]
	@AccountNumber VARCHAR(10),
	@Name VARCHAR(50),
	@Principal FLOAT,
	@Interest FLOAT,
	@LegalCosts FLOAT,
	@OtherCosts FLOAT,
	@CollectionCosts FLOAT,
	@ProjectedCollectionCosts FLOAT
AS
	IF NOT EXISTS (SELECT * FROM pmtlnhist.BorrowerLoanPayment WHERE AccountNumber = @AccountNumber AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE))
		BEGIN
			INSERT INTO pmtlnhist.BorrowerLoanPayment(AccountNumber, [Name], Principal, Interest, LegalCosts, OtherCosts, ProjectedCollectionCosts)
			VALUES(@AccountNumber, @Name, @Principal, @Interest, @LegalCosts, @OtherCosts, @ProjectedCollectionCosts)
		END

	SELECT
		BorrowerLoanPaymentId
	FROM
		pmtlnhist.BorrowerLoanPayment
	WHERE
		AccountNumber = @AccountNumber
		AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)