CREATE PROCEDURE [lnderlettr].[GetLetterData]
	@LettersId INT
AS
	SELECT
		LettersId,
		BF_SSN,
		II_LDR_VLD_ADR,
		InLenderList,
		LetterCreatedAt,
		ArcAddedAt,
		QueueClosedAt
	FROM
		[lnderlettr].Letters
	WHERE
		LettersId = @LettersId
GO
GRANT EXECUTE
    ON OBJECT::[lnderlettr].[GetLetterData] TO [db_executor]
    AS [dbo];

