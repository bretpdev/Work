CREATE PROCEDURE [lnderlettr].[GetLetters]
AS
	SELECT
		LettersId,
		BF_SSN,
		WF_ORG_LDR,
		II_LDR_VLD_ADR,
		InLenderList,
		LetterCreatedAt,
		ArcAddedAt,
		QueueClosedAt
	FROM
		[lnderlettr].Letters
	WHERE
		DeletedAt IS NULL
		AND (
				(LetterCreatedAt IS NULL AND (InLenderList = 1 OR II_LDR_VLD_ADR = 'Y'))
				OR (ArcAddedAt IS NULL AND II_LDR_VLD_ADR = 'N')
				OR QueueClosedAt IS NULL
			)
GO
GRANT EXECUTE
    ON OBJECT::[lnderlettr].[GetLetters] TO [db_executor]
    AS [dbo];

