
CREATE PROCEDURE [print].[GetPendingAccountsForScript]
(
	@ScriptDataId INT
)
AS
BEGIN
	SELECT DISTINCT
		PP.PrintProcessingId,
		PP.AccountNumber,
		PP.EmailAddress,
		PD10.DF_PRS_ID AS BF_SSN,
		pp.LetterData,
		PP.ArcAddProcessingId,
		PP.ArcNeeded,
		PP.ImagedAt,
		PP.ImagingNeeded,
		PP.PrintedAt,
		PP.EcorrDocumentCreatedAt,
		PP.OnEcorr
	FROM
		[print].PrintProcessing PP
		INNER JOIN [print].ScriptData SD
			ON SD.ScriptDataId = PP.ScriptDataId
		INNER JOIN [print].FileHeaders FH
			ON FH.FileHeaderId = SD.FileHeaderId
		INNER JOIN [print].Letters L 
			ON L.LetterId = SD.LetterId
		LEFT JOIN [print].DocIds D
			ON D.DocIdId = SD.DocIdId
		LEFT JOIN [UDW].[dbo].PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = PP.AccountNumber
WHERE 
       PP.ScriptDataId = @ScriptDataId
       AND (
			  (PP.ArcAddProcessingId IS NULL AND PP.ArcNeeded = 1)
              OR (PP.PrintedAt IS NULL AND pp.OnEcorr = 0)
              OR (PP.ImagedAt IS NULL AND  PP.ImagingNeeded = 1)
			  OR (PP.EcorrDocumentCreatedAt IS NULL)
		   )
		AND DeletedAt IS NULL
		AND PP.AccountNumber != ''
ORDER BY
	pp.AccountNumber
END;