
CREATE PROCEDURE [print].[LoadPrintData]
(
	@ScriptDataId int,
	@SourceFile VARCHAR(100),
	@AddedBy VARCHAR(50)
)

AS

BEGIN TRANSACTION
DECLARE @ERROR INT = 0 
DECLARE @AccountNumberLocation INT = (
	SELECT
		FH.AccountNumberIndex
	FROM
		[print].ScriptData SD
		INNER JOIN [print].FileHeaders FH
			ON FH.FileHeaderId = SD.FileHeaderId
	WHERE
		SD.ScriptDataId = @ScriptDataId)

DECLARE @CostCenterCodeIndex INT = (
	SELECT
		FH.CostCenterCodeIndex
	FROM
		[print].ScriptData SD
		INNER JOIN [print].FileHeaders FH
			ON FH.FileHeaderId = SD.FileHeaderId
	WHERE
		SD.ScriptDataId = @ScriptDataId)

SET @ERROR = @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR OCCURED PULLING THE ACCOUNT NUMBER INDEX FOR SCRIPTDATAID:%d', 16, 1, @ScriptDataId) 
END

		

--Insert data into Print.PrintProcessing from _BulkLoad
INSERT INTO [print].PrintProcessing
(
	AccountNumber, 
	ScriptDataId,
	SourceFile,
	LetterData,
	ArcNeeded,
	ImagingNeeded, 
	AddedBy,
	CostCenter
) 
SELECT DISTINCT
	dbo.SplitAndRemoveQuotes(BL.LineData, ',', @AccountNumberLocation, 1) as AccountNumber, 
	@ScriptDataId,
	@SourceFile,
	BL.LineData,
	CASE WHEN ASM.ArcId IS NOT NULL THEN 1 ELSE 0 END,
	CASE WHEN DI.DocIdId IS NOT NULL THEN 1 ELSE 0 END,
	@AddedBy,
	dbo.SplitAndRemoveQuotes(BL.LineData, ',', @CostCenterCodeIndex, 1) as CostCenter
FROM
	[print]._BulkLoad BL
	INNER JOIN [print].ScriptData SD
		ON SD.ScriptDataId = @ScriptDataId
	LEFT JOIN [print].DocIds DI
		ON DI.DocIdId = SD.DocIdId
	LEFT JOIN [print].ArcScriptDataMapping ASM
		ON ASM.ScriptDataId = SD.ScriptDataId
ORDER BY
	AccountNumber

SET @ERROR = @ERROR + @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR CREATING [print].PrintProcessing FOR SCRIPTDATAID:%d', 16, 1, @ScriptDataId) 
END

UPDATE 
	S
SET
	S.EmailAddress = COALESCE(OQ.EmailAddress, 'ECORR@UHEAA.ORG'),
	S.OnEcorr = COALESCE(OQ.OnEcorr,0)
FROM
	[print].PrintProcessing S
	LEFT JOIN 
	(
		SELECT
			 PH05.DF_SPE_ID AS AccountNumber,
			 PH05.DX_CNC_EML_ADR AS EmailAddress,
			 CASE
				WHEN BFH.BillingFileHeaderDataId IS NULL AND PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y' THEN 1
				WHEN BFH.BillingFileHeaderDataId IS NOT NULL AND PH05.DI_CNC_EBL_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y' THEN 1
				ELSE 0
			END AS OnEcorr
		FROM 
			UDW..PH05_CNC_EML PH05
			LEFT JOIN [print].ScriptData SD 
				ON SD.ScriptDataId = @ScriptDataId
			LEFT JOIN [print].BillingFileHeaderData BFH
				ON BFH.FileHeaderId = SD.FileHeaderId
	) OQ
		ON S.AccountNumber = OQ.AccountNumber
	WHERE
		S.EmailAddress IS NULL

SET @ERROR = @ERROR + @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR UPDATING [print].PrintProcessing FOR SCRIPTDATAID:%d', 16, 1, @ScriptDataId) 
END

DELETE FROM [print]._BulkLoad

SET @ERROR = @ERROR + @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR OCCURED DELETING _BULKLOAD SCRIPTDATAID:%d', 16, 1, @ScriptDataId) 
END
ELSE
	COMMIT