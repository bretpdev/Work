
CREATE PROCEDURE [smbalwo].[UpdateActualWriteOffAmounts]
	@LoanWriteOffId int,
	@ActualPrincipalWrittenOff DECIMAL(18,2),
	@ActualInterestWrittenOffint DECIMAL(18,2)
AS
	UPDATE
		ULS.smbalwo.LoanWriteOffs
	SET
		ActualPrincipalWrittenOff = @ActualPrincipalWrittenOff,
		ActualInterestWrittenOff = @ActualInterestWrittenOffint
	WHERE
		LoanWriteOffId = @LoanWriteOffId
RETURN 0