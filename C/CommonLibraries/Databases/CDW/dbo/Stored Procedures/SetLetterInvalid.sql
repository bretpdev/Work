

CREATE PROCEDURE [dbo].[SetLetterInvalid]
	@LetterRequestId INT
AS
	UPDATE
		LT20_LetterRequests
	SET
		InactivatedAt = GETDATE(),
		[SystemLetterExclusionReasonId] = 5
	WHERE
		LT20_LETTER_REQUEST_ID = @LetterRequestId
RETURN 0