CREATE PROCEDURE [smbalwo].[MarkRecordAsProcessed]
	@LoanWriteOffId int,
	@HadError BIT,
	@ActualPrincipalWrittenOff DECIMAL(18,2) = NULL,
	@ActualInterestWrittenOffint DECIMAL(18,2) = NULL
AS
	UPDATE
		CLS.smbalwo.LoanWriteOffs
	SET
		ProcessedAt = GETDATE(),
		HadError = @HadError,
		ActualPrincipalWrittenOff = @ActualPrincipalWrittenOff,
		ActualInterestWrittenOff = @ActualInterestWrittenOffint
	WHERE
		LoanWriteOffId = @LoanWriteOffId
RETURN 0
