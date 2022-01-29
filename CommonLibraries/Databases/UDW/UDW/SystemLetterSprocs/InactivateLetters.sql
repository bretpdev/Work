CREATE PROCEDURE [dbo].[InactivateLetters]

AS
	--inactivate all suspressed and manual print letters
	UPDATE
		LT20
	SET
		InactivatedAt = GETDATE(),
		SystemLetterExclusionReasonId = SLE.SystemLetterExclusionReasonId
	FROM
		LT20_LTR_REQ_PRC LT20
		INNER JOIN ULS..SystemLetterExclusions SLE
			ON SLE.LetterId = LT20.RM_DSC_LTR_PRC
	WHERE
		LT20.PrintedAt IS NULL
		AND LT20.InactivatedAt IS NULL
		AND SLE.SystemLetterExclusionReasonId != 3

	--inactivate records AES marked for deletion deleted records
	UPDATE
		LT20_LTR_REQ_PRC
	SET
		InactivatedAt = GETDATE(),
		SystemLetterExclusionReasonId = 8
	WHERE
		RI_LTR_REQ_DEL_PRC = 'Y'
		AND PrintedAt IS NULL
		AND InactivatedAt IS NULL
RETURN 0
