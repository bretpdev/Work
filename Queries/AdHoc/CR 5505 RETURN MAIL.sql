
UPDATE 
	COM
SET
	COM.CommunicationLetterReturned = 1
FROM
	CDW..CR_5505_BORROWER_COMMUNICATIONS COM
	INNER JOIN CLS.[print].PrintProcessing PP
		ON PP.PrintProcessingId = COM.PrintProcessingId
	INNER JOIN CLS.[print].ScriptData SD
		ON SD.ScriptDataId = PP.ScriptDataId
	INNER JOIN CLS.[print].Letters L
		ON L.LetterId = SD.LetterId
	INNER JOIN CLS.barcodefed.ReturnMail RM
		ON RM.RecipientId = COM.DF_SPE_ACC_ID
		AND RM.LetterId = L.Letter
		AND RM.AddedAt > PP.PrintedAt

UPDATE 
	COM
SET
	COM.CommunicationLetterReturned = 1
FROM
	CDW..CR_5505_COBORROWER_COMMUNICATIONS COM
	INNER JOIN CLS.[print].PrintProcessing PP
		ON PP.PrintProcessingId = COM.PrintProcessingId
	INNER JOIN CLS.[print].ScriptData SD
		ON SD.ScriptDataId = PP.ScriptDataId
	INNER JOIN CLS.[print].Letters L
		ON L.LetterId = SD.LetterId
	INNER JOIN CLS.barcodefed.ReturnMail RM
		ON RM.RecipientId = COM.DF_SPE_ACC_ID
		AND RM.LetterId = L.Letter
		AND RM.AddedAt > PP.PrintedAt