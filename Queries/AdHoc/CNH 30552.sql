UPDATE 
	CDW..LTXX_LetterRequests 
SET 
	InactivatedAt = GETDATE(), 
	SystemLetterExclusionReasonId = X 
FROM 
	CDW..LTXX_LetterRequests 
WHERE
	LTXX_LETTER_REQUEST_ID IN(XXXXXXX,XXXXXXX,XXXXXXX,XXXXXXX,XXXXXXX)
