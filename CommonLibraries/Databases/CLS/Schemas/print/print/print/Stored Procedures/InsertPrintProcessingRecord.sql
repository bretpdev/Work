﻿CREATE PROCEDURE [print].[InsertPrintProcessingRecord]
	@ScriptId VARCHAR(10),
	@LetterId VARCHAR(10),
	@LetterData VARCHAR(MAX),
	@AccountNumber VARCHAR(10),
	@CostCenter VARCHAR(5)
AS
	
DECLARE 
	@ONECORR BIT,
	@EMAILADDRESS varchar(256) 


SELECT 
	 @EMAILADDRESS = COALESCE(PH05.DX_CNC_EML_ADR, 'Ecorr@MyCornerStoneLoan.org'),
	 @ONECORR = (CASE  WHEN PH05.DI_VLD_CNC_EML_ADR = 'Y' AND DI_CNC_ELT_OPI = 'Y' THEN 1 ELSE 0 END) 
FROM
	CDW..PD10_PRS_NME PD10
	LEFT JOIN CDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
WHERE
	PD10.DF_SPE_ACC_ID = @AccountNumber




INSERT INTO CLS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT
	@AccountNumber,
	@EMAILADDRESS,
	SD.ScriptDataId,
	@LetterData,
	@CostCenter, 
	SD.DoNotProcessEcorr, 
	@ONECORR,
	CASE WHEN ASD.ScriptDataId IS NOT NULL THEN 1 ELSE 0 END,
	CASE WHEN SD.DocIdId IS NOT NULL THEN 1 ELSE 0 END,
	SUSER_SNAME(),
	GETDATE() 
FROM
	CLS.[print].ScriptData SD
	INNER JOIN CLS.[print].Letters L
		ON L.LetterId = SD.LetterId
	LEFT JOIN CLS.[print].ArcScriptDataMapping ASD
		ON ASD.ScriptDataId = SD.ScriptDataId
	LEFT JOIN
	(
		SELECT
			PP.AccountNumber
		FROM
			CLS.[print].PrintProcessing PP
			INNER JOIN CLS.[print].ScriptData SD
				ON PP.ScriptDataId = SD.ScriptDataId
			INNER JOIN CLS.[print].Letters L
				ON L.LetterId = SD.LetterId
		WHERE
			PP.AccountNumber = @AccountNumber
			AND PP.LetterData = @LetterData
			AND SD.ScriptId = @ScriptId
			AND L.Letter = @LetterId
			AND PP.DeletedAt IS NULL
			AND CAST(PP.AddedAt AS DATE) = CAST(GETDATE() AS DATE)

	) PP
		ON PP.AccountNumber = @AccountNumber
WHERE
	SD.ScriptId = @ScriptId
	AND L.Letter = @LetterId
	AND PP.AccountNumber IS NULL

SELECT SCOPE_IDENTITY()


RETURN 0