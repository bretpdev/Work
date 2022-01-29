UPDATE 
	ULS.finalrev.BorrowerRecord 
SET
	DeleteBy = 'UNH 72337',
	DeletedAt = GETDATE()
where BorrowerRecordID = 19