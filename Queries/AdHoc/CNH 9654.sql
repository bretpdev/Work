UPDATE
	CDW..LTXX_LetterRequests
SET
	InactivatedAt = GETDATE(),
	SystemLetterExclusionReasonId = X
WHERE
	LTXX_LETTER_REQUEST_ID IN(XXXXXXX,XXXXXXX,XXXXXXX,XXXXXXX,XXXXXXX)
	
INSERT INTO CLS..SystemLetterExclusions(LetterId, SystemLetterExclusionReasonId) 
VALUES('TSXXBTCRDX',X)
