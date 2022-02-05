CREATE PROCEDURE [tilpcrost].[InsertRecord]
	@AccountNumber VARCHAR(10),
	@TransactionType VARCHAR(10),
	@LoanSequence INT,
	@PrincipalAmount FLOAT,
	@TransactionDate DATETIME,
	@LastName VARCHAR(50)
AS

	DECLARE @Ssn VARCHAR(9) = (SELECT DF_PRS_ID FROM UDW..PD10_PRS_NME WHERE DF_SPE_ACC_ID = @AccountNumber)

	IF NOT EXISTS(SELECT * FROM tilpcrost.Accounts WHERE AccountNumber = @AccountNumber AND LoanSequence = @LoanSequence AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE))
		BEGIN
			INSERT INTO tilpcrost.Accounts(AccountNumber, Ssn, TransactionType, LoanSequence, PrincipalAmount, TransactionDate, LastName)
			VALUES(@AccountNumber, @Ssn, @TransactionType, @LoanSequence, @PrincipalAmount, @TransactionDate, @LastName)
		END