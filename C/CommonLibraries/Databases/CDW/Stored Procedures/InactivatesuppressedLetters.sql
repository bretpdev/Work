CREATE PROCEDURE [dbo].[InactivateSuppressedLetters]

AS
	UPDATE LT20
		
	SET
		InactivatedAt = GETDATE(),
		LT20.SystemLetterExclusionReasonId = SLE.SystemLetterExclusionReasonId
	FROM
		[CDW].dbo.LT20_LetterRequests LT20
	INNER JOIN [CLS].dbo.[SystemLetterExclusions] SLE
		ON SLE.LetterID = LT20.RM_DSC_LTR_PRC
		AND SLE.SystemLetterExclusionReasonId = 1
	WHERE
		LT20.PrintedAt IS NULL 
		AND LT20.InactivatedAt IS NULL


RETURN 0

GRANT EXECUTE ON [dbo].[InactivateSuppressedLetters] TO db_executor
GO
