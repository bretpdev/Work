UPDATE
	CDW..LTXX_LetterRequests
SET
	InactivatedAt = GETDATE(),
	SystemLetterExclusionReasonId = X
WHERE
	PrintedAt IS NULL 
	AND InactivatedAt IS NULL 
	AND LTXX_LETTER_REQUEST_ID = 'XXXXXXX'
