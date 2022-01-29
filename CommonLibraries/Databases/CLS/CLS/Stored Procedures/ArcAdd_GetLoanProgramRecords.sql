CREATE PROCEDURE [dbo].[ArcAdd_GetLoanProgramRecords]
	@ArcAddProcessingId	bigint
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
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetLoanProgramRecords] TO [db_executor]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetLoanProgramRecords] TO [UHEAA\SystemAnalysts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetLoanProgramRecords] TO [UHEAA\CornerStoneUsers]
    AS [dbo];