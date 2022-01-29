USE ULS
GO

UPDATE 
	ULS.[print].PrintProcessing
SET
	DeletedAt = getdate(),
	DeletedBy = 'DCR'
WHERE
	PrintProcessingId = '898628'