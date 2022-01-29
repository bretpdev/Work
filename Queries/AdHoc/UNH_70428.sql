USE [ULS]
GO
/****** Object:  StoredProcedure [print].[InsertOneLinkPrintProcessingRecord]    Script Date: 2/4/2021 3:36:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [print].[InsertOneLinkPrintProcessingRecord]
	@ScriptId VARCHAR(10),
	@LetterId VARCHAR(10),
	@LetterData VARCHAR(MAX),
	@AccountNumber VARCHAR(10),
	@CostCenter VARCHAR(10),
	@RunBy VARCHAR(50) = NULL
AS

IF @RunBy IS NULL
	BEGIN
		SET @RunBy = SUSER_SNAME()
	END

INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT
	@AccountNumber,
	'Ecorr@Uheaa.org',
	SD.ScriptDataId,
	@LetterData,
	@CostCenter, 
	SD.DoNotProcessEcorr, 
	0,  --OneLink accounts cannot be on Ecorr
	CASE WHEN ASD.ScriptDataId IS NOT NULL THEN 1 ELSE 0 END,
	CASE WHEN SD.DocIdId IS NOT NULL THEN 1 ELSE 0 END,
	@RunBy,
	GETDATE() 
FROM
	ULS.[print].ScriptData SD
	INNER JOIN ULS.[print].Letters L
		ON L.LetterId = SD.LetterId
	LEFT JOIN ULS.[print].ArcScriptDataMapping ASD
		ON ASD.ScriptDataId = SD.ScriptDataId
	LEFT JOIN
	(
		SELECT
			PP.AccountNumber
		FROM
			ULS.[print].PrintProcessing PP
			INNER JOIN ULS.[print].ScriptData SD
				ON PP.ScriptDataId = SD.ScriptDataId
			INNER JOIN ULS.[print].Letters L
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
