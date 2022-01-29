CREATE PROCEDURE [dbo].[spPRNT_GetNonSpecialHandlingRecsSummary]


AS

--check if the application is in recovery (if anything is returned then it is in recovery and a further population should not be selected until the currently selected population is printed)
IF (SELECT COUNT(*) FROM dbo.PRNT_DAT_Print WHERE PrintDate IS NOT NULL AND ActualPrintedTime IS NULL) = 0
BEGIN
	--if not in recovery then reserve population by updating PrintDate to today
	UPDATE dbo.PRNT_DAT_Print SET PrintDate = GETDATE() WHERE PrintDate IS NULL
END

--populate population
SELECT LtrDat.UHEAACostCenter,
	CASE LtrDat.Duplex
		WHEN 1 THEN CEILING(Pages / 2)
		ELSE Pages
	END AS PagesToPrint,
	LtrDat.Duplex,
	CASE PrintRecs.Domestic
		WHEN NULL THEN 'N'
		ELSE PrintRecs.Domestic
	END AS DomesticCalc,
	COUNT(*) as TheCount
FROM dbo.PRNT_DAT_Print PrintRecs
JOIN dbo.LTDB_DAT_CentralPrintingDocData LtrDat
	ON PrintRecs.LetterID = LtrDat.[ID]
WHERE PrintRecs.PrintDate IS NOT NULL 
AND ActualPrintedTime IS NULL
AND (LtrDat.Instructions LIKE '' OR LtrDat.Instructions IS NULL) 
GROUP BY LtrDat.UHEAACostCenter,
	CASE LtrDat.Duplex
		WHEN 1 THEN CEILING(Pages / 2)
		ELSE Pages
	END,
	LtrDat.Duplex,
	CASE PrintRecs.Domestic
		WHEN NULL THEN 'N'
		ELSE PrintRecs.Domestic
	END
ORDER BY LtrDat.Duplex
