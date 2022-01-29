﻿CREATE PROCEDURE [dbo].[spPRNT_GetSpecialHandlingRecsSummary]


AS

--check if the application is in recovery or if population has already been selected previously (if anything is returned then it is in recovery and a further population should not be selected until the currently selected population is printed)
IF (SELECT COUNT(*) FROM dbo.PRNT_DAT_Print WHERE PrintDate IS NOT NULL AND ActualPrintedTime IS NULL) = 0
BEGIN
	--if not in recovery then reserve population by updating PrintDate to today
	UPDATE dbo.PRNT_DAT_Print SET PrintDate = GETDATE() WHERE PrintDate IS NULL
END

--populate temporary table with population
SELECT PrintRecs.LetterID, LtrDat.Duplex, COUNT(*) as TheCount
FROM dbo.PRNT_DAT_Print PrintRecs
JOIN dbo.LTDB_DAT_CentralPrintingDocData LtrDat
	ON PrintRecs.LetterID = LtrDat.[ID]
WHERE PrintRecs.PrintDate IS NOT NULL 
AND ActualPrintedTime IS NULL
AND (LtrDat.Instructions NOT LIKE '' AND LtrDat.Instructions IS NOT NULL)
GROUP BY PrintRecs.LetterID, LtrDat.Duplex
ORDER BY LtrDat.Duplex 

RETURN 0
