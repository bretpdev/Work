USE ULS
GO

BEGIN TRANSACTION

	DECLARE @FileHeaderId INT
	DECLARE @ScriptdataId INT
	DECLARE @LetterId INT
	DECLARE @CommentId INT
	DECLARE @Comment VARCHAR(MAX) ='Letter sent to reference for skip assistance'
	DECLARE @ArcId INT
	DECLARE @HeaderId INT
	DECLARE @ArcScriptDataMapping INT
	DECLARE @Priority INT
	DECLARE @SourceFile VARCHAR(50) = NULL --can be null if sas inserts directly into db
	DECLARE @ScriptId VARCHAR(50) = 'SKPBRWATRN' --previous scriptId
	DECLARE @LetterName VARCHAR(20) = 'ATNYSKP' --LetterId from letter tracking
	DECLARE @Pages INT = 2
	DECLARE @FileHeaderText VARCHAR(MAX) = 'BF_SSN,BF_RFR,ACS_CODE,ACSKEY,RNAME,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP_CDE,DM_FGN_CNY,STU_SSN,BNAME,DF_SPE_ACC_ID,BKY_CASE,B_DX_STR_ADR_1,B_DX_STR_ADR_2,B_DM_CT,B_DC_DOM_ST,B_DF_ZIP_CDE,B_DM_FGN_CNY,DN_PHN,STATE_IND,COST_CENTER_CODE'
	DECLARE @FileHeaderState INT = 8
	DECLARE @FileHeaderAcc INT = 1
	DECLARE @FileHeaderCost INT = 0 --If your letter doesnt have a cost center field, use 0
	DECLARE @Arc VARCHAR(5) = 'KATNY'--Add the Arc ID here
	DECLARE @ActivityType VARCHAR(2) = 'LT'
	DECLARE @ActivityContact VARCHAR(2) = '33'
	DECLARE @EcorrId INT
	DECLARE @SubjectLine VARCHAR(MAX) = ''
	DECLARE @EmailComment VARCHAR(MAX) = ''


	-----Add letter to Letters table------------------------------------------------------------------------
	SELECT @LetterId = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = @LetterName)
	IF @LetterId IS NULL
		BEGIN
			INSERT INTO ULS.[print].[Letters](Letter, PagesPerDocument)
			VALUES(@LetterName, @Pages)

			SELECT @LetterId = @@IDENTITY
		END

	PRINT CONCAT('Letter ID ',@LetterId)

	-----Add a file header----------------------------------------------------------------------------------

	SELECT @FileHeaderId = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderText)
	IF @FileHeaderId IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderText, @FileHeaderState, @FileHeaderAcc, @FileHeaderCost)

			SELECT @FileHeaderId = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority)

	-----Add the letter to the ScriptData table-------------------------------------------------------------
	SELECT @ScriptdataId = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId AND SourceFile = @SourceFile AND LetterId = @LetterId AND FileHeaderId = @FileHeaderId)
	IF @ScriptdataId IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active)
			VALUES(@ScriptId, @SourceFile, @LetterId, @FileHeaderId, 0, 0, @Priority, 1, 1, SUSER_SNAME(), GETDATE(), 1)

			SELECT @ScriptdataId = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId)

	-----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF @Comment != ''
	BEGIN
		SELECT @CommentId = (SELECT CommentId FROM ULS.[print].Comments WHERE Comment = @Comment)
		IF @CommentId IS NULL
			BEGIN
				INSERT INTO ULS.[print].Comments(Comment)
				VALUES(@Comment)

				SELECT @CommentId = @@IDENTITY
			END
		PRINT CONCAT('Comment ID ',@CommentId)
	END

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	IF @Arc != ''
	BEGIN
		PRINT CONCAT('ARC ',@Arc)
		SELECT @ArcId = (SELECT ArcId FROM ULS.[print].Arcs WHERE Arc = @Arc)

		IF @ArcId IS NULL
			BEGIN
				INSERT INTO ULS.[print].Arcs(Arc, ActivityType, ActivityContact)
				VALUES(@Arc, @ActivityType, @ActivityContact)

				SELECT @ArcId = @@IDENTITY
			END
		PRINT CONCAT('Arc ID ',@ArcId)
	END

	IF @Arc != '' AND @Comment != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
		IF @ArcScriptDataMapping IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId, @ArcId, 1, @CommentId)

				SELECT @ArcScriptDataMapping = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)
	END
	-------Add the header name to the HeaderNames table-------------------------------------------------------
	--INSERT INTO [print].HeaderNames(HeaderName)
	--VALUES('TILP')

	--SELECT @HeaderId = @@IDENTITY
	--PRINT CONCAT('Header ID ',@HeaderId)

	-------Tie the HeaderNames value to the ArcScriptDataMapping table----------------------------------------
	--INSERT INTO [print].ArcLoanHeaderMapping(ArcScriptFileMappingId, HeaderNameId)
	--VALUES(@ArcScriptDataMapping, @HeaderId)

	SELECT @EcorrId = (SELECT LetterId FROM EcorrUheaa..Letters WHERE Letter = @LetterName)
	IF @EcorrId IS NULL 
		BEGIN
			INSERT INTO EcorrUheaa..Letters(Letter,LetterTypeId,DocId,Viewable,ReportDescription,ReportName,Viewed,MainframeRegion,SubjectLine,DocSource,DocComment,WorkFlow,DocDelete,Active)
			VALUES(@LetterName,1,'XELT','Y','Description','Name','N','RSUT',@SubjectLine,'IMPORT',@EmailComment,'N','N',1)
		END
	
COMMIT TRANSACTION
--ROLLBACK TRANSACTION