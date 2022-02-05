CREATE PROCEDURE [dbo].[ArcAdd_GetLoanProgramRecords]
	@ArcAddProcessingId	int
AS
BEGIN
	SELECT
		LoanProgram
	FROM
		ArcLoanProgramSelection
	WHERE
		ArcAddProcessingId = @ArcAddProcessingId
RETURN 0
END