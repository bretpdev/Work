UPDATE
	[CLS].[print].[PrintProcessing]
SET
	PrintedAt = NULL
WHERE
	(
		CAST(AddedAt AS DATE) = 'X/XX/XX'
		AND PrintedAt IS NOT NULL
		AND OnEcorr = X
	)
	OR 
	(
		CAST(AddedAt AS DATE) = 'X/XX/XX'
		AND PrintedAt IS NOT NULL
		AND ScriptFileId IN (X,X,XX,XX,XX,XX)
	)