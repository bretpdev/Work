CREATE PROCEDURE [billing].[GetPendingAccountsForScript]
(
	@ScriptId VARCHAR(10)
)
AS
BEGIN
	SELECT DISTINCT
       PP.PrintProcessingId,
       COALESCE(PD10.DF_SPE_ACC_ID, PP.AccountNumber) AS [AccountNumber],
       COALESCE(PD10.DF_PRS_ID, PD10.DF_PRS_ID) AS [Ssn],
       SF.SourceFile,
	   PP.OnEcorr,
	   PP.EcorrDocumentCreatedAt,
       PP.ArcAddedAt,
       PP.ImagedAt,
       PP.PrintedAt
FROM
       billing.PrintProcessing PP
       INNER JOIN billing.ScriptFiles SF ON PP.ScriptFileId = SF.ScriptFileId
       LEFT OUTER JOIN [CDW].[dbo].[PD10_PRS_NME] PD10 ON PD10.DF_SPE_ACC_ID = PP.AccountNumber
WHERE 
	SF.ScriptID = @ScriptId
	AND DeletedAt IS NULL
	AND 
	(  --Applies to both Ecorr and non Ecorr borrowers
		PP.ArcAddedAt IS NULL
		OR (PP.ImagedAt IS NULL AND PP.ScriptFileId NOT IN (7, 12))
		OR (PP.EcorrDocumentCreatedAt IS NULL AND PP.ScriptFileId NOT IN (7, 12))
		OR
			(  --Borrower is not on Ecorr
				PP.OnEcorr = 0
				AND	PP.PrintedAt IS NULL
				AND PP.ScriptFileId NOT IN (7, 12, 15, 16)
			)
	)

END;