UPDATE
	CLS.[print].PrintProcessing 
SET
	PrintedAt = NULL,
	EcorrDocumentCreatedAt = NULL,
	ImagedAt = NULL
FROM
	CLS.[print].PrintProcessing 	
WHERE
	SourceFile LIKE '%UNWSXX%'
	AND DATEDIFF(DAY,AddedAt,GETDATE()) = X
	AND OnEcorr = X
	AND ScriptFileId NOT IN (XX,X)
