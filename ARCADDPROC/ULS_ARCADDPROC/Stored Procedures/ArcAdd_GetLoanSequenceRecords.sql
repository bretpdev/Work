CREATE PROCEDURE [dbo].[ArcAdd_GetLoanSequenceRecords]
	@ArcAddProcessingId	int
AS
BEGIN
	SELECT
		LoanSequence
	FROM
		ArcLoanSequenceSelection
	WHERE
		ArcAddProcessingId = @ArcAddProcessingId
RETURN 0
END