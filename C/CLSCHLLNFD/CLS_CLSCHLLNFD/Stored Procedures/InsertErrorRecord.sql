CREATE PROCEDURE [clschllnfd].[InsertErrorRecord]
	@BorrowerSsn CHAR(9), 
    @AccountNumber CHAR(10), 
    @LoanSeq INT, 
    @DisbursementSeq INT, 
    @Arc VARCHAR(8), 
    @ErrorMessage VARCHAR(300), 
    @SessionMessage VARCHAR(300),
	@SchoolClosureDataId INT
AS
	IF NOT EXISTS(SELECT * FROM clschllnfd.ErrorLogs WHERE BorrowerSsn = @BorrowerSsn AND LoanSeq = @LoanSeq AND DisbursementSeq = @DisbursementSeq AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE))
		BEGIN
			INSERT INTO 
				clschllnfd.ErrorLogs
				(
					BorrowerSsn, 
					AccountNumber, 
					LoanSeq, 
					DisbursementSeq, 
					Arc, 
					ErrorMessage, 
					SessionMessage,
					SchoolClosureDataId
				)
			VALUES
			(
				@BorrowerSsn, 
				@AccountNumber, 
				@LoanSeq, 
				@DisbursementSeq, 
				@Arc, 
				@ErrorMessage, 
				@SessionMessage,
				@SchoolClosureDataId
			)
		END

RETURN 0
