CREATE PROCEDURE [lnderlettr].[GetLetterIds]
	@WF_ORG_LDR char(8)
AS
	SELECT
		LettersId
	FROM
		[lnderlettr].Letters
	WHERE
		LetterCreatedAt IS NULL
		AND DeletedAt IS NULL
		AND (InLenderList = 1
		OR II_LDR_VLD_ADR = 'Y')
		AND WF_ORG_LDR = @WF_ORG_LDR
GO
GRANT EXECUTE
    ON OBJECT::[lnderlettr].[GetLetterIds] TO [db_executor]
    AS [dbo];

