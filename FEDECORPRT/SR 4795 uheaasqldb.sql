USE [CLS]
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE [print].ScriptData ADD
	BarcodeOffset int NULL
GO
ALTER TABLE [print].ScriptData SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

GO

/****** Object:  StoredProcedure [print].[GetNextScriptForProcessing]    Script Date: 6/22/2018 9:08:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [print].[GetNextScriptForProcessing]
	@ScriptDataIds ProcessedScriptDataIds READONLY
AS
BEGIN TRANSACTION
DECLARE @ScriptDataId int = NULL
	
SELECT
	@ScriptDataId = sd.ScriptDataId
FROM
	[print].ScriptData SD
	INNER JOIN
	(
		SELECT
			MIN(SD.[Priority]) [Priority]
		FROM
			[print].ScriptData SD
			INNER JOIN
			(
				SELECT
					CAST(MIN(LastProcessed) AS DATE) [LastProcessed]
				FROM
					[print].ScriptData
				WHERE
					Active = 1
			)LP 
				ON LP.LastProcessed = CAST(SD.LastProcessed AS DATE)
			LEFT JOIN 
			(
				SELECT 
					CAST(context_info AS INT) [ScriptDataId]
				FROM 
					sys.dm_exec_sessions
				WHERE 
					context_info IS NOT NULL AND  context_info != 0x
				GROUP BY 
					cast(context_info as INT)
			) CON
				ON CON.ScriptDataId = SD.ScriptDataId 
		WHERE
			CON.ScriptDataId IS NULL
			AND SD.ScriptDataId NOT IN (SELECT * FROM @ScriptDataIds)
	)POP
		ON POP.[Priority] = SD.[Priority]

IF @ScriptDataId IS NOT NULL
BEGIN
	DECLARE @binary VARBINARY(128) = CAST(@ScriptDataId AS VARBINARY(128))
	SET CONTEXT_INFO @binary 
END

SELECT 
	SD.ScriptDataId,
	SD.ScriptID,
	SD.SourceFile,
	L.Letter,
	DID.DocIdName,
	FH.FileHeader,
	FH.StateIndex,
	FH.AccountNumberIndex,
	FH.CostCenterCodeIndex,
	SD.ProcessAllFiles,
	SD.[Priority],
	SD.DoNotProcessEcorr,
	SD.AddBarCodes,
	BFHD.BillDueDateIndex,
	BFHD.BillSeqIndex,
	BFHD.TotalDueIndex,
	BFHD.BillCreateDateIndex,
	SD.IsEndorser,
	SD.EndorsersBorrowerSSNIndex,
	SD.BarcodeOffset
FROM
	[print].ScriptData SD
	INNER JOIN [print].FileHeaders FH
		ON FH.FileHeaderId = SD.FileHeaderId
	INNER JOIN [print].Letters L
		ON L.LetterId = SD.LetterId
	LEFT JOIN [print].DocIds DID
		ON DID.DocIdId = SD.DocIdId
	LEFT JOIN [print].BillingFileHeaderData BFHD
		ON BFHD.FileHeaderId = FH.FileHeaderId
WHERE
	SD.ScriptDataId = @ScriptDataId
	
COMMIT
RETURN 0

GO
/*
   Friday, June 22, 20189:14:04 AM
   User: 
   Server: uheaasqldb
   Database: CLS
   Application: 
*/


