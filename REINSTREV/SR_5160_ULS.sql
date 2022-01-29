USE ULS
GO


DECLARE @ScriptId VARCHAR(10) = 'REINSTREV'

DECLARE @Header VARCHAR(100) = 'SSN,LastName,FirstName,Address1,Address2,City,State,ZIP,Country,KeyLine,SchoolName,SchAddress1,SchAddress2,SchAddress3,SchCity,SchState,SchZip,SchCountry,AccountNumber,CostCenterCode'
DECLARE @HeaderId INT = NULL

SELECT
	@HeaderId = FileHeaderId
FROM
	[print].FileHeaders
WHERE
	FileHeader = @Header

IF @HeaderId IS NULL
BEGIN
	INSERT INTO [print].FileHeaders (FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
	VALUES (@Header, 6, 18, 19)

	SET @HeaderId = SCOPE_IDENTITY()
END

--DECLARE @Arc VARCHAR(5) = 'DRHBA'
--DECLARE @ArcId INT = NULL

--IF NOT EXISTS(SELECT * FROM [print].Arcs WHERE Arc = @Arc)
--	INSERT INTO [print].Arcs (Arc) VALUES (@Arc)

--SELECT @ArcId = ArcId FROM [print].Arcs WHERE Arc = @Arc

--DECLARE @Comment VARCHAR(50) = 'Rehab agreement sent to borrower.'
--DECLARE @CommentId INT = NULL

--IF NOT EXISTS(SELECT * FROM [print].Comments WHERE Comment = @Comment)
--	INSERT INTO [print].Comments (Comment) VALUES (@Comment)

--SELECT @CommentId = CommentId FROM [print].Comments WHERE Comment = @Comment

DECLARE @APVB VARCHAR(10) = 'REINAPVB'
DECLARE @ApvbId INT = NULL
DECLARE @Priority INT = (SELECT MAX([Priority]) + 1 FROM [print].ScriptData)

IF NOT EXISTS(SELECT * FROM [print].Letters WHERE Letter = @APVB)
	INSERT INTO [print].Letters ([Letter]) VALUES (@APVB)

SELECT @ApvbId = LetterId FROM [print].Letters WHERE Letter = @APVB


IF NOT EXISTS(SELECT * FROM [print].ScriptData WHERE ScriptId = @ScriptId AND LetterId = @ApvbId)
BEGIN
	INSERT INTO [print].ScriptData (Scriptid, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, Active, CheckForCoBorrower)
	VALUES  (@ScriptId, '', @ApvbId,  @HeaderId, 0, 0, @Priority, 1, 1, 1, 0)
END


DECLARE @APVS VARCHAR(10) = 'REINAPVS'
DECLARE @ApvsId INT = NULL
SET @Priority = (SELECT MAX([Priority]) + 1 FROM [print].ScriptData)


IF NOT EXISTS(SELECT * FROM [print].Letters WHERE Letter = @APVS)
	INSERT INTO [print].Letters ([Letter]) VALUES (@APVS)

SELECT @ApvsId = LetterId FROM [print].Letters WHERE Letter = @APVS


IF NOT EXISTS(SELECT * FROM [print].ScriptData WHERE ScriptId = @ScriptId AND LetterId = @ApvsId)
BEGIN
	INSERT INTO [print].ScriptData (Scriptid, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, Active, CheckForCoBorrower)
	VALUES  (@ScriptId, '', @ApvsId,  @HeaderId, 0, 0, @Priority, 1, 1, 1, 0)
END

DECLARE @DENB VARCHAR(10) = 'REINDENB'
DECLARE @DenbId INT = NULL
SET @Priority = (SELECT MAX([Priority]) + 1 FROM [print].ScriptData)

IF NOT EXISTS(SELECT * FROM [print].Letters WHERE Letter = @DENB)
	INSERT INTO [print].Letters ([Letter]) VALUES (@DENB)

SELECT @DenbId = LetterId FROM [print].Letters WHERE Letter = @DENB


IF NOT EXISTS(SELECT * FROM [print].ScriptData WHERE ScriptId = @ScriptId AND LetterId = @DenbId)
BEGIN
	INSERT INTO [print].ScriptData (Scriptid, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, Active, CheckForCoBorrower)
	VALUES  (@ScriptId, '', @DenbId,  @HeaderId, 0, 0, @Priority, 1, 1, 1, 0)
END


DECLARE @DENS VARCHAR(10) = 'REINDENS'
DECLARE @DensId INT = NULL
SET @Priority = (SELECT MAX([Priority]) + 1 FROM [print].ScriptData)

IF NOT EXISTS(SELECT * FROM [print].Letters WHERE Letter = @DENS)
	INSERT INTO [print].Letters ([Letter]) VALUES (@DENS)

SELECT @DensId = LetterId FROM [print].Letters WHERE Letter = @DENS


IF NOT EXISTS(SELECT * FROM [print].ScriptData WHERE ScriptId = @ScriptId AND LetterId = @DensId)
BEGIN
	INSERT INTO [print].ScriptData (Scriptid, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, Active, CheckForCoBorrower)
	VALUES  (@ScriptId, '', @DensId,  @HeaderId, 0, 0, @Priority, 1, 1, 1, 0)
END

GO

ALTER PROCEDURE [print].[InsertOneLinkPrintProcessingRecord]
	@ScriptId VARCHAR(10),
	@LetterId VARCHAR(10),
	@LetterData VARCHAR(MAX),
	@AccountNumber VARCHAR(10),
	@CostCenter VARCHAR(5)
AS
	

INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT
	@AccountNumber,
	'',
	SD.ScriptDataId,
	@LetterData,
	@CostCenter, 
	SD.DoNotProcessEcorr, 
	0,
	CASE WHEN ASD.ScriptDataId IS NOT NULL THEN 1 ELSE 0 END,
	CASE WHEN SD.DocIdId IS NOT NULL THEN 1 ELSE 0 END,
	SUSER_SNAME(),
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