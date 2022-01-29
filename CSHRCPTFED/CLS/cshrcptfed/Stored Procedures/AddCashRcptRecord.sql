CREATE PROCEDURE [cshrcptfed].[AddCashRcptRecord]
	@Account	VARCHAR(10),
	@Borrower VARCHAR(48),
	@Amount	DECIMAL(18,2),
	@Check		VARCHAR(15),
	@Payee		INT,
	@Date		DATETIME,
	@ArcId		BIGINT = NULL,
	@WindowsUser VARCHAR(32)
AS
BEGIN
		INSERT INTO 
		[cshrcptfed].CashReceipts
		(
			Account,
			Borrower,
			AmountRecvd,
			CheckNum,
			DateRecvd,
			Payee,
			ArcId,
			AddedBy,
			AddedAt
		)
		VALUES
		(
			@Account,
			@Borrower,
			@Amount,
			@Check,
			@Date,
			@Payee,
			@ArcId,
			@WindowsUser,
			GETDATE()
		)
		SELECT SCOPE_IDENTITY()

END