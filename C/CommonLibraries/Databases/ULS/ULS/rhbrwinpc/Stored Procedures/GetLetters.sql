CREATE PROCEDURE [rhbrwinpc].[GetLetters]
AS
	SELECT
		LettersId,
		DF_SPE_ACC_ID,
		DM_PRS_1,
		DM_PRS_LST,
		DX_STR_ADR_1,
		DX_STR_ADR_2,
		DM_CT,
		DC_DOM_ST,
		DF_ZIP,
		DM_FGN_CNY,
		PrintedAt,
		ArcAddedAt
	FROM
		[rhbrwinpc].Letters
	WHERE
		DeletedAt IS NULL
		AND (
				PrintedAt IS NULL
				OR ArcAddedAt IS NULL
			)
GO
GRANT EXECUTE
    ON OBJECT::[rhbrwinpc].[GetLetters] TO [db_executor]
    AS [dbo];

