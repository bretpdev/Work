CREATE PROCEDURE [cntrprnt].[GetUnprocessedLetterRecords]
AS

SELECT 
	LR.UHEAACostCenter,
	CASE LR.Duplex
		WHEN 1 THEN CEILING(Pages / 2)
		ELSE Pages
	END AS PagesToPrint,
	LR.Duplex,
	ISNULL(PR.Domestic, 'N') DomesticCalc,
	PR.LetterID,
	PR.BusinessUnit,
	PR.SeqNum,
	PR.AccountNumber,
	PR.PrintedAt,
	PR.IsOnEcorr,
	PR.EcorrDocumentCreatedAt,
	LR.Instructions
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
	AND DeletedAt IS NULL
ORDER BY
	PR.SeqNum

RETURN 0