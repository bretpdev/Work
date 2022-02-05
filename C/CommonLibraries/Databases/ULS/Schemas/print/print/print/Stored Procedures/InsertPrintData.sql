CREATE PROCEDURE [print].[InsertPrintData]
	@Letter VARCHAR(10),
	@ScriptId VARCHAR(10),
	@AccountNumber CHAR(10),
	@LetterData VARCHAR(MAX),
	@IsOnEcorr BIT,
	@AddedBy varchar(256)
AS
	DECLARE @LetterId INT = (SELECT LetterId FROM [print].Letters where Letter = @Letter)
	
	DECLARE @ScriptDataId int = (SELECT
		ScriptDataId
	FROM 
		[print].ScriptData SD
	WHERE 
		SD.LetterId = @LetterId
		AND SD.ScriptID = @ScriptId)

	IF(@ScriptDataId < 1)
		BEGIN
			RAISERROR('InsertPrintData RETURNED 0 RECORDS FOR LETTER:%s', 16, 1, @Letter) 
		END

	INSERT INTO [print].PrintProcessing(AccountNumber, ScriptDataId, LetterData, OnEcorr, AddedBy)
	VALUES(@AccountNumber, @ScriptDataId, @LetterData, @IsOnEcorr, @AddedBy)

	IF(@@ROWCOUNT = 0)
		BEGIN
			DECLARE @ECORRSTR CHAR(1) = CAST(@IsOnEcorr AS CHAR(1))
			RAISERROR('InsertPrintData Unable to insert data Letter:%s, @AccountNumber:%s, @ScriptId:%s, @LetterData:%s, @IsOnEcorr:%s, @AddedBy:%s', 16, 1, @Letter, @AccountNumber, @ScriptId, 
			@LetterData, @ECORRSTR, @AddedBy) 
		END
RETURN 0
