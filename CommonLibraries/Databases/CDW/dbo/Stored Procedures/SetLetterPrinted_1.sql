
CREATE PROCEDURE SetLetterPrinted
	@LetterRequestId INT
AS
	UPDATE
		LT20_LetterRequests
	SET
		PrintedAt = GETDATE()
	WHERE
		LT20_LETTER_REQUEST_ID = @LetterRequestId
RETURN 0