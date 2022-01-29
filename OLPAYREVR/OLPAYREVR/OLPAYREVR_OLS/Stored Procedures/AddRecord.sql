CREATE PROCEDURE [olpayrevr].[AddRecord]
	@Ssn CHAR(9),
	@PaymentAmount FLOAT,
	@PaymentEffectiveDate DATE,
	@PaymentType CHAR(2),
	@CreatedAt DATETIME

AS
	IF NOT EXISTS (SELECT Ssn FROM olpayrevr.ReversalsProcessingQueue WHERE Ssn = @Ssn AND PaymentAmount = @PaymentAmount AND PaymentEffectiveDate = @PaymentEffectiveDate AND PaymentType = @PaymentType AND DeletedAt IS NULL AND CreatedAt != @CreatedAt) --Don't add duplicate records on a different run, same-run dupes are okay
		BEGIN
			INSERT INTO olpayrevr.ReversalsProcessingQueue(Ssn, PaymentAmount, PaymentEffectiveDate, PaymentType, CreatedAt)
			VALUES (@Ssn, @PaymentAmount, @PaymentEffectiveDate, @PaymentType, @CreatedAt)
		END

RETURN 0
