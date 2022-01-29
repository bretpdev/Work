CREATE PROCEDURE [cntrprnt].[GetUnprocessedLetterRecordsSummary]
AS

SELECT 
	LR.UHEAACostCenter,
	CASE LR.Duplex
		WHEN 1 THEN CEILING(Pages / 2)
		ELSE Pages
	END AS PagesToPrint,
	LR.Duplex,
	ISNULL(PR.Domestic, 'N') DomesticCalc,
	LR.Instructions,
	PR.LetterId,
	COUNT(*) SummaryCount
FROM 
	PRNT_DAT_Print PR
	LEFT JOIN LTDB_DAT_CentralPrintingDocData LR
		ON PR.LetterID = LR.[ID]
WHERE 
	(
		PR.IsOnEcorr IS NULL 
		OR (PR.IsOnEcorr = 1 AND PR.EcorrDocumentCreatedAt IS NULL) 
		OR (PR.IsOnEcorr = 0 AND PR.PrintedAt IS NULL)
	)
	AND	DeletedAt IS NULL
GROUP BY LR.UHEAACostCenter,
	CASE LR.Duplex
		WHEN 1 THEN CEILING(Pages / 2)
		ELSE Pages
	END,
	LR.Duplex,
	ISNULL(PR.Domestic, 'N'),
	LR.Instructions,
	PR.LetterId
ORDER BY
	LR.Duplex

RETURN 0