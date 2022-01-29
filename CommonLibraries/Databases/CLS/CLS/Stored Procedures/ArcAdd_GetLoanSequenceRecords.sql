CREATE PROCEDURE [dbo].[ArcAdd_GetLoanSequenceRecords]
	@ArcAddProcessingId	bigint
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
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetLoanSequenceRecords] TO [db_executor]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetLoanSequenceRecords] TO [UHEAA\SystemAnalysts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetLoanSequenceRecords] TO [UHEAA\CornerStoneUsers]
    AS [dbo];