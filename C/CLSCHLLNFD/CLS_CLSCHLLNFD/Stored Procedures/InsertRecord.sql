CREATE PROCEDURE [clschllnfd].[InsertRecord]
		@BF_SSN CHAR(9),
	@StudentSsn CHAR(9),
	@LN_SEQ INT,
	@DisbursementSeq INT,
	@DischargeDate DATETIME,
	@DischargeAmount MONEY,
	@SchoolCode CHAR(8)
AS
BEGIN
	BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

		IF NOT EXISTS (SELECT * FROM clschllnfd.SchoolClosureData WHERE BorrowerSsn = @BF_SSN AND LoanSeq = @LN_SEQ AND DisbursementSeq = @DisbursementSeq AND StudentSsn = @StudentSsn AND DischargeAmount = @DischargeAmount AND DischargeDate = @DischargeDate AND SchoolCode = @SchoolCode AND DeletedAt IS NULL)
			BEGIN
				INSERT INTO clschllnfd.SchoolClosureData(BorrowerSsn, StudentSsn, LoanSeq, DisbursementSeq, DischargeAmount, DischargeDate, SchoolCode)
				VALUES(@BF_SSN, @StudentSsn, @LN_SEQ, @DisbursementSeq, @DischargeAmount, @DischargeDate, @SchoolCode)
				SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT
			END

	IF(@ERROR = 0 AND @@ROWCOUNT = 1)
		BEGIN
			COMMIT TRANSACTION
			SELECT CAST(1 AS BIT) [WasSuccessful]
		END
	ELSE
		BEGIN
			ROLLBACK TRANSACTION
			SELECT CAST(0 AS BIT) [WasSuccessful]
		END
END

RETURN 0